namespace MindbridgeWebAPI.Repository
{
    using Models;
    public interface IUserRepository
    {
        Task<List<UserModel>> GetUsers();
        Task<bool> RegisterUser(RegisterModel model);
        Task<bool> Login(LoginModel model);
       
    }
}
