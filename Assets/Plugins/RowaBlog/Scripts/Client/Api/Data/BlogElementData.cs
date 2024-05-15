using System;

namespace RowaBlog.Client.Api.Data
{
    [System.Serializable]
    public class BlogElementData
    {
        public int id;
        public string title;
        public string description;
        public string dateTime;
        public int authorId;
        public int roomId;
        public int likeCount;
        public bool liked;
        public bool myBlog;
    }
}