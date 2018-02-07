using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SeatedNow.Models
{
    public class CustomerLoginViewModel
    {
        [Required(ErrorMessage = "Must not leave this field empty")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Must not leave this field empty")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
