using Rowa.Blog.Client.Api.Data;
using RowaBlog.Client.Api.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.Networking;

namespace Rowa.Blog.Client.Api
{
    public partial class ClientApi : MonoBehaviour, IClientApi
    {
        #region Fields
        [SerializeField] private string _deviceId = "0";
        [SerializeField] private string _token = "0";
        #endregion
        #region Public Methods
        public string GenerateNewDeviceId()
        {
            _deviceId = Guid.NewGuid().ToString();
            return _deviceId;
        }
        public void SetDeviceId(string deviceId)
        {
            _deviceId = deviceId;
        }
        public void SetToken(string token)
        {
            _token = token;
        }
        public virtual void PingApi(Action<bool> onComplete)
        {
            StartEnumerator(PingApiInternal(onComplete));
        }
        #endregion
        #region Blog
        public virtual void GetBlogs(int page = 0, int limit = 5, int authorId = -1, int roomId = -1, Action<HttpStatusCode, GetBlogsData> onComplete = null)
        {
            Action<HttpStatusCode, string> callback = (code, json) =>
            {
                if(string.IsNullOrEmpty(json)) onComplete?.Invoke(code, new GetBlogsData());
                GetBlogsData getBlogsData = JsonUtility.FromJson<GetBlogsData>(json);
                onComplete?.Invoke(code, getBlogsData);
            };
            StartEnumerator(Request.Get(
                url: GetFullUrl(GetBlogPath, _deviceId, param: new Dictionary<string, string>()
                {
                    { "page", page.ToString() },
                    { "limit", limit.ToString() },
                    { "authorId", authorId.ToString() },
                    { "roomId", roomId.ToString() },
                }),
                token: _token,
                onComplete: callback));
        }
        public virtual void PostBlog(PostBlogCreateData postBlogCreateData, Action<HttpStatusCode> onComplete = null)
        {
            string json = JsonUtility.ToJson(postBlogCreateData);
            StartEnumerator(Request.Post(
                url: GetFullUrl(PostBlogCreatePath, _deviceId),
                json: json,
                token: _token,
                onComplete: onComplete));
        }
        public virtual void PutBlogLike(PutBlogLikeData putBlogLikeData, Action<HttpStatusCode> onComplete = null)
        {
            string json = JsonUtility.ToJson(putBlogLikeData);
            StartEnumerator(Request.Put(
                url: GetFullUrl(PutBlogLikePath, _deviceId),
                json: json,
                token: _token,
                onComplete: onComplete));
        }
        #endregion
        #region Rooms
        public virtual void GetRooms(int page = 0, Action<HttpStatusCode, GetRoomsData> onComplete = null)
        {
            Action<HttpStatusCode, string> callback = (code, json) =>
            {
                GetRoomsData getRoomsData = JsonUtility.FromJson<GetRoomsData>(json);
                onComplete?.Invoke(code, getRoomsData);
            };
            StartEnumerator(Request.Get(
                url:GetFullUrl(GetRoomPath, page: page.ToString()), 
                token: _token,
                onComplete: callback));
        }
        #endregion
        #region Player
        public virtual void PutPlayerLogin(PutPlayerLoginData data,Action<HttpStatusCode, string> onComplete = null)
        {
            string json = JsonUtility.ToJson(data);
            StartEnumerator(Request.Put(
                url: GetFullUrl(PutPlayerLoginPath, _deviceId), 
                json: json, 
                token: _token, 
                onComplete: onComplete));
        }
        public virtual void PutPlayerRegister(PutPlayerRegisterData data, Action<HttpStatusCode, string> onComplete = null)
        {
            string json = JsonUtility.ToJson(data);
            StartEnumerator(Request.Put(
                url: GetFullUrl(PutPlayerRegisterPath, _deviceId),
                json: json, 
                token: _token, 
                onComplete: onComplete));
        }
        public virtual void DeletePlayerLogout(Action<HttpStatusCode> onComplete = null)
        {
            StartEnumerator(Request.Delete(
                url: GetFullUrl(DeletePlayerLogoutPath, _deviceId), 
                token: _token, 
                onComplete: onComplete));
        }
        #endregion
        #region Private Methods

        private IEnumerator PingApiInternal(Action<bool> onComplete)
        {
            string pingTarget = Protocol + Domain;

            UnityWebRequest request = new UnityWebRequest(pingTarget);
            yield return request.SendWebRequest();;

            var isHasError = request.result == UnityWebRequest.Result.ConnectionError;
            onComplete?.Invoke(!isHasError);

            request.Dispose();
        }
        private void StartEnumerator(IEnumerator enumerator)
        {
            if (enumerator == null) return;
            StartCoroutine(enumerator);
        }
        #endregion
    }
}