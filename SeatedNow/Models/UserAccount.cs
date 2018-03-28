using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SeatedNow.Models
{
    public class UserAccount
    {
        public UserAccount(int id, string name, string email, string phoneNumber, string password, string role)
        {
            UserID = id;
            Name = name;
            Email = email;
            PhoneNumber = phoneNumber;
            Password = password;
            Role = role;
        }

        public UserAccount(string name, string email, string phoneNumber, string password, string role)
        {
            Name = name;
            Email = email;
            PhoneNumber = phoneNumber;
            Password = password;
            Role = role;
        }

        [Key]
        public int UserID { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public string Name { get; set; }

        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "This field is required")]
        public string Email { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "This field is required")]
        public string PhoneNumber { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string Role { get; set; }

        public string getFirstName()
        {
            var names = Name.Split(' ');
            string firstname = names[0];
            return firstname;
        }

        public string getLastName()
        {
            var names = Name.Split(' ');
            string lastname = "";

            if (names.Length >= 3)
            {
                StringBuilder sb = new StringBuilder();

                for (int i = 1; i < names.Length; i++)
                {
                    if (i == names.Length - 1)
                    {
                        sb.Append(names[i]);
                    } else
                    {
                        sb.Append(names[i] + " ");
                    }
                }

                lastname = sb.ToString();
            } else if (names.Length == 2)
            {
                return names[1];
            } else
            {
                return "";
            }

            return lastname;
        }

    }
}
