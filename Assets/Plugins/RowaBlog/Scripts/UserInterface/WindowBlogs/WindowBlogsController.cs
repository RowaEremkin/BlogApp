using Rowa.Blog.Client.Api;
using Rowa.Blog.Client.Auth;
using Rowa.Blog.UserInterface.Panels;
using Rowa.Blog.UserInterface.WindowLogin;
using Rowa.Blog.UserInterface.WindowRooms;
using RowaBlog.Client.Api.Data;
using RowaBlog.UserInterface.WindowRooms;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using UnityEngine;

namespace RowaBlog.UserInterface.WindowBlogs
{
    public class WindowBlogsController : IWindowBlogsController, IDisposable
    {
        private bool _updated = false;
        private bool _endList = false;
        private int _roomId;
        private int _authorId;
        private Stack<ShowBlogData> _stack;
        private readonly BlogElementFactory _blogElementFactory;
        private readonly IWindowBlogsView _windowBlogsView;
        private readonly IPanelController _panelController;
        private readonly IAuthController _authController;
        private readonly IClientApi _clientApi;
        public WindowBlogsController(
            IWindowBlogsView windowBlogsView,
            IPanelController panelController,
            IAuthController authController,
            IClientApi clientApi
            )
        {
            _windowBlogsView = windowBlogsView;
            _panelController = panelController;
            _authController = authController;
            _clientApi = clientApi;

            _blogElementFactory = new BlogElementFactory(
                _windowBlogsView.BlogElementViewPrefab,
                _windowBlogsView.TransformContainer,
                _windowBlogsView.BlogDateElementViewPrefab);
            _stack = new Stack<ShowBlogData>();

            _windowBlogsView.OnButtonBack += ButtonBack;
            _windowBlogsView.OnButtonSend += ButtonSend;
            _windowBlogsView.OnScrollUpdate += ScrollUpdate;
            _panelController.OnPanelShow += PanelShow;
            _blogElementFactory.OnBlogClicked += BlogClicked;
            _blogElementFactory.OnBlogLikeClicked += BlogLikeClicked;
        }

        private void BlogLikeClicked(int blogId)
        {
            _clientApi.PutBlogLike(new PutBlogLikeData() { BlogId = blogId }, (HttpStatusCode statusCode)=> {
                BlogLikeComplete(statusCode, blogId);
            });
        }
        private void BlogLikeComplete(HttpStatusCode statusCode, int blogId)
        {
            switch (statusCode)
            {
                case HttpStatusCode.OK:
                    BlogElementViewData blogElementViewData = null;
                    foreach (var pair in _blogElementFactory.Dictinary)
                    {
                        if (pair.Value.BlogId == blogId)
                        {
                            blogElementViewData = pair.Value;
                            blogElementViewData.Likes = blogElementViewData.Liked ? blogElementViewData.Likes - 1 : blogElementViewData.Likes + 1;
                            blogElementViewData.Liked = !blogElementViewData.Liked;
                            pair.Key.SetData(blogElementViewData, updateRect: false);
                            break;
                        }
                    }
                    Debug.Log("PutBlogLike ok");
                    break;
                default:
                    Debug.Log("PutBlogLike error: " + (int)statusCode + " - " + statusCode.ToString());
                    break;
            }
        }

        public void Dispose()
        {
            _windowBlogsView.OnButtonBack -= ButtonBack;
            _windowBlogsView.OnButtonSend -= ButtonSend;
            _panelController.OnPanelShow -= PanelShow;
            _blogElementFactory.OnBlogClicked -= BlogClicked;
            _blogElementFactory.OnBlogLikeClicked -= BlogLikeClicked;
        }

        private void ScrollUpdate()
        {
            if (!_updated || _endList) return;
            if (_stack.TryPop(out ShowBlogData showBlogData2))
            {
                showBlogData2.Page += 1;
                ShowBlogs(showBlogData2);
            }
        }

        private void ButtonSend(string description)
        {
            _clientApi.PostBlog(new PostBlogCreateData
            {
                Title = _authController.GetLogin(),
                Description = description,
                RoomId = _roomId,
                AuthorId = _authorId,
            }, onComplete: BlogPosted);
        }

        private void BlogPosted(HttpStatusCode statusCode)
        {
            switch (statusCode)
            {
                case HttpStatusCode.OK:
                    Debug.Log("PostBlog ok");
                    UpdateList();
                    //ShowBlogs(new ShowBlogData() { RoomId= _roomId, AuthorId= _authorId, Page=0 }, updateMode: true);
                    _windowBlogsView.ClearInput();
                    break;
                default:

                    Debug.Log("PostBlog error: " + (int)statusCode + " - " + statusCode.ToString());
                    break;
            }
        }
        private void UpdateList()
        {
            if (_stack.TryPeek(out ShowBlogData showBlogData))
            {
                showBlogData.Page = 0;
                ShowBlogs(showBlogData, updateMode: true);
            }
        }

