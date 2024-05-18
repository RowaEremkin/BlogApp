using Rowa.Blog.Client.Api;
using Rowa.Blog.Client.Auth;
using Rowa.Blog.UserInterface.Panels;
using Rowa.Blog.UserInterface.WindowLogin;
using Rowa.Blog.UserInterface.WindowRooms;
using RowaBlog.Client.Api.Data;
using RowaBlog.Tools.Coroutine;
using RowaBlog.UserInterface.WindowRooms;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using UnityEngine;
using UnityEngine.UI;

namespace RowaBlog.UserInterface.WindowBlogs
{
    public class WindowBlogsController : IWindowBlogsController, IDisposable
    {
        #region Fields
        private bool _updated = false;
        private bool _endList = false;
        private int _roomId;
        private int _authorId;
        private int _editBlogId = -1;
        public float _oldScrollY = 0;
        public float _oldSizeY = 0;
        private Stack<ShowBlogData> _stack;
        private readonly BlogElementFactory _blogElementFactory;
        private readonly IWindowBlogsView _windowBlogsView;
        private readonly IPanelController _panelController;
        private readonly IAuthController _authController;
        private readonly IClientApi _clientApi;
        private readonly ICoroutineService _coroutineService;
        #endregion
        #region Init\Dispose
        public WindowBlogsController(
            IWindowBlogsView windowBlogsView,
            IPanelController panelController,
            IAuthController authController,
            IClientApi clientApi,
            ICoroutineService coroutineService
            )
        {
            _windowBlogsView = windowBlogsView;
            _panelController = panelController;
            _authController = authController;
            _clientApi = clientApi;
            _coroutineService = coroutineService;

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
            _blogElementFactory.OnListChanged += ListChanged;
            _blogElementFactory.OnBlogDeleteClicked += BlogDeleteClicked;
            _blogElementFactory.OnBlogEditClicked += BlogEditClicked;
        }

