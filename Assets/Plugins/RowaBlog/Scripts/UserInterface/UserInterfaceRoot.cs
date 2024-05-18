using Rowa.Blog.Client.Api;
using Rowa.Blog.Client.Auth;
using Rowa.Blog.Client.Device;
using Rowa.Blog.Client.Storage;
using Rowa.Blog.Tools.Hasher;
using Rowa.Blog.UserInterface.Panels;
using Rowa.Blog.UserInterface.WindowLogin;
using Rowa.Blog.UserInterface.WindowMessage;
using Rowa.Blog.UserInterface.WindowRooms;
using RowaBlog.Tools.Coroutine;
using RowaBlog.UserInterface.WindowBlogs;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rowa.Blog.UserInterface
{
	public class UserInterfaceRoot : MonoBehaviour
    {
        [SerializeField] private CoroutineService _coroutineService;
        [SerializeField] private ClientApi _clientApi;
        [SerializeField] private WindowLoginView _windowLoginView;
        [SerializeField] private WindowRoomsView _windowRoomsView;
        [SerializeField] private WindowBlogsView _windowBlogsView;
        [SerializeField] private WindowMessageView _windowMessageView;
        [SerializeField] private List<PanelEnumToView> _panels;

        private IWindowMessageController _windowMessageController;
        private IWindowLoginController _windowLoginController;
        private IAuthController _authController;
        private IHasher _hasher;
        private IStorage _storage;
        private IDeviceIdLoader _deviceIdLoader;
        private IPanelController _panelController;
        private IWindowRoomsController _windowRoomsController;
        private IWindowBlogsController _windowBlogsController;
        private void Awake()
        {
            Dictionary<EPanel, IPanelView> panels = new Dictionary<EPanel, IPanelView>();
            foreach (PanelEnumToView panel in _panels)
            {
                panels.Add(panel.EPanel, panel.PanelView);
            }
            _panelController = new PanelsController(panels);
            _hasher = new Hasher();
            _storage = new StorageJson();
            _authController = new AuthController(_hasher, _clientApi, _storage);
            _windowMessageController = new WindowMessageController(_windowMessageView);
            _windowLoginController = new WindowLoginController(_windowLoginView, _windowMessageController, _authController, _storage, _panelController);
            _windowBlogsController = new WindowBlogsController(_windowBlogsView, _panelController, _authController, _clientApi, _coroutineService);
            _windowRoomsController = new WindowRoomsController(_windowRoomsView, _panelController, _authController, _clientApi, _windowBlogsController);
            _deviceIdLoader = new DeviceIdLoader(_clientApi, _storage);
            _panelController.Show(EPanel.Login);
        }
    }
}