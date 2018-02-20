using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SeatedNow.Models
{
    public class CustomerApiLoginAuth
    {

        public int UserID { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Role { get; set; }


    }
}
