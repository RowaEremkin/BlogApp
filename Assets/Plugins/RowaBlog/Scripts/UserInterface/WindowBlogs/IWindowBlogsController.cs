using UnityEngine;

namespace RowaBlog.UserInterface.WindowBlogs
{
    public interface IWindowBlogsController
    {
        public void ShowBlogs(ShowBlogData showBlogData, bool updateMode = false);
        public void SetLabel(string label);
    }
}