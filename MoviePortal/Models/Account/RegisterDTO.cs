using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MoviePortal.Models.Account
{
    public class RegisterDTO
    {
        [Required]
        [Display(Name = "Логин")]
        public string UserName { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Почта")]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Пароль")]
        public string Password { get; set; }
        [Display(Name = "Подтверждение пароля")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string ConfirmPassword { get; set; }
    }
}
