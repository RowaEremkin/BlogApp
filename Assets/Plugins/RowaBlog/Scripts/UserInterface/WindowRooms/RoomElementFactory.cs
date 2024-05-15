using Rowa.Blog.Tools.Factory;
using Rowa.Blog.UserInterface.WindowRooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace RowaBlog.UserInterface.WindowRooms
{
    public class RoomElementFactory : Factory<RoomElementView, RoomElementViewData>
    {
        public event Action<int> OnRoomClicked;
        public RoomElementFactory(RoomElementView prefab, Transform container) : base(prefab, container)
        {

        }
        public override void SetData(RoomElementView view, RoomElementViewData data)
        {
            if (view == null) return;
            view.SetData(data);
            if (data == null) return;
            view.OnButtonEnter += () =>
            {
                OnRoomClicked?.Invoke(data.RoomId);
            };
        }
    }
}
