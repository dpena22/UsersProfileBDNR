using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using Dominio;
using System.Text.Json;
using UsersProfileBDNR.DTOs;
using UsersProfileBDNR.DTOs.Dominio.DTOs;
using StackExchange.Redis;

namespace UsersProfileBDNR.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly IMongoCollection<UserAccount> _accounts;
        private readonly IMongoCollection<UserProfile> _profiles;
        private readonly IDatabase _cache;

        public UsersController(IConnectionMultiplexer redis)
        {
            var client = new MongoClient("mongodb://localhost:27017");
            var db = client.GetDatabase("DuolingoDB");

            _accounts = db.GetCollection<UserAccount>("UserAccounts");
            _profiles = db.GetCollection<UserProfile>("UserProfiles");
            _cache = redis.GetDatabase();
        }

        // ---------------------- CREATE USER ----------------------
        [HttpPost("create")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDTO dto)
        {
            var userId = ObjectId.GenerateNewId().ToString();

            var account = MapToAccount(dto, userId);
            var profile = MapToProfile(dto, userId);

            await _accounts.InsertOneAsync(account);
            await _profiles.InsertOneAsync(profile);

            var response = BuildUserResponse(account, profile);

            return Ok(response);
        }

        // ---------------------- GET USER ----------------------
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUser(string userId)
        {
            string redisKey = $"user:{userId}:summary";

            var cached = await _cache.StringGetAsync(redisKey);
            if (cached.HasValue)
            {
                var dto = JsonSerializer.Deserialize<UserResponseDTO>(cached!);
                return Ok(dto);
            }

            var account = await _accounts.Find(a => a.Id == userId).FirstOrDefaultAsync();
            var profile = await _profiles.Find(p => p.UserId == userId).FirstOrDefaultAsync();

            if (account == null && profile == null)
                return NotFound($"No user found with id {userId}");

            var response = BuildUserResponse(account, profile);
            var json = JsonSerializer.Serialize(response);

            await _cache.StringSetAsync(redisKey, json, TimeSpan.FromSeconds(60));

            return Ok(response);
        }

        // ---------------------- SUMMARY FOR RECOMMENDER ----------------------
        [HttpGet("{userId}/summary-for-recommender")]
        public async Task<IActionResult> GetSummaryForRecommender(string userId)
        {
            var profile = await _profiles.Find(p => p.UserId == userId).FirstOrDefaultAsync();

            if (profile == null)
                return NotFound($"No profile found for user {userId}");

            var extra = profile.Extra?.AsBsonDocument ?? new BsonDocument();

            var preferencesDoc = extra.Contains("preferences")
                ? extra["preferences"].AsBsonDocument
                : new BsonDocument();

            var tagsArray = preferencesDoc.Contains("content_tags")
                ? preferencesDoc["content_tags"].AsBsonArray
                : new BsonArray();

            var contentTags = tagsArray
                .Where(t => t.IsBsonDocument)
                .Select(t =>
                {
                    var d = t.AsBsonDocument;

                    double weight = 0;
                    if (d.Contains("weight"))
                        double.TryParse(d["weight"].ToString(), out weight);

                    return new
                    {
                        id = d.Contains("id") ? d["id"].ToString() : null,
                        weight,
                        source = d.Contains("source") ? d["source"].ToString() : null
                    };
                }).ToList();

            var lang = profile.Languages?.FirstOrDefault();

            var summary = new
            {
                user_id = profile.UserId,
                display_name = profile.DisplayName,
                language = lang?.Code,
                course_id = lang != null ? $"{lang.Code}_es" : null,
                level = lang?.Level,
                xp = lang?.XP,
                current_streak = profile.Streak?.CurrentStreak,
                last_practice_at = profile.Streak?.LastUpdated?.ToUniversalTime(),
                plus_active = profile.PlusStatus?.IsActive,

                preferences = new
                {
                    content_tags = contentTags
                }
            };

            return Ok(summary);
        }

        // ==================================================================
        // -------------------------- MAPPERS -------------------------------
        // ==================================================================

        private UserAccount MapToAccount(CreateUserDTO dto, string userId)
        {
            return new UserAccount
            {
                Id = userId,
                Username = dto.Username,
                Email = dto.Email,
                TwoFactorEnabled = dto.TwoFactorEnabled,

                Privacy = dto.Privacy != null
                    ? new PrivacySettings
                    {
                        ShowProfile = dto.Privacy.ShowProfile,
                        ShowFollowers = dto.Privacy.ShowFollowers,
                        ShowActivity = dto.Privacy.ShowActivity
                    }
                    : new PrivacySettings(),

                Notifications = dto.Notifications != null
                    ? new NotificationSettings
                    {
                        EmailNotifications = dto.Notifications.EmailNotifications,
                        PushNotifications = dto.Notifications.PushNotifications,
                        Extra = DeserializeExtra(dto.Notifications.Extra)
                    }
                    : new NotificationSettings(),

                Extra = DeserializeExtra(dto.AccountExtra)
            };
        }

        private UserProfile MapToProfile(CreateUserDTO dto, string userId)
        {
            return new UserProfile
            {
                UserId = userId,
                DisplayName = dto.DisplayName,
                PhotoUrl = dto.PhotoUrl,

                Languages = dto.Languages?.Select(l => new UserLanguage
                {
                    Code = l.Code,
                    Level = l.Level,
                    XP = l.XP,
                    Active = l.Active
                }).ToList(),

                Streak = dto.Streak != null
                    ? new StreakInfo
                    {
                        CurrentStreak = dto.Streak.CurrentStreak,
                        MaxStreak = dto.Streak.MaxStreak,
                        LastUpdated = dto.Streak.LastUpdated ?? DateTime.UtcNow
                    }
                    : new StreakInfo(),

                TotalXP = dto.TotalXP,
                Achievements = dto.Achievements,
                Friends = dto.Friends,
                Followers = dto.Followers,

                PlusStatus = dto.PlusStatus != null
                    ? new DuolingoPlusStatus
                    {
                        IsActive = dto.PlusStatus.IsActive,
                        Expiration = dto.PlusStatus.Expiration,
                        Extra = DeserializeExtra(dto.PlusStatus.Extra)
                    }
                    : new DuolingoPlusStatus(),

                Extra = DeserializeExtra(dto.ProfileExtra)
            };
        }

        private BsonDocument DeserializeExtra(object? extra)
        {
            if (extra == null)
                return new BsonDocument();

            if (extra is BsonDocument bson)
                return bson;

            var json = JsonSerializer.Serialize(extra);
            return BsonDocument.Parse(json);
        }

        private UserResponseDTO BuildUserResponse(UserAccount? account, UserProfile? profile)
        {
            return new UserResponseDTO
            {
                UserId = account?.Id ?? profile?.UserId,

                Account = account != null ? new AccountResponseDTO
                {
                    Username = account.Username,
                    Email = account.Email,
                    TwoFactorEnabled = account.TwoFactorEnabled,
                    Extra = account.Extra?.ToPlainObject()
                } : null,

                Profile = profile != null ? new ProfileResponseDTO
                {
                    DisplayName = profile.DisplayName,
                    PhotoUrl = profile.PhotoUrl,

                    Languages = profile.Languages?.Select(l => new LanguageResponseDTO
                    {
                        Code = l.Code,
                        Level = l.Level,
                        XP = l.XP
                    }).ToList(),

                    Streak = profile.Streak != null ? new StreakResponseDTO
                    {
                        CurrentStreak = profile.Streak.CurrentStreak,
                        MaxStreak = profile.Streak.MaxStreak,
                        LastUpdated = profile.Streak.LastUpdated?.ToString("o")
                    } : null,

                    TotalXP = profile.TotalXP,
                    Achievements = profile.Achievements,
                    Friends = profile.Friends,
                    Followers = profile.Followers,
                    PlusActive = profile.PlusStatus?.IsActive,
                    Extra = profile.Extra?.ToPlainObject()
                } : null
            };
        }
    }
}
