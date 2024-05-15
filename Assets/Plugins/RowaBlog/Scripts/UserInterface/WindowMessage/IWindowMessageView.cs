using System;

namespace Rowa.Blog.UserInterface.WindowMessage
{
    public interface IWindowMessageView : IShowHide
    {
        public void SetLabel(string text);
        public void SetDiscription(string text);
        public event Action OnOkClicked;
    }
}