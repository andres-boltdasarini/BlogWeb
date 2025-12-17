// ViewModels/CreateUserViewModel.cs
using System.ComponentModel.DataAnnotations;

namespace BlogWeb.ViewModels
{
    public class CreateUserViewModel
    {
        [Required(ErrorMessage = "Имя пользователя обязательно")]
        [Display(Name = "Имя пользователя")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email обязателен")]
        [EmailAddress(ErrorMessage = "Некорректный формат email")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Пароль обязателен")]
        [StringLength(100, ErrorMessage = "Пароль должен содержать минимум {2} и максимум {1} символов", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Подтверждение пароля обязательно")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтвердите пароль")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Отображаемое имя")]
        public string? DisplayName { get; set; }

        [Display(Name = "О себе")]
        public string? Bio { get; set; }

        [Display(Name = "URL аватара")]
        public string? AvatarUrl { get; set; }

        [Display(Name = "Активен")]
        public bool IsActive { get; set; } = true;
    }
}