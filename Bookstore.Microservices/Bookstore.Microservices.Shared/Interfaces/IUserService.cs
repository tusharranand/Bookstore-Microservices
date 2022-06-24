using Bookstore.Microservices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Microservices.Shared.Interfaces
{
    public interface IUserService
    {
        Task<UserModel> AddUserService(UserModel newUserData);
        Task<LoginResponse> LoginUserService(LoginCredentials credentials);
        Task<List<string>> ForgotPasswordService(string email);
        Task ResetPasswordService(ResetPassword data, string email);
    }
}
