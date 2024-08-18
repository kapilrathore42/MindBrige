using MindbridgeWebAPI.Models;

namespace MindbridgeWebAPI.Services
{
    public interface IUserService
    {
        Task<bool> RegisterUser(RegisterModel model);
        Task<bool> Login(LoginModel model);
        Task<List<UserModel>> GetUsers();
    }
}
