using Rowa.Blog.UserInterface.WindowRooms;
using System;

namespace RowaBlog.UserInterface.WindowBlogs
{
    public interface IBlogElementView
    {
        public event Action OnButtonEnter;
        public event Action OnButtonLike;
        public event Action OnButtonDelete;
        public event Action OnButtonEdit;
        public void SetData(BlogElementViewData blogElementViewData, bool updateRect = true);
        public void SetEditMode(bool active);
    }
}