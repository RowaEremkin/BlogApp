using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RowaBlog.UserInterface.WindowBlogs
{
    public class WindowBlogsView : MonoBehaviour, IWindowBlogsView
    {
        [SerializeField] private Button _buttonBack;
        [SerializeField] private Button _buttonSend;
        [SerializeField] private GameObject _goInput;
        [SerializeField] private TextMeshProUGUI _textLabel;
        [SerializeField] private TMP_InputField _inputDescription;
        [SerializeField] private ScrollRect _scrollRect;
        [SerializeField] private Transform _transformContainer;
        [Header("Prefab")]
        [SerializeField] private BlogElementView _blogElementViewPrefab;
        [SerializeField] private BlogDateElementView _blogDateElementViewPrefab;
        private Vector2 _scroll;
        public BlogElementView BlogElementViewPrefab => _blogElementViewPrefab;
        public BlogDateElementView BlogDateElementViewPrefab => _blogDateElementViewPrefab;
        public Transform TransformContainer => _transformContainer;
        public event Action OnButtonBack;
        public event Action<string> OnButtonSend;
        public event Action OnScrollUpdate;

        public void ClearInput()
        {
            _inputDescription.text = "";
        }
        public void SetLabel(string label)
        {
            _textLabel.text = label;
        }
        public void SetInputAvailable(bool available)
        {
            _goInput.SetActive(available);
        }
        public void SetScrollDown()
        {
            StartCoroutine(DelayScrollDown());
        }

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
        private IEnumerator DelayScrollDown()
        {
            yield return new WaitForEndOfFrame();
            _scrollRect.verticalNormalizedPosition = 0f;
        }

    }
}
