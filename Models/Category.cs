using System.ComponentModel.DataAnnotations;

namespace SchoolChat.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Category's name is mandatory!")]
        public string CategoryName { get; set; }

        public virtual ICollection<SchoolGroup>? Groups { get; set; }
    }
}



