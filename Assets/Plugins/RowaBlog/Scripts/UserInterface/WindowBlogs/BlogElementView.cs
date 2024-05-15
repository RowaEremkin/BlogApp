using Rowa.Blog.UserInterface.WindowRooms;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RowaBlog.UserInterface.WindowBlogs
{
    public class BlogElementView : MonoBehaviour, IBlogElementView
    {
        [SerializeField] private TextMeshProUGUI _textTitle;
        [SerializeField] private TextMeshProUGUI _textDescription;
        [SerializeField] private TextMeshProUGUI _textDateTime;
        [SerializeField] private TextMeshProUGUI _textLikes;
        [SerializeField] private Image _imageLiked;
        [SerializeField] private Image _imageBackground;
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private Button _buttonEnter;
        [SerializeField] private Button _buttonLike;
        [SerializeField] private float _minSizeX = 200f;
        public event Action OnButtonEnter;
        public event Action OnButtonLike;
        private void Awake()
        {
            if(_buttonEnter) _buttonEnter.onClick.AddListener(Enter);
            if (_buttonLike) _buttonLike.onClick.AddListener(Like);
        }
        public void SetData(BlogElementViewData blogElementViewData, bool updateRect = true)
        {
            gameObject.name = $"BlogElement ({blogElementViewData.BlogId})";
            if (_textDescription)
            {
                _textDescription.text = blogElementViewData.Description;
                //_textDescription.horizontalAlignment = blogElementViewData.MyBlog?HorizontalAlignmentOptions.Right:HorizontalAlignmentOptions.Left;
            }
            if (_textTitle)
            {
                _textTitle.text = blogElementViewData.Title;
                _textTitle.horizontalAlignment = blogElementViewData.MyBlog ? HorizontalAlignmentOptions.Right : HorizontalAlignmentOptions.Left;
                
            }
            if (_textDateTime) _textDateTime.text = blogElementViewData.DateTime.ToShortTimeString();
            if (_textLikes) _textLikes.text = blogElementViewData.Likes.ToString();
            if (_imageLiked) _imageLiked.gameObject.SetActive(blogElementViewData.Liked);
            if (updateRect) StartCoroutine(Delay(blogElementViewData.MyBlog));
        }
        private void Enter()
        {
            OnButtonEnter?.Invoke();
        }
        private void Like()
        {
            OnButtonLike?.Invoke();
        }
        private IEnumerator Delay(bool myBlog)
        {
            yield return new WaitForEndOfFrame();
            Vector2 minSize = new Vector2(_minSizeX, 120);
            if (_textDescription)
            {
                Vector2 addSize = new Vector2(
                    Mathf.Abs(_textDescription.rectTransform.offsetMin.x) + Mathf.Abs(_textDescription.rectTransform.offsetMax.x) + 20,
                    Mathf.Abs(_textDescription.rectTransform.sizeDelta.y) + 30
                    );
                Debug.Log($"GO: {gameObject.name} _textDescription.textBounds.size: {_textDescription.textBounds.size} addSize: {addSize}");
                if (_textDescription.textBounds.size.x + addSize.x > minSize.x)
                {
                    minSize.x = _textDescription.textBounds.size.x + addSize.x;
                }
                if (_textDescription.textBounds.size.y + addSize.y > minSize.y)
                {
                    minSize.y = _textDescription.textBounds.size.y + addSize.y;
                }
            }
            if (_textTitle)
            {
                Vector2 addSize = new Vector2(
                    Mathf.Abs(_textTitle.rectTransform.offsetMin.x) + Mathf.Abs(_textTitle.rectTransform.offsetMax.x) + 20,
                    Mathf.Abs(_textTitle.rectTransform.sizeDelta.y) + 30
                    );
                Debug.Log($"GO: {gameObject.name} _textTitle.textBounds.size: {_textTitle.textBounds.size}");
                if (_textTitle.textBounds.size.x + addSize.x > minSize.x)
                {
                    minSize.x = _textTitle.textBounds.size.x + addSize.x;
                }
                if (_textTitle.textBounds.size.y + addSize.y > minSize.y)
                {
                    minSize.y = _textTitle.textBounds.size.y + addSize.y;
                }
            }
            if (_imageBackground)
            {
                _imageBackground.rectTransform.ForceUpdateRectTransforms();

                Vector3[] corners = new Vector3[4];
                _imageBackground.rectTransform.GetLocalCorners(corners);
                Vector2 sizeDelta = new Vector2(corners[3].x - corners[0].x, corners[2].y - corners[0].y);

                Debug.Log($"GO: {gameObject.name} SizeDelta: {sizeDelta} minSize: {minSize}");
                float needX = Mathf.Max(sizeDelta.x - minSize.x, 0);
                if (myBlog)
                {
                    _imageBackground.rectTransform.offsetMin = new Vector2(needX, 30);
                    _imageBackground.rectTransform.offsetMax = new Vector2(0, 0);
                }
                else
                {
                    _imageBackground.rectTransform.offsetMin = new Vector2(0, 30);
                    _imageBackground.rectTransform.offsetMax = new Vector2(-needX, 0);
                }
                if (_rectTransform)
                {
                    _rectTransform.sizeDelta = new Vector2(_rectTransform.sizeDelta.x, minSize.y);
                }
            }
        }
    }
}