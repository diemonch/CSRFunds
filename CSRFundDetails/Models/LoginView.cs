using System;
using System.ComponentModel.DataAnnotations;

namespace CSRFundDetails.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "User ID is required")]
        public string? UserId { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }
}


