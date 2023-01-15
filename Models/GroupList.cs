namespace SchoolChatOriginal.Models
{
    public class GroupList
    {
        public object Id;
        public object Name;
        public object Description;
        public object Category;

        public GroupList(object Id, object Name, object Description, object Category)
        {
            this.Id = Id;
            this.Name = Name;
            this.Description = Description;
            this.Category = Category;
            
        }
    }
}
