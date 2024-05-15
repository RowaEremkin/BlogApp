
using Rowa.Blog.Client.Auth;
using Rowa.Blog.Client.Storage;
using Rowa.Blog.UserInterface.WindowMessage;
using System;
using UnityEngine;

namespace Rowa.Blog.UserInterface.WindowLogin
{
	public class WindowLoginController : IWindowLoginController, IDisposable
	{
		private readonly IWindowLoginView _windowLoginView;
		private readonly IWindowMessageController _windowMessageController;
        private readonly IAuthController _authController;
        private readonly IStorage _storage;
        private readonly IPanelController _panelController;
        public WindowLoginController(
			IWindowLoginView windowLoginView, 
			IWindowMessageController windowMessageController,
			IAuthController authController,
			IStorage storage,
            IPanelController panelController

            )
		{
			_windowLoginView = windowLoginView;
			_windowMessageController = windowMessageController;
			_authController = authController;
			_storage = storage;
            _panelController = panelController;

            _windowLoginView.OnLoginClicked += Login;
            _windowLoginView.OnRegisterClicked += Register;

            _storage.Load(EStorageEnum.Login.ToString(), (complete, value) =>
			{
				_windowLoginView.SetLogin(value);
            });
            _storage.Load(EStorageEnum.Password.ToString(), (complete, value) =>
            {
                _windowLoginView.SetPassword(value);
            });

        }

        public void Dispose()
		{
			_windowLoginView.OnLoginClicked -= Login;
            _windowLoginView.OnRegisterClicked -= Register;
        }

        private void Login()
		{
			_authController.Login(_windowLoginView.Login, _windowLoginView.Password, OnLoginComplete);
        }
        private void Register()
        {
            _authController.Register(_windowLoginView.Login, _windowLoginView.Password, OnLoginComplete);
        }
        private void OnLoginComplete(EAuthStatus authStatus)
        {
			switch (authStatus)
            {
                case EAuthStatus.Success:
                    _windowMessageController.ShowMessage("Успех", "Вы вошли");
                    _panelController.Show(Panels.EPanel.Rooms);
                    break;
                case EAuthStatus.UserAlreadyExist:
                    _windowMessageController.ShowMessage("Ошибка", "Логин занят");
                    break;
                case EAuthStatus.NoConnection:
                    _windowMessageController.ShowMessage("Ошибка", "Нет подключения");
                    break;
                default:
                    _windowMessageController.ShowMessage("Ошибка", "Неверный логин или пароль");
                    break;
			}
        }
	}
}