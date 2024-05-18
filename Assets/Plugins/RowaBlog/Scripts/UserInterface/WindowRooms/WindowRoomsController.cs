using Rowa.Blog.Client.Api;
using Rowa.Blog.Client.Auth;
using Rowa.Blog.UserInterface.WindowLogin;
using RowaBlog.Client.Api.Data;
using RowaBlog.UserInterface.WindowBlogs;
using RowaBlog.UserInterface.WindowRooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Rowa.Blog.UserInterface.WindowRooms
{
    public class WindowRoomsController : IWindowRoomsController, IDisposable
    {
        private readonly RoomElementFactory _roomElementFactory;
        private readonly IWindowBlogsController _windowBlogsController;
        private readonly IWindowRoomsView _windowRoomsView;
        private readonly IPanelController _panelController;
        private readonly IAuthController _authController;
        private readonly IClientApi _clientApi;
        public WindowRoomsController(
            IWindowRoomsView windowRoomsView,
            IPanelController panelController,
            IAuthController authController,
            IClientApi clientApi,
            IWindowBlogsController windowBlogsController
            ) 
        {
            _windowRoomsView = windowRoomsView;
            _panelController = panelController;
            _authController = authController;
            _clientApi = clientApi;
            _windowBlogsController = windowBlogsController;

            _roomElementFactory = new RoomElementFactory(
                _windowRoomsView.RoomElementViewPrefab,
                _windowRoomsView.TransformContainer);

            _windowRoomsView.OnButtonLogout += ButtonLogout;
            _windowRoomsView.OnButtonRefresh += ButtonRefresh;
            _panelController.OnPanelShow += PanelShow;
            _roomElementFactory.OnRoomClicked += RoomClicked;
        }

        private void ButtonRefresh()
        {
            Refresh();
        }

        private void RoomClicked(int roomId)
        {
            Debug.Log($"RoomClicked roomId: {roomId}");
            _panelController.Show(Panels.EPanel.Blogs);
            RoomElementViewData roomElementData = null;
            foreach (var pair in _roomElementFactory.Dictinary)
            {
                if(pair.Value.RoomId == roomId)
                {
                    roomElementData = pair.Value;
                    break;
                }
            }
            if (roomElementData == null) return;
            _windowBlogsController.ShowBlogs(new ShowBlogData() { RoomId = roomId, Title = roomElementData.RoomName, Page = 0});
        }

        private void PanelShow(Panels.EPanel panel)
        {
            if(panel == Panels.EPanel.Rooms)
            {
                Refresh();
            }
        }
        private void Refresh()
        {
            _clientApi.GetRooms(0, GetRoomsComplete);
        }

        private void GetRoomsComplete(HttpStatusCode statusCode, GetRoomsData getRoomsData)
        {
            switch (statusCode)
            {
                case HttpStatusCode.OK:
                    List<RoomElementViewData> list = new List<RoomElementViewData>();
                    if(getRoomsData!= null && getRoomsData.items != null)
                    {
                        for (int i = 0; i < getRoomsData.items.Count; i++)
                        {
                            RoomElementViewData roomElementViewData = new RoomElementViewData();
                            roomElementViewData.RoomName = getRoomsData.items[i].roomName;
                            roomElementViewData.RoomId = getRoomsData.items[i].roomId;
                            list.Add(roomElementViewData);
                        }
                        _roomElementFactory.Create(list);
                    }
                    else
                    {
                        _roomElementFactory.Clear();
                    }
                    break;
                default:
                    Debug.Log("GetRooms error: " + (int)statusCode + " - " + statusCode.ToString());
                    break;
            }
        }

        public void Dispose()
        {
            _windowRoomsView.OnButtonLogout -= ButtonLogout;
            _windowRoomsView.OnButtonRefresh -= ButtonRefresh;
            _panelController.OnPanelShow -= PanelShow;
            _roomElementFactory.OnRoomClicked -= RoomClicked;
        }

        private void ButtonLogout()
        {
            _authController.Logout();
            _panelController.Show(Panels.EPanel.Login);
        }
    }
}
