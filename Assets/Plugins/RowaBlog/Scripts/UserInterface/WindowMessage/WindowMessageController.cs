using System;

namespace Rowa.Blog.UserInterface.WindowMessage
{
    public class WindowMessageController : IWindowMessageController
    {
        public readonly IWindowMessageView _windowMessageView;
        private Action _storedAction;
        public WindowMessageController(
            IWindowMessageView windowMessageView
            )
        {
			_windowMessageView = windowMessageView;

            _windowMessageView.OnOkClicked += () =>
            {
                _windowMessageView.SetShow(false);
                _storedAction?.Invoke();
            };

        }

        public void ShowMessage(string label, string description, Action onOk = null)
        {
            _windowMessageView.SetLabel(label);
            _windowMessageView.SetDiscription(description);
            _windowMessageView.SetShow(true);
            _storedAction = onOk;

        }
    }
}