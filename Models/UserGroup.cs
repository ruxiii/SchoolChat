using SchoolChat.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolChatOriginal.Models
{
    public class UserGroup
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? IdUser { get; set; }
        public int? IdGroup { get; set; }
        public bool? IsModerator { get; set; }
        public virtual SchoolGroup? Group { get; set; }
        public virtual ApplicationUser? User { get; set; }

    }
}
