namespace RowaBlog.Client.Api.Data
{
    [System.Serializable]
    public class PostBlogCreateData
    {
        public string Title;
        public string Description;
        public int AuthorId = -1;
        public int RoomId = -1;
    }
}