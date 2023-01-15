using SchoolChatOriginal.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolChat.Models
{
    public class Message
    {
        [Key]
        public int IdMessage { get; set; }

        [Required(ErrorMessage = "You can't send an empty message!")] 
        public string TextMessage { get; set; }
        public DateTime MessageTime { get; set; }

        [Required(ErrorMessage = "Group is mandatory!")]
        public int IdGroup { get; set; }

        [NotMapped]
        public virtual SchoolGroup? Group { get; set; }

        [Required(ErrorMessage = "User is mandatory!")]
        public string IdUser { get; set; }

        [NotMapped]
        public virtual ApplicationUser? User { get; set; }

    }
}



