using SchoolChat.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolChatOriginal.Models
{
    public class UserRole
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public bool IsModerator { get; set; }
        public int? IdUser { get; set; }
        public int? IdRole { get; set; }
        public virtual Role? Role { get; set; }
        public virtual ApplicationUser? User { get; set; }

    }
}
