using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolChatOriginal.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolChat.Models
{
    public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<UserGroup>? UserGroups { get; set; }
        public virtual ICollection<Message>? Messages { get; set; }
        public virtual ICollection<GroupRequest>? GroupRequests { get; set; }
        //[Required(ErrorMessage = "Prenume obligatoriu")]
        public string? FirstName { get; set; }

        //[Required(ErrorMessage = "Nume obligatoriu")]
        public string? LastName { get; set; }

        [NotMapped]
        public IEnumerable<SelectListItem> AllRoles { get; set; }
    }


}



