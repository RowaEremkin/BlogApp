using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rowa.Blog.UserInterface
{
    public interface IShowHide
    {
        void SwitchShow();
        void SetShow(bool show);
    }
}
