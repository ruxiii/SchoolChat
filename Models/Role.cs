using SchoolChatOriginal.Models;
using System.ComponentModel.DataAnnotations;

namespace SchoolChat.Models
{
    public class Role
    {
        [Key]
        public int IdRole { get; set; }

        [Required(ErrorMessage = "Role is mandatory!")]
        public string RoleType { get; set; }

        public virtual ICollection<UserRole>? UserRoles { get; set; }
    }
}



