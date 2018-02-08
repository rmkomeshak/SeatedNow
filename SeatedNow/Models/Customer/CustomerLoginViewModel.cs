using System.ComponentModel.DataAnnotations;

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
