﻿using System.ComponentModel.DataAnnotations;

namespace web_253501_zhalkovsky.Models
{
    public class RegisterUserViewModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [Compare(nameof(Password), ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }

        public IFormFile? Avatar { get; set; }
    }
}
