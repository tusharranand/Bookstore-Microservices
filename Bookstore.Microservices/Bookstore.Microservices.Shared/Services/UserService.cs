using Bookstore.Microservices.Models;
using Bookstore.Microservices.Shared.Interfaces;
using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Microservices.Shared.Services
{
    public class UserService : IUserService
    {
        private readonly IJWTService _jWTService;
        private readonly ISettingServices _settingServices;

        // Cosmos DocDB API database
        private string _docDbEndpointUri;
        private string _docDbPrimaryKey;
        private string _docDbDatabaseName;

        // Doc DB Collections
        private string _docDbDigitalMainCollectionName;

        private static CosmosClient _docDbSingletonClient;
        private readonly Container _cosmosContainer;

        public UserService(ISettingServices settingServices, IJWTService jWTService)
        {
            _settingServices = settingServices;
            _jWTService = jWTService;

            _docDbEndpointUri = _settingServices.GetDocDbEndpointUri();
            _docDbPrimaryKey = _settingServices.GetDocDbApiKey();
            _docDbDatabaseName = _settingServices.GetDocDbDatabaseName();
            _docDbDigitalMainCollectionName = _settingServices.GetDocDbMainCollectionName();
            _docDbSingletonClient = new CosmosClient(_settingServices.GetDocDbEndpointUri(), settingServices.GetDocDbApiKey());
            _cosmosContainer = _docDbSingletonClient.GetContainer(_docDbDatabaseName, _docDbDigitalMainCollectionName);
        }

        public async Task<UserModel> AddUserService(UserModel newUserData)
        {
            try
            {
                if (newUserData == null)
                    throw new ArgumentNullException(nameof(newUserData));

                newUserData.UserID = Guid.NewGuid().ToString();
                using (var response = _cosmosContainer
                    .CreateItemAsync<UserModel>(newUserData,
                    new PartitionKey(newUserData.UserID)))
                {
                    return response.Result.Resource;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<LoginResponse> LoginUserService(LoginCredentials credentials)
        {
            try
            {
                var user = _cosmosContainer.GetItemLinqQueryable<UserModel>(true)
                    .Where(x => x.Email == credentials.Email &&
                    x.Password == credentials.Password).AsEnumerable().FirstOrDefault();

                if (user == null)
                    throw new ArgumentNullException("Invalid Credentials");

                LoginResponse response = new LoginResponse();
                response.data = user;
                response.token = _jWTService.GenerateJWT(user.Email, user.UserID);

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<string>> ForgotPasswordService(string email)
        {
            try
            {
                var user = _cosmosContainer.GetItemLinqQueryable<UserModel>(true)
                    .Where(x => x.Email == email).AsEnumerable().FirstOrDefault();

                if (user == null)
                    throw new ArgumentNullException("Invalid Credentials");

                List<string> response = new();
                response.Add(_jWTService.GenerateJWT(user.Email, user.UserID));

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task ResetPasswordService(ResetPassword data, string email)
        {
            try
            {
                var user = _cosmosContainer.GetItemLinqQueryable<UserModel>(true)
                    .Where(x => x.Email == email)
                    .AsEnumerable().FirstOrDefault();
                if (user == null)
                    throw new ArgumentNullException(nameof(user));

                if (data.password != data.confirmPassword)
                    throw new ArgumentException("Entered passwords don't match");

                    user.Password = data.password;
                await _cosmosContainer.ReplaceItemAsync<UserModel>(user, user.UserID,
                    new PartitionKey(user.UserID));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
