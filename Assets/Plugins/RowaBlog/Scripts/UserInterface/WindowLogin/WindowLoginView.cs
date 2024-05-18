using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Rowa.Blog.UserInterface.WindowLogin
{
    public class WindowLoginView : MonoBehaviour, IWindowLoginView
    {
        #region Fields
        [SerializeField] private TMP_InputField _inputLogin;
        [SerializeField] private TMP_InputField _inputPassword;
        [SerializeField] private Toggle _togglePassword;
        [SerializeField] private Button _buttonLogin;
        [SerializeField] private Button _buttonRegister;
        #endregion
        #region Properties
        public string Login => _inputLogin.text;
        public string Password => _inputPassword.text;
        public bool PasswordShow => _togglePassword.isOn;
        #endregion
        #region Events
        public event Action<string> OnLoginChanged;
        public event Action<string> OnPasswordChanged;
        public event Action<bool> OnPasswordShowChanged;
        public event Action OnLoginClicked;
        public event Action OnRegisterClicked;
        #endregion
        #region Unity
        public void Awake()
        {
            _inputLogin.onEndEdit.AddListener((string text) =>
            {
                OnLoginChanged?.Invoke(text);
            });
            _inputPassword.onEndEdit.AddListener((string text) =>
            {
                OnPasswordChanged?.Invoke(text);
            });
            _togglePassword.onValueChanged.AddListener((bool active) =>
            {
                SetPasswordShowInternal(active);
                OnPasswordShowChanged?.Invoke(active);
            });
            SetPasswordShowInternal(false);
            _buttonLogin.onClick.AddListener(() =>
            {
                OnLoginClicked?.Invoke();
            });
            _buttonRegister.onClick.AddListener(() =>
            {
                OnRegisterClicked?.Invoke();
            });
        }
        #endregion
        #region Private Methods
        private void SetPasswordShowInternal(bool passwordShow)
        {
            _inputPassword.contentType = passwordShow ? TMP_InputField.ContentType.Standard : TMP_InputField.ContentType.Password;
            _inputPassword.ForceLabelUpdate();
        }
        #endregion
        #region Public Methods
        public void SetLogin(string login)
        {
            _inputLogin.SetTextWithoutNotify(login);
        }
        public void SetPassword(string password)
        {
            _inputPassword.SetTextWithoutNotify(password);
        }
        public void SetPasswordShow(bool passwordShow)
        {
            SetPasswordShowInternal(passwordShow);
            _togglePassword.SetIsOnWithoutNotify(passwordShow);
        }
        #endregion
    }
}