using Rowa.Blog.Tools.Factory;
using Rowa.Blog.UserInterface.WindowRooms;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RowaBlog.UserInterface.WindowBlogs
{
    public class BlogElementFactory : Factory<BlogElementView, BlogElementViewData>
    {
        private List<BlogDateElementView> _dateList = new List<BlogDateElementView>();
        private BlogDateElementView _datePrefab;
        public event Action<int> OnBlogClicked;
        public event Action<int> OnBlogLikeClicked;
        public BlogElementFactory(BlogElementView prefab, Transform container, BlogDateElementView blogDatePrefab) : base(prefab, container)
        {
            _datePrefab = blogDatePrefab;
        }
        public override List<BlogElementView> Add(List<BlogElementViewData> data, bool descent = false)
        {
            List<BlogElementView> list = base.Add(data);
            for (int i = 0; i < list.Count; i++)
            //for (int i = list.Count - 1; i >= 0 ; i--)
            {
                if (!Dictinary.ContainsKey(list[i])) continue;
                for (int j = 0; j < data.Count; j++)
                {
                    if (Dictinary[list[i]].BlogId != data[j].BlogId) continue;
                    list[i].transform.SetSiblingIndex(0);
                    break;
                }
            }
            UpdateDates();
            return list;
        }
        public override List<BlogElementView> Create(List<BlogElementViewData> data, bool descent = false)
        {
            List<BlogElementView> list = base.Create(data, descent);
            UpdateDates();
            return list;
        }
        public override void SetData(BlogElementView view, BlogElementViewData data)
        {
            if (view == null) return;
            view.SetData(data);
            if (data == null) return;
            view.OnButtonEnter += () =>
            {
                OnBlogClicked?.Invoke(data.BlogId);
            };
            view.OnButtonLike += () =>
            {
                OnBlogLikeClicked?.Invoke(data.BlogId);
            };
        }
        private void UpdateDates()
        {
            ClearDates();
            DateTime currentDate = DateTime.Today;
            for (int i = 0; i < _container.childCount; i++)
            {
                Transform child = _container.GetChild(i);
                BlogElementView blogElementView = null;
                for (int j = 0; j < _list.Count; j++)
                {
                    if (_list[j].transform == child)
                    {
                        blogElementView = _list[j];
                        break;
                    }
                }
                if (blogElementView)
                {
                    BlogElementViewData blogElementViewData = GetData(blogElementView);
                    if (blogElementViewData != null)
                    {
                        if (blogElementViewData.DateTime.Date != currentDate)
                        {
                            currentDate = blogElementViewData.DateTime.Date;
                            BlogDateElementView blogDateElementView = AddDate(currentDate, i);
                            if(blogDateElementView != null)
                            {

                            }
                        }
                    }
                }
            }
        }
        private void ClearDates()
        {
            if (_dateList == null) return;
            for (int i = _dateList.Count - 1; i >= 0; i--)
            {
                if (_dateList[i] != null)
                {
                    GameObject.Destroy(_dateList[i].gameObject);
                }
                _dateList.RemoveAt(i);
            }
        }
        private BlogDateElementView AddDate(DateTime dateTime, int index = -1)
        {
            BlogDateElementView blogDateElementView = GameObject.Instantiate(_datePrefab, _container);
            if (blogDateElementView)
            {
                blogDateElementView.SetData(dateTime);
                if(index >= 0)
                {
                    blogDateElementView.transform.SetSiblingIndex(index);
                }
                _dateList.Add(blogDateElementView);
            }
            return blogDateElementView;
        }
    }
}