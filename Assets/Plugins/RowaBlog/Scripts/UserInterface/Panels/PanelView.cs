using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Rowa.Blog.UserInterface.Panels
{
    public class PanelView : MonoBehaviour, IPanelView
    {
        public void SwitchShow()
        {
            SetShow(!gameObject.activeSelf);
        }

        public void SetShow(bool show)
        {
            gameObject.SetActive(show);
        }
    }
}
