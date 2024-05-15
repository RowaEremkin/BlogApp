

using System;
using UnityEngine;
using UnityEngine.UI;

namespace Rowa.Blog.UserInterface.WindowRooms
{
    public class WindowRoomsView : MonoBehaviour, IWindowRoomsView
    {
        [SerializeField] private Button _buttonLogout;
        [SerializeField] private Button _buttonRefresh;
        [SerializeField] private RoomElementView _roomElementViewPrefab;
        [SerializeField] private Transform _transformContainer;
        public RoomElementView RoomElementViewPrefab => _roomElementViewPrefab;
        public Transform TransformContainer => _transformContainer;
        public event Action OnButtonLogout;
        public event Action OnButtonRefresh;
        private void Awake()
        {
            _buttonLogout.onClick.AddListener(Logout);
            _buttonRefresh.onClick.AddListener(Refresh);
        }
        private void Logout()
        {
            OnButtonLogout?.Invoke();
        }
        private void Refresh()
        {
            OnButtonRefresh?.Invoke();
        }
    }
}
