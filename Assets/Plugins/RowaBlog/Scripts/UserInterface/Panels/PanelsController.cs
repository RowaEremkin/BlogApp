using Rowa.Blog.UserInterface.Panels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rowa.Blog.UserInterface.WindowLogin
{
    public class PanelsController : IPanelController
    {
        private readonly Dictionary<EPanel, IPanelView> _dictinary;
        public event Action<EPanel> OnPanelShow;
        public PanelsController(Dictionary<EPanel, IPanelView> dictinary)
        {
            _dictinary = dictinary;
        }


        public void Show(EPanel panel)
        {
            foreach (KeyValuePair<EPanel, IPanelView> pair in _dictinary)
            {
                pair.Value.SetShow(pair.Key == panel);
            }
            OnPanelShow?.Invoke(panel);
        }
    }
}
