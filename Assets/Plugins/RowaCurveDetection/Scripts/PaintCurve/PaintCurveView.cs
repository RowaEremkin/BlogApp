using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Rowa.CurveDetection.Paint
{
    public class PaintCurveView : MonoBehaviour, IPaintCurveView, IPointerDownHandler, IPointerUpHandler, IPointerMoveHandler
    {
        #region Fields

        [Header("Settings")]
        [SerializeField] private PaintCurveSetting paintCurveSetting;

        [Header("Debug")]
        [SerializeField] private float _currentLength = 0f;
        [SerializeField] private double _startTime;
        [SerializeField] private bool _paint = false;
        [SerializeField] private List<Vector2> _points = new List<Vector2>();

        #endregion

        #region Property

        public List<Vector2> Points => _points;

        #endregion

        #region Events

        public event Action<Vector2> OnPaintStart;
        public event Action<Vector2> OnPaintAddPoint;
        public event Action<PaintCurveData> OnPaintEnd;

        #endregion

        #region Interfaces

        public void OnPointerDown(PointerEventData eventData)
        {
            _paint = true;
            _currentLength = 0;
            _points = new List<Vector2>();
            _startTime = Time.realtimeSinceStartupAsDouble;

            _points.Add(eventData.position);

            OnPaintStart?.Invoke(eventData.position);
        }

        public void OnPointerMove(PointerEventData eventData)
        {
            if (!_paint) return;
            float distance = Vector2.Distance(_points[_points.Count-1], eventData.position);
            if (distance >= (paintCurveSetting ? paintCurveSetting.NextPointDistance : 1f)) ;
            {
                _points.Add(eventData.position);
                _currentLength += distance;
                OnPaintAddPoint?.Invoke(eventData.position);
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _paint = false;
            PaintCurveData paintCurveData = new PaintCurveData();
            paintCurveData.Points = _points;
            paintCurveData.CompleteLength = _currentLength;
            paintCurveData.CompleteSeconds = Time.realtimeSinceStartupAsDouble - _startTime;

            OnPaintEnd?.Invoke(paintCurveData);
        }

        #endregion
    }
}