using System;

namespace Rowa.Blog.UserInterface.WindowMessage
{
	public interface IWindowMessageController
	{
		public void ShowMessage(string label, string description, Action onOk = null);
	}
}