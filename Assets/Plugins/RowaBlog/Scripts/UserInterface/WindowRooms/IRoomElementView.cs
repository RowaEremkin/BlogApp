using System;

namespace Rowa.Blog.UserInterface.WindowRooms
{
    internal interface IRoomElementView
    {
        public event Action OnButtonEnter;
        public void SetData(RoomElementViewData roomElementViewData);
    }
}