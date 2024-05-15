using Rowa.Blog.Client.Api.Data;
using System.Net;
using System;
using System.Collections;
using RowaBlog.Client.Api.Data;
using System.Collections.Generic;
using UnityEngine;

namespace Rowa.Blog.Client.Api
{
    public interface IClientApi
    {
        public string GenerateNewDeviceId();
        public void SetDeviceId(string deviceId);
        public void SetToken(string token);
        public void GetBlogs(int page = 0, int limit = 5, int authorId = -1, int roomId = -1, Action<HttpStatusCode, GetBlogsData> onComplete = null);
        public void PostBlog(PostBlogCreateData postBlogCreateData, Action<HttpStatusCode> onComplete = null);
        public void GetRooms(int page = 0, Action<HttpStatusCode, GetRoomsData> onComplete = null);
        public void PutPlayerLogin(PutPlayerLoginData data, Action<HttpStatusCode, string> onComplete = null);
        public void PutPlayerRegister(PutPlayerRegisterData data, Action<HttpStatusCode, string> onComplete = null);
        public void PutBlogLike(PutBlogLikeData putBlogLikeData, Action<HttpStatusCode> onComplete = null);
        public void DeletePlayerLogout(Action<HttpStatusCode> onComplete = null);
        public void PingApi(Action<bool> onComplete);
    }
}