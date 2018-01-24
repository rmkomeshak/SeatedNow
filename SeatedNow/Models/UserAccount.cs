using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SeatedNow.Models
{
    public class UserAccount
    {
        public UserAccount(string name, string email, string phoneNumber, string password)
        {
            Name = name;
            Email = email;
            PhoneNumber = phoneNumber;
            Password = password;
        }

        [Key]
        public int UserID { get; set; }

        [Required(ErrorMessage = "Please enter your name to continue!")]
        public string Name { get; set; }

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

        public string getFirstName()
        {
            var names = Name.Split(' ');
            string firstname = names[0];
            return firstname;
        }

        public string getLastName()
        {
            var names = Name.Split(' ');
            string lastname = names[1];
            return lastname;
        }

    }
}
