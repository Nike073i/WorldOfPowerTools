using System.Security.Cryptography;
using System.Text;
using WorldOfPowerTools.Domain.Enums;
using WorldOfPowerTools.Domain.Exceptions;
using WorldOfPowerTools.Domain.Models.Entities;
using WorldOfPowerTools.Domain.Repositories;

namespace WorldOfPowerTools.Domain.Services
{
    public class IdentityService
    {
        private readonly Actions _defaultUserActions = Actions.Cart | Actions.MyOrders;
        private readonly string UserExistErrorMessage = "ѕользователь с таким логином уже существует";

        private readonly IUserRepository _userRepository;

        public IdentityService(IUserRepository userRepository)
        {
            if (userRepository == null) throw new ArgumentNullException(nameof(userRepository));
            _userRepository = userRepository;
        }

        public async Task<User?> Authorization(string login, string password)
        {
            if (string.IsNullOrEmpty(login)) throw new ArgumentNullException(nameof(login));
            if (string.IsNullOrEmpty(password)) throw new ArgumentNullException(nameof(password));
            var userByLogin = await _userRepository.GetByLoginAsync(login);
            if (userByLogin == null) return null;
            var user = PasswordHash(password).Equals(userByLogin.PasswordHash, StringComparison.OrdinalIgnoreCase)
                ? userByLogin : null;
            return user;
        }
        public async Task<User> Registration(string login, string password)
        {
            if (string.IsNullOrEmpty(login)) throw new ArgumentNullException(nameof(login));
            if (string.IsNullOrEmpty(password)) throw new ArgumentNullException(nameof(password));
            var userByLogin = await _userRepository.GetByLoginAsync(login);
            if (userByLogin != null) throw new UserExistsException(UserExistErrorMessage);
            var newUser = new User(login, password, _defaultUserActions);
            return await _userRepository.SaveAsync(newUser);
        }

        public string PasswordHash(string password)
        {
            if (string.IsNullOrEmpty(password)) throw new ArgumentNullException(nameof(password));
            var md5 = MD5.Create();
            var bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToHexString(bytes);
        }
    }
}
