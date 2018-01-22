using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SeatedNow.Models
{
    public class UserAccount
    {

        [Key]
        public int UserID { get; set; }

        [Required(ErrorMessage = "Please enter your first name to continue!")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter your last name to continue!")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please enter your desired email to continue!")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter your desired mobile phone number")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Please enter your password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Your password must match!")]
        public string ConfirmPassword { get; set; }

    }
}
