using Azure.Identity;

namespace MindbridgeWebAPI.Models
{
    public class LoginModel
    {
        public string username { get; set; }
        public string password { get; set; }
    }
}
