using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SeatedNow.Models
{
    public class CustomerRegisterViewModel
    {
        [Required(ErrorMessage = "Please enter your first and last name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter your desired email")]
        [DataType(DataType.EmailAddress)]
        [Remote("EmailIsRegistered", "Account", ErrorMessage = "That email already exists in our system")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter your desired mobile phone number")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Must be a valid US phone numner (xxx xxx xxxx)")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Please enter your password")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Password must be between 5 and 20 characters")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Your passwords must match")]
        public string ConfirmPassword { get; set; }
    }
}
