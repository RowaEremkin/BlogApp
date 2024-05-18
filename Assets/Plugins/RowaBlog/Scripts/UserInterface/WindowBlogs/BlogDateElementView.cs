using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace RowaBlog.UserInterface.WindowBlogs
{
    public class BlogDateElementView : MonoBehaviour, IBlogDateElementView
    {
        [SerializeField] private TextMeshProUGUI _textDate;
        public void SetData(DateTime date)
        {
            if(_textDate) _textDate.SetText(date.ToShortDateString());
        }
    }
}
