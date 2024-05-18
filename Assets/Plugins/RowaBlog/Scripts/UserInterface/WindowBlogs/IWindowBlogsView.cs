using System;
using UnityEngine;
using UnityEngine.UI;

namespace RowaBlog.UserInterface.WindowBlogs
{
    public interface IWindowBlogsView
    {
        public BlogElementView BlogElementViewPrefab { get; }
        public BlogDateElementView BlogDateElementViewPrefab { get; }
        public Transform TransformContainer { get; }
        public Rect ContentRect { get; }
        public float VerticalScroll { get; }
        public float VerticalViewPort { get; }
        public event Action OnButtonBack;
        public event Action<string> OnButtonSend;
        public event Action OnScrollUpdate;
        public void SetLabel(string label);
        public void SetInputText(string text);
        public void SetInputAvailable(bool available);
        public void SetEditMode(bool active);
        public void ClearInput();
        public void SetVerticalScroll(float scroll);
    }
}