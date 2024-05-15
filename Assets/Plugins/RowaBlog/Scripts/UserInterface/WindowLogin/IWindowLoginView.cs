using System;

namespace Rowa.Blog.UserInterface.WindowLogin
{
    public interface IWindowLoginView
    {
        public string Login { get; }
        public string Password { get; }
        public bool PasswordShow { get; }

        public event Action<string> OnLoginChanged;
        public event Action<string> OnPasswordChanged;
        public event Action<bool> OnPasswordShowChanged;
        public event Action OnLoginClicked;
        public event Action OnRegisterClicked;

        public void SetLogin(string login);
        public void SetPassword(string password);
        public void SetPasswordShow(bool passwordShow);
    }
}