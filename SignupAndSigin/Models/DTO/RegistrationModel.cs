﻿using System.ComponentModel.DataAnnotations;

namespace SignupAndSigin.Models.DTO
{
    public class RegistrationModel
    {
        [Required]
        public string Name { get; set; }    
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

    }
}
