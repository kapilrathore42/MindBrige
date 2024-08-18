using MindbridgeWebAPI.Models;
using MindbridgeWebAPI.Repository;

namespace MindbridgeWebAPI.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
       

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
          
        }

        public Task<List<UserModel>> GetUsers()
        {

            var users = _userRepository.GetUsers(); 
            return users;
        }

        public Task<bool> Login(LoginModel model)
        {
       
            var result  = _userRepository.Login(model);
            return result;
        }

        public async Task<bool> RegisterUser(RegisterModel model)
        {
            bool success = false;
            if (model != null)
            {
                try
                {
                    await _userRepository.RegisterUser(model);
                    success = true;
                }
                catch (Exception ex)
                {
                    
                }
            }
            return success;
        }
    }
}
