using System;
using UnityEngine;

namespace RowaBlog.UserInterface.WindowBlogs
{
    public interface IWindowBlogsView
    {
        public BlogElementView BlogElementViewPrefab { get; }
        public BlogDateElementView BlogDateElementViewPrefab { get; }
        public Transform TransformContainer { get; }
        public event Action OnButtonBack;
        public event Action<string> OnButtonSend;
        public event Action OnScrollUpdate;
        public void SetLabel(string label);
        public void SetInputAvailable(bool available);
        public void ClearInput();
        public void SetScrollDown();
    }
}