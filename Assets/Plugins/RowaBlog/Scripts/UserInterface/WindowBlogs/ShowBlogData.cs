using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RowaBlog.UserInterface.WindowBlogs
{
    [System.Serializable]
    public class ShowBlogData
    {
        public int Page = 1;
        public int AuthorId = -1;
        public int RoomId = -1;
        public string Title = "";
    }
}
