using System.Collections.Generic;

namespace UsersProfileBDNR.DTOs
{
    public class CreateUserDTO
    {
        public string? Username { get; set; }
        public string? Email { get; set; }
        public bool? TwoFactorEnabled { get; set; }
        public PrivacySettingsDTO? Privacy { get; set; }
        public NotificationSettingsDTO? Notifications { get; set; }
        public object? AccountExtra { get; set; }

        public string? DisplayName { get; set; }
        public string? PhotoUrl { get; set; }
        public List<UserLanguageDTO>? Languages { get; set; }
        public StreakInfoDTO? Streak { get; set; }
        public int? TotalXP { get; set; }
        public List<string>? Achievements { get; set; }
        public List<string>? Friends { get; set; }
        public List<string>? Followers { get; set; }
        public DuolingoPlusStatusDTO? PlusStatus { get; set; }
        public object? ProfileExtra { get; set; }
    }

    public class UserLanguageDTO
    {
        public string? Code { get; set; }
        public int? Level { get; set; }
        public int? XP { get; set; }
        public bool? Active { get; set; }
    }

    public class StreakInfoDTO
    {
        public int? CurrentStreak { get; set; }
        public int? MaxStreak { get; set; }
        public DateTime? LastUpdated { get; set; }
    }

    public class DuolingoPlusStatusDTO
    {
        public bool? IsActive { get; set; }
        public DateTime? Expiration { get; set; }
        public object? Extra { get; set; }
    }

    public class PrivacySettingsDTO
    {
        public bool? ShowProfile { get; set; }
        public bool? ShowFollowers { get; set; }
        public bool? ShowActivity { get; set; }
    }

    public class NotificationSettingsDTO
    {
        public bool? EmailNotifications { get; set; }
        public bool? PushNotifications { get; set; }
        public object? Extra { get; set; }
    }

    /////////////
    //RESPONSE
    /////////////
    ///


namespace Dominio.DTOs
    {
        public class UserResponseDTO
        {
            public string UserId { get; set; }
            public AccountResponseDTO Account { get; set; }
            public ProfileResponseDTO Profile { get; set; }
        }

        public class AccountResponseDTO
        {
            public string Username { get; set; }
            public string Email { get; set; }
            public bool? TwoFactorEnabled { get; set; }
            public object Extra { get; set; }
        }

        public class ProfileResponseDTO
        {
            public string DisplayName { get; set; }
            public string PhotoUrl { get; set; }
            public List<LanguageResponseDTO> Languages { get; set; }
            public StreakResponseDTO Streak { get; set; }
            public int? TotalXP { get; set; }
            public List<string> Achievements { get; set; }
            public List<string> Friends { get; set; }
            public List<string> Followers { get; set; }
            public bool? PlusActive { get; set; }
            public object Extra { get; set; }
        }

        public class LanguageResponseDTO
        {
            public string Code { get; set; }
            public int? Level { get; set; }
            public int? XP { get; set; }
        }

        public class StreakResponseDTO
        {
            public int? CurrentStreak { get; set; }
            public int? MaxStreak { get; set; }
            public string LastUpdated { get; set; }
        }
    }

}

