
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json;
namespace Dominio
{
    [BsonIgnoreExtraElements]
    public class UserAccount
    {
        public string? Id { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public bool? TwoFactorEnabled { get; set; }
        public PrivacySettings? Privacy { get; set; } = new();
        public NotificationSettings? Notifications { get; set; } = new();
        public BsonDocument Extra { get; set; } = new BsonDocument();
    }
    [BsonIgnoreExtraElements]
    public class PrivacySettings
    {
        public bool? ShowProfile { get; set; } = true;
        public bool? ShowFollowers { get; set; } = true;
        public bool? ShowActivity { get; set; } = true;
    }
    [BsonIgnoreExtraElements]
    public class NotificationSettings
    {
        public bool? EmailNotifications { get; set; } = true;
        public bool? PushNotifications { get; set; } = true;
        public BsonDocument Extra { get; set; } = new BsonDocument();
    }

    [BsonIgnoreExtraElements]
    public class UserProfile
    {
        public string? UserId { get; set; }
        public string? DisplayName { get; set; }
        public string? PhotoUrl { get; set; }
        public List<UserLanguage>? Languages { get; set; } = new();
        public StreakInfo? Streak { get; set; } = new();
        public int? TotalXP { get; set; }
        public List<string>? Achievements { get; set; } = new();
        public List<string>? Friends { get; set; } = new();
        public List<string>? Followers { get; set; } = new();
        public DuolingoPlusStatus? PlusStatus { get; set; } = new();
        public BsonDocument Extra { get; set; } = new BsonDocument();
    }
    [BsonIgnoreExtraElements]
    public class UserLanguage
    {
        public string? Code { get; set; }
        public int? Level { get; set; }
        public int? XP { get; set; }
        public bool? Active { get; set; }
    }
    [BsonIgnoreExtraElements]
    public class StreakInfo
    {
        public int? CurrentStreak { get; set; }
        public int? MaxStreak { get; set; }
        public DateTime? LastUpdated { get; set; } = DateTime.UtcNow;
    }
    [BsonIgnoreExtraElements]
    public class DuolingoPlusStatus
    {
        public bool? IsActive { get; set; }
        public DateTime? Expiration { get; set; }
        public BsonDocument Extra { get; set; } = new BsonDocument();
    }


}
