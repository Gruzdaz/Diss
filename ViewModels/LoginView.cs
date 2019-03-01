using System;
using System.ComponentModel.DataAnnotations;

namespace Diss.ViewModels
{
    public class LoginView
    {
        [Required(ErrorMessage ="This field is required.")]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string ErrorMessage { get; set; }

    }
}