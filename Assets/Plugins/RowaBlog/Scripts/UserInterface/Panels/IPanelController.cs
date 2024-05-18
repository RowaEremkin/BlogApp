using Rowa.Blog.UserInterface.Panels;
using System;

namespace Rowa.Blog.UserInterface.WindowLogin
{
    public interface IPanelController
    {
        public event Action<EPanel> OnPanelShow;
        public void Show(EPanel panel);
    }
}