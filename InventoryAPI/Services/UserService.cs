using InventoryAPI.Helpers;
using InventoryAPI.Models;
using InventoryAPI.Repositories;

namespace InventoryAPI.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepo _userRepository;
        private readonly JwtHelper _jwtHelper;

        public UserService(IUserRepo userRepository, JwtHelper jwtHelper)
        {
            _userRepository = userRepository;
            _jwtHelper = jwtHelper;
        }

        public LoginResponse Authenticate(LoginRequest request)
        {
            var user = _userRepository.GetUser(request.Username, request.Password);
            if (user == null) return null;
            return _jwtHelper.GenerateToken(user);
        }
    }
}
