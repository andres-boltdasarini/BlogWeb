using System.ComponentModel.DataAnnotations;

namespace BlogWeb.ViewModels
{
    public class EditUserViewModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Имя пользователя обязательно")]
        [Display(Name = "Имя пользователя")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email обязателен")]
        [EmailAddress(ErrorMessage = "Неверный формат email")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Отображаемое имя")]
        public string DisplayName { get; set; }

        [Display(Name = "О себе")]
        public string Bio { get; set; }

        [Display(Name = "URL аватара")]
        [Url(ErrorMessage = "Неверный URL")]
        public string AvatarUrl { get; set; }

        [Display(Name = "Номер телефона")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Email подтвержден")]
        public bool EmailConfirmed { get; set; }

        [Display(Name = "Номер телефона подтвержден")]
        public bool PhoneNumberConfirmed { get; set; }

        [Display(Name = "Двухфакторная аутентификация")]
        public bool TwoFactorEnabled { get; set; }

        [Display(Name = "Активен")]
        public bool IsActive { get; set; }

        // Поля для управления блокировкой
        public DateTimeOffset? LockoutEnd { get; set; }
        public int AccessFailedCount { get; set; }
        public bool ClearLockout { get; set; }
        public bool ResetSecurityStamp { get; set; }
    }
}