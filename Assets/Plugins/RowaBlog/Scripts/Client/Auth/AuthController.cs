
using Rowa.Blog.Client.Api;
using Rowa.Blog.Client.Storage;
using Rowa.Blog.Tools.Hasher;
using System;
using System.Net;
using UnityEngine;

namespace Rowa.Blog.Client.Auth
{
    public class AuthController : IAuthController
    {
        private string _login;
        private readonly IHasher _hasher;
        private readonly IClientApi _clientApi;
        private readonly IStorage _storage;
        public AuthController(
            IHasher hasher, 
            IClientApi clientApi,
            IStorage storage
            )
        {
            _hasher = hasher;
            _clientApi = clientApi;
            _storage = storage;
        }
        public string GetLogin()
        {
            return _login;
        }
        private void LoginComplete(HttpStatusCode statusCode, string token)
        {
            switch (statusCode)
            {
                case HttpStatusCode.OK:
                    Debug.Log($"PutLogin ok token: {token}");
                    _clientApi.SetToken(token);
                    break;
                default:
                    Debug.Log("PutLogin error: " + (int)statusCode + " - " + statusCode.ToString());
                    break;
            }
        }

        public void Login(string username, string password, Action<EAuthStatus> onComplete)
        {
            string hash = _hasher.ToHash(password);
            Debug.Log("User: " + username + " Password Hash: " + hash);
            Action<HttpStatusCode, string> callback = (statusCode, token) =>
            {
                EAuthStatus authStatus = EAuthStatus.NoConnection;
                switch (statusCode)
                {
                    case HttpStatusCode.OK:
                        authStatus = EAuthStatus.Success;
                        break;
                    case HttpStatusCode.NoContent:
                        authStatus = EAuthStatus.NoUser;
                        break;
                }
                LoginComplete(statusCode, token);
                onComplete?.Invoke(authStatus);
            };
            _storage.Save(EStorageEnum.Login.ToString(), username);
            _storage.Save(EStorageEnum.Password.ToString(), password);
            _clientApi.PutPlayerLogin(new Api.Data.PutPlayerLoginData(username, hash), callback);
            _login = username;
        }
        public void Register(string username, string password, Action<EAuthStatus> onComplete)
        {
            string hash = _hasher.ToHash(password);
            Debug.Log("User: " + username + " Password Hash: " + hash);
            Action<HttpStatusCode, string> callback = (statusCode, token) =>
            {
                EAuthStatus authStatus = EAuthStatus.NoConnection;
                switch (statusCode)
                {
                    case HttpStatusCode.OK:
                        authStatus = EAuthStatus.Success;
                        break;
                    case HttpStatusCode.Conflict:
                        authStatus = EAuthStatus.UserAlreadyExist;
                        break;
                }
                LoginComplete(statusCode, token);
                onComplete?.Invoke(authStatus);
            };
            _storage.Save(EStorageEnum.Login.ToString(), username);
            _storage.Save(EStorageEnum.Password.ToString(), password);
            _clientApi.PutPlayerRegister(new Api.Data.PutPlayerRegisterData(username, hash), callback);
            _login = username;
        }
        public void Logout()
        {
            _clientApi.DeletePlayerLogout(PlayerLogouted);
            string newDeviceId = _clientApi.GenerateNewDeviceId();
            _storage.Save(EStorageEnum.DeviceId.ToString(), newDeviceId);
        }
        private void PlayerLogouted(HttpStatusCode statusCode)
        {
            switch (statusCode)
            {
                case HttpStatusCode.OK:
                    Debug.Log("DeletePlayerLogout ok");
                    break;
                default:
                    Debug.Log("DeletePlayerLogout error: " + (int)statusCode + " - " + statusCode.ToString());
                    break;
            }
        }
    }
}