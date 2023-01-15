using SchoolChatOriginal.Models;
using System.ComponentModel.DataAnnotations;

namespace SchoolChat.Models
{
    public class User
    {
        [Key]
        public int IdUser { get; set; }

        [Required(ErrorMessage = "Firstname is mandatory!")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Lastname is mandatory!")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Phone number is mandatory!")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Username is mandatory!")]
        [EmailAddress(ErrorMessage = "Your username has to be an email!")]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "Password is mandatory!")]
        [MinLength(5, ErrorMessage = "Your password is too short!")]
        [MaxLength(20, ErrorMessage = "Your password is too long!")]
        public string Password { get; set; }

        public virtual ICollection<UserGroup>? UserGroups { get; set; }
        public virtual ICollection<UserRole>? UserRoles { get; set; }
        public virtual ICollection<Message>? Messages { get; set; }
    }
}




