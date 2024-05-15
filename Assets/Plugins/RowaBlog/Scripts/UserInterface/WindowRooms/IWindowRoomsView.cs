using System;
using UnityEngine;

namespace Rowa.Blog.UserInterface.WindowRooms
{
    public interface IWindowRoomsView
    {
        public RoomElementView RoomElementViewPrefab { get; }
        public Transform TransformContainer { get; }
        public event Action OnButtonLogout;
        public event Action OnButtonRefresh;
    }
}