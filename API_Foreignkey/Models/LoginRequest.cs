namespace API_Foreignkey.Models
{
    public class LoginRequest
    {
        public string UserName { get; set; }    
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
