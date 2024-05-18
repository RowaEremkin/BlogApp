using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RowaBlog.UserInterface.WindowBlogs
{
    public class WindowBlogsView : MonoBehaviour, IWindowBlogsView
    {
        #region Fields
        [SerializeField] private Button _buttonBack;
        [SerializeField] private Button _buttonSend;
        [SerializeField] private GameObject _goInput;
        [SerializeField] private TextMeshProUGUI _textLabel;
        [SerializeField] private TMP_InputField _inputDescription;
        [SerializeField] private ScrollRect _scrollRect;
        [SerializeField] private Transform _transformContainer;
        [SerializeField] private Image _imageEdit;
        [Header("Prefab")]
        [SerializeField] private BlogElementView _blogElementViewPrefab;
        [SerializeField] private BlogDateElementView _blogDateElementViewPrefab;
        private Vector2 _scroll;
        #endregion
        #region Property
        public BlogElementView BlogElementViewPrefab => _blogElementViewPrefab;
        public BlogDateElementView BlogDateElementViewPrefab => _blogDateElementViewPrefab;
        public Transform TransformContainer => _transformContainer;
        public Rect ContentRect => _scrollRect.content.rect;
        public float VerticalScroll => _scrollRect.verticalNormalizedPosition;
        public float VerticalViewPort => _scrollRect.viewport.rect.height;
        #endregion
        #region Events
        public event Action OnButtonBack;
        public event Action<string> OnButtonSend;
        public event Action OnScrollUpdate;
        #endregion
        #region Public
        public void ClearInput()
        {
            _inputDescription.text = "";
        }
        public void SetLabel(string label)
        {
            _textLabel.text = label;
        }
        public void SetInputText(string text)
        {
            _inputDescription.SetTextWithoutNotify(text);
        }
        public void SetInputAvailable(bool available)
        {
            _goInput.SetActive(available);
        }
        public void SetEditMode(bool active)
        {
            _imageEdit.gameObject.SetActive(active);
        }
        public void SetVerticalScroll(float scroll)
        {
            StartCoroutine(DelayVerticalScroll(scroll));
        }
        #endregion
        #region Private

        private void Awake()
        {
            _buttonBack.onClick.AddListener(Back);
            _buttonSend.onClick.AddListener(Send);
            _scrollRect.onValueChanged.AddListener(ScrollRectChanged);
        }
        private void ScrollRectChanged(Vector2 scroll)
        {
            _scroll = scroll;
            if(scroll.y > 1)
            {
                OnScrollUpdate?.Invoke();
            }
        }
        private void Back()
        {
            OnButtonBack?.Invoke();
        }
        private void Send()
        {
            OnButtonSend?.Invoke(_inputDescription.text);
        }
        private IEnumerator DelayVerticalScroll(float scroll)
        {
            yield return new WaitForEndOfFrame();
            _scrollRect.verticalNormalizedPosition = scroll;
        }
        #endregion

    }
}
