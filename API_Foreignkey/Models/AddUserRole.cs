namespace API_Foreignkey.Models
{
    public class AddUserAssignRole
    {
        public int  UserId { get; set; }
        public List<int>RoleIds { get; set; }
    }
}