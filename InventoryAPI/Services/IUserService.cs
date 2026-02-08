using InventoryAPI.Models;

namespace InventoryAPI.Services
{
    public interface IUserService
    {
        LoginResponse Authenticate(LoginRequest request);
    }
}
