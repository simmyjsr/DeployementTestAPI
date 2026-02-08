using InventoryAPI.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace InventoryAPI.Repositories
{
    public class UserRepo : IUserRepo
    {
        // For demo only; use DB in real app
        private static List<User> _users = new List<User>
    {
        new User { Id = 1, Username = "admin", Password = "admin123", Role = "Admin" }
    };

        
        public User GetUser(string username, string password)
        {
            return _users.FirstOrDefault(u => u.Username == username && u.Password == password);
        }

        //public async Task<int> CreateUserAsync(User user)
        //{
        //    using (IDbConnection db = new SqlConnection(_connectionString))
        //    {
        //        string sql = @"INSERT INTO Users (Username, PasswordHash, Role) 
        //                   VALUES (@Username, @PasswordHash, @Role);
        //                   SELECT CAST(SCOPE_IDENTITY() AS INT);";
        //        return await db.ExecuteScalarAsync<int>(sql, user);
        //    }
        //}

    }
}
