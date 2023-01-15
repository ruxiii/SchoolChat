using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolChatOriginal.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolChat.Models
{
    public class SchoolGroup
    {
        [Key]
        public int IdGroup { get; set; }

        [Required(ErrorMessage = "Group name is mandatory!")]
        [MaxLength(50, ErrorMessage = "Your group name is too long!")]
        public string GroupName { get; set; }

        [Required(ErrorMessage = "Group description is mandatory!")]
        [MaxLength(256, ErrorMessage = "Your description is too long!")]
        public string GroupDescription { get; set; }

        [Required(ErrorMessage = "Category is mandatory!")]
        public int? IdCategory { get; set; }

        public virtual Category? Category { get; set; }

        public virtual ICollection<UserGroup>? UserGroups { get; set; }
        public virtual ICollection<GroupRequest>? GroupRequests { get; set; }

        [NotMapped]
        public IEnumerable<SelectListItem>? Categ { get; set; }
    }
}