        private void BlogClicked(int blogId)
        {
            BlogElementViewData blogElementViewData = null;
            foreach (var pair in _blogElementFactory.Dictinary)
            {
                if (pair.Value.BlogId == blogId)
                {
                    blogElementViewData = pair.Value;
                    break;
                }
            }
            if(blogElementViewData != null)
            {
                if (blogElementViewData.AuthorId == _authorId) return;
                ShowBlogs(new ShowBlogData() { AuthorId = blogElementViewData.AuthorId, Title = blogElementViewData.Title });
            }
        }

        private void PanelShow(EPanel panel)
        {

        }

        private void ButtonBack()
        {
            if(_stack.TryPop(out ShowBlogData showBlogData))
            {
                if (_stack.TryPop(out ShowBlogData showBlogData2))
                {
                    ShowBlogs(showBlogData2);
                }
                else
                {
                    _panelController.Show(EPanel.Rooms);
                }
            }
            else
            {
                _panelController.Show(EPanel.Rooms);
            }
        }

        public void ShowBlogs(ShowBlogData showBlogData, bool updateMode = false)
        {
            Debug.Log($"ShowBlogs page: {showBlogData.Page}");
            SetLabel(showBlogData.Title);
            if (!updateMode) _stack.Push(showBlogData);
            _authorId = showBlogData.AuthorId;
            _roomId = showBlogData.RoomId;
            _updated = false;
            _clientApi.GetBlogs(page: showBlogData.Page, authorId: showBlogData.AuthorId, roomId: showBlogData.RoomId, onComplete: (HttpStatusCode, GetBlogsData) =>
            {
                OnBlogLoaded(HttpStatusCode, GetBlogsData, showBlogData);
            });
            _windowBlogsView.SetInputAvailable(showBlogData.AuthorId == -1);
        }
        public void SetLabel(string label)
        {
            _windowBlogsView.SetLabel(label);
        }
        private void OnBlogLoaded(HttpStatusCode statusCode, GetBlogsData getBlogsData, ShowBlogData showBlogData)
        {
            switch (statusCode)
            {
                case HttpStatusCode.OK:
                    string json = JsonUtility.ToJson(getBlogsData);
                    Debug.Log("GetBlogs ok json: " + json);
                    if (getBlogsData != null && getBlogsData.items.Count > 0)
                    {
                        List<BlogElementViewData> blogElementViews = new List<BlogElementViewData>();
                        for (int i = 0; i < getBlogsData.items.Count; i++)
                        {
                            BlogElementData blogElementData = getBlogsData.items[i];
                            BlogElementViewData blogElementViewData = new BlogElementViewData();
                            blogElementViewData.BlogId = blogElementData.id;
                            blogElementViewData.AuthorId = blogElementData.authorId;
                            blogElementViewData.Title = blogElementData.title;
                            blogElementViewData.Description = blogElementData.description;
                            blogElementViewData.Likes = blogElementData.likeCount;
                            blogElementViewData.Liked = blogElementData.liked;
                            blogElementViewData.MyBlog = blogElementData.myBlog;
                            if (DateTime.TryParseExact(blogElementData.dateTime, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateValue))
                            {
                                blogElementViewData.DateTime = dateValue;
                            }
                            blogElementViews.Add(blogElementViewData);
                        }
                        if(showBlogData.Page > 1)
                        {
                            if (blogElementViews.Count > 0)
                            {
                                _blogElementFactory.Add(blogElementViews);
                            }
                        }
                        else
                        {
                            _endList = false;
                            _blogElementFactory.Create(blogElementViews, descent: true);
                            _windowBlogsView.SetScrollDown();
                        }
                    }
                    else
                    {
                        if (showBlogData.Page > 1)
                        {

                        }
                        else
                        {
                            _blogElementFactory.Clear();
                        }
                    }
                    _updated = true;
                    break;
                case HttpStatusCode.NoContent:
                    _endList = true;
                    if (showBlogData.Page > 1)
                    {

                    }
                    else
                    {
                        _blogElementFactory.Clear();
                    }
                    _updated = true;
                    Debug.Log("GetBlogs error: " + (int)statusCode + " - " + statusCode.ToString());
                    break;
                default:
                    _blogElementFactory.Clear();
                    Debug.Log("GetBlogs error: " + (int)statusCode + " - " + statusCode.ToString());
                    break;
            }
        }
    }
}