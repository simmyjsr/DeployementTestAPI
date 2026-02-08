using InventoryAPI.Models;

namespace InventoryAPI.Repositories
{
    public interface IUserRepo
    {
        User GetUser(string username, string password);
    }
}
