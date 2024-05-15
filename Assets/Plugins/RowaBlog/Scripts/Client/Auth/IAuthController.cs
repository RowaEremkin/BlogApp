using System;

namespace Rowa.Blog.Client.Auth
{
    public interface IAuthController
    {
        void Login(string username, string password, Action<EAuthStatus> onComplete);
        void Register(string username, string password, Action<EAuthStatus> onComplete);
        public void Logout();
        public string GetLogin();
    }
}