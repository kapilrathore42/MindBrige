using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using MindbridgeWebAPI.Models;
using Newtonsoft.Json;
using System.Linq; // Assuming this has your UserModel and RegisterModel definitions

namespace MindbridgeWebAPI.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;

        public UserRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public  async Task<List<UserModel>> GetUsers()
        {
            List<UserModel> result = new List<UserModel>(); 
         

            using (var connection = new SqlConnection(_connectionString))
            {
               await connection.OpenAsync();
                 var users =  await connection.QueryAsync<UserModel>("SELECT * FROM Users");
                 foreach(var user in users)
                {
                    result.Add(user);
                }
                
               
            }
            return result;

        }

        public async Task<bool> Login(LoginModel model)
        {
            using(var connection = new SqlConnection(_connectionString))
            {await connection.OpenAsync();
                string query = "select Top 1 1 from Users where name = @username and password = @password";
                var parameter = new {model.username, model.password};
                var success = await connection.ExecuteScalarAsync<bool>(query, parameter);
                return success;
            }
        }

        public async Task<bool> RegisterUser(RegisterModel user)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                // Implement password hashing logic here
                var hashedPassword = HashPassword(user.Password);

                var sql = "INSERT INTO Users (Name, Email, Password) VALUES (@Name, @Email, @HashedPassword); SELECT SCOPE_IDENTITY()";
                var parameters = new { user.Name, user.Email, HashedPassword = hashedPassword };

                var ok = await connection.ExecuteScalarAsync<bool>(sql, parameters);
                return ok;
            }
        }

        private string HashPassword(string password)
        {
            // Implement your password hashing logic here
            // Example: return BCrypt.Net.BCrypt.HashPassword(password);
            return password; // Replace with actual hashing logic
        }
    }
}