        public void Dispose()
        {
            _windowBlogsView.OnButtonBack -= ButtonBack;
            _windowBlogsView.OnButtonSend -= ButtonSend;
            _panelController.OnPanelShow -= PanelShow;
            _blogElementFactory.OnBlogClicked -= BlogClicked;
            _blogElementFactory.OnBlogLikeClicked -= BlogLikeClicked;
            _blogElementFactory.OnListChanged -= ListChanged;
            _blogElementFactory.OnBlogDeleteClicked -= BlogDeleteClicked;
            _blogElementFactory.OnBlogEditClicked -= BlogEditClicked;
        }
        #endregion
        #region Public
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
        #endregion
        #region OnCompleted
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
        private void BlogDeleteComplete(HttpStatusCode statusCode)
        {
            switch (statusCode)
            {
                case HttpStatusCode.OK:
                    Debug.Log("DeleteBlog ok");
                    UpdateList();
                    break;
                default:
                    Debug.Log("DeleteBlog error: " + (int)statusCode + " - " + statusCode.ToString());
                    break;
            }
        }
        private void BlogEditComplete(HttpStatusCode statusCode)
        {
            switch (statusCode)
            {
                case HttpStatusCode.OK:
                    Debug.Log("PutBlogEdit ok");
                    SetEditMode(false);
                    UpdateList();
                    break;
                default:
                    Debug.Log("PutBlogEdit error: " + (int)statusCode + " - " + statusCode.ToString());
                    break;
            }
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
                        if (showBlogData.Page > 1)
                        {
                            if (blogElementViews.Count > 0)
                            {
                                _blogElementFactory.Add(blogElementViews);
                            }
                        }
                        else
                        {
                            _endList = false;
                            _oldScrollY = 0;
                            _oldSizeY = 0;
                            _blogElementFactory.Create(blogElementViews, descent: true);
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
        #endregion
        #region Event Handlers
        private void BlogLikeClicked(int blogId)
        {
            _clientApi.PutBlogLike(new PutBlogLikeData() { BlogId = blogId }, (HttpStatusCode statusCode)=> {
                BlogLikeComplete(statusCode, blogId);
            });
        }
        private void ButtonSend(string description)
        {
            if(_editBlogId < 0)
            {
                _clientApi.PostBlog(new PostBlogCreateData
                {
                    Title = _authController.GetLogin(),
                    Description = description,
                    RoomId = _roomId,
                    AuthorId = _authorId,
                }, onComplete: BlogPosted);
            }
            else
            {
                _clientApi.PutBlogEdit(new PutBlogEditData() { BlogId = _editBlogId, BlogDescription = description}, onComplete: BlogEditComplete);
            }
        }
        private void BlogClicked(int blogId)
        {
            BlogElementViewData blogElementViewData = GetElementDataByBlogId(blogId);
            if (blogElementViewData != null)
            {
                if (blogElementViewData.AuthorId == _authorId) return;
                ShowBlogs(new ShowBlogData() { AuthorId = blogElementViewData.AuthorId, Title = blogElementViewData.Title });
            }
        }
        private void BlogEditClicked(int blogId)
        {
            if(_editBlogId < 0)
            {
                BlogElementViewData blogElementViewData = GetElementDataByBlogId(blogId);
                if (blogElementViewData != null)
                {
                    _windowBlogsView.SetInputText(blogElementViewData.Description);
                }
                BlogElementView blogElementView = GetElementViewByBlogId(blogId);
                if (blogElementView != null)
                {
                    blogElementView.SetEditMode(true);
                }
                _editBlogId = blogId;
                SetEditMode(true);
            }
            else
            {
                SetEditMode(false);
            }
        }
        private void BlogDeleteClicked(int blogId)
        {
            _clientApi.DeleteBlog(new DeleteBlogData() { BlogId =  blogId }, onComplete: BlogDeleteComplete);
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
        private void PanelShow(EPanel panel)
        {

        }
        private void ButtonBack()
        {
            if (_editBlogId >= 0)
            {
                SetEditMode(false);
                return;
            }
            if (_stack.TryPop(out ShowBlogData showBlogData))
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
        private void SetEditMode(bool active)
        {
            if (!active)
            {
                if(_editBlogId >= 0)
                {
                    BlogElementView blogElementView = GetElementViewByBlogId(_editBlogId);
                    if (blogElementView != null)
                    {
                        blogElementView.SetEditMode(false);
                    }
                }
                _windowBlogsView.ClearInput();
                _editBlogId = -1;
            }
            _windowBlogsView.SetEditMode(active);
        }
        private void ListChanged()
        {
            _coroutineService.StartOnceCoroutine(DelayListChanged());
        }
        private BlogElementViewData GetElementDataByBlogId(int blogId)
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
            return blogElementViewData;
        }
        private BlogElementView GetElementViewByBlogId(int blogId)
        {
            BlogElementView blogElementView = null;
            foreach (var pair in _blogElementFactory.Dictinary)
            {
                if (pair.Value.BlogId == blogId)
                {
                    blogElementView = pair.Key;
                    break;
                }
            }
            return blogElementView;
        }
        #endregion
        #region Private
        private void UpdateList()
        {
            if (_stack.TryPeek(out ShowBlogData showBlogData))
            {
                showBlogData.Page = 0;
                ShowBlogs(showBlogData, updateMode: true);
            }
        }
        #endregion
        private static bool DebugDelayListChanged = true;
        private IEnumerator DelayListChanged()
        {
            yield return new WaitForEndOfFrame();
            float viewPort = _windowBlogsView.VerticalViewPort;
            float oldPos = (_oldSizeY - viewPort) * (_oldScrollY);
            float newSizeY = _windowBlogsView.ContentRect.height;
            float scroll = 0f;
            if (newSizeY - viewPort > 0 && _oldSizeY - viewPort > 0)
            {
                scroll = oldPos / (newSizeY-viewPort);
                //scroll = _oldScrollY + (newSizeY - _oldSizeY) / Mathf.Max(newSizeY - viewPort, 0.0001f) * (viewPort * _oldScrollY);
            }
            if (DebugDelayListChanged) Debug.Log($"oldPos: {oldPos} oldSize: {_oldSizeY} newSize: {newSizeY} oldScroll: {_oldScrollY} newScroll: {scroll}");
            _windowBlogsView.SetVerticalScroll(scroll);

            _oldScrollY = _windowBlogsView.VerticalScroll;
            _oldSizeY = _windowBlogsView.ContentRect.height;
        }
    }
}