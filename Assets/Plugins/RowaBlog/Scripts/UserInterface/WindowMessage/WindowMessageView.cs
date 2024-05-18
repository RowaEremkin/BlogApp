using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Rowa.Blog.UserInterface.WindowMessage
{
    public class WindowMessageView : MonoBehaviour, IWindowMessageView
    {
        [SerializeField] private TextMeshProUGUI _textLabel;
        [SerializeField] private TextMeshProUGUI _textDescription;
        [SerializeField] private Button _buttonOk;

        public event Action OnOkClicked;
        private void Awake()
        {
            _buttonOk.onClick.AddListener(ButtonOkClicked);
        }
        private void ButtonOkClicked()
        {
            OnOkClicked?.Invoke();
        }

        public void SetDiscription(string text)
        {
            _textDescription.text = text;
        }

        public void SetLabel(string text)
        {
            _textLabel.text = text;
        }

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

