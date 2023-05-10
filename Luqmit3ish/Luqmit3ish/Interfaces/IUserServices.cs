using System;
using Luqmit3ish.Models;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace Luqmit3ish.Interfaces
{
	public interface IUserServices
	{
        Task<bool> Login(LoginRequest loginRequest);
        Task<User> GetUserByEmail(string email);
        Task<bool> DeleteAccount(int userId);
        Task<ObservableCollection<User>> GetUsers();
        Task<bool> InsertUser(SignUpRequest user);
        Task<User> GetUserById(int id);
        Task EditProfile(User user);
        Task<bool> ResetPassword(int id, string password);
        Task<bool>ForgetPassword(int id, string password);
        Task<bool> UploadPhoto(string photoPath, int userId);
        Task<bool> ForgotPassword(int id, string password);
    }
}

