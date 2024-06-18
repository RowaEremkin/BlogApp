using Rowa.CurveDetection.Detection;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Rowa.CurveDetection.Paint
{
    public class PaintCurvePresenter : IPaintCurvePresenter, IDisposable
    {
        #region Fields

        private readonly PaintCurvePresenterData _paintCurvePresenterData;

        #endregion

        #region Events

        public event Action<CurveDetectionResultData> OnPaintMove;
        public event Action<CurveDetectionResultData> OnPaintEnd;

        #endregion

        #region Init\Dispose

        public PaintCurvePresenter(PaintCurvePresenterData paintCurvePresenterData)
        {
            _paintCurvePresenterData = paintCurvePresenterData;

            _paintCurvePresenterData.PaintCurveView.OnPaintStart += PaintStart;
            _paintCurvePresenterData.PaintCurveView.OnPaintAddPoint += PaintAddPoint;
            _paintCurvePresenterData.PaintCurveView.OnPaintEnd += PaintEnd;
        }

        public void Dispose()
        {
            _paintCurvePresenterData.PaintCurveView.OnPaintStart -= PaintStart;
            _paintCurvePresenterData.PaintCurveView.OnPaintAddPoint -= PaintAddPoint;
            _paintCurvePresenterData.PaintCurveView.OnPaintEnd -= PaintEnd;
        }

        #endregion

        #region Event Handlers

        private void PaintEnd(PaintCurveData paintCurveData)
        {
            Debug.Log($"Seconds: {paintCurveData.CompleteSeconds}");
            Debug.Log($"Length: {paintCurveData.CompleteLength}");
            List<Vector2> list2D = new List<Vector2>();
            foreach (Vector2 item in paintCurveData.Points)
            {
                list2D.Add(FromScreenToWorldPoint(item));
            }
            CalculateResult(list2D, end: true);
            if (_paintCurvePresenterData.ParticleSystem.isPlaying) _paintCurvePresenterData.ParticleSystem.Stop();
        }

        private void PaintAddPoint(UnityEngine.Vector2 vector)
        {
            Vector3[] vectors = new Vector3[_paintCurvePresenterData.LineRenderer.positionCount];
            _paintCurvePresenterData.LineRenderer.GetPositions(vectors);
            List<Vector3> list = new List<Vector3>(vectors);
            Vector3 vector3 = FromScreenToWorldPoint(vector);
            list.Add(vector3);
            _paintCurvePresenterData.LineRenderer.positionCount += 1;
            _paintCurvePresenterData.LineRenderer.SetPositions(list.ToArray());

            List<Vector2> list2D = new List<Vector2>();
            foreach (Vector3 item in list)
            {
                list2D.Add(item);
            }
            CalculateResult(list2D);

            _paintCurvePresenterData.ParticleSystem.transform.position = vector3;
        }

        private void PaintStart(UnityEngine.Vector2 startVector)
        {
            _paintCurvePresenterData.LineRenderer.positionCount = 1;
            Vector3 vector3 = FromScreenToWorldPoint(startVector);
            _paintCurvePresenterData.LineRenderer.SetPositions(new UnityEngine.Vector3[1] { vector3 });
            _paintCurvePresenterData.ParticleSystem.transform.position = vector3;
            _paintCurvePresenterData.ParticleSystem.Play();


        }

        #endregion
        private void CalculateResult(List<Vector2> positions, bool end = false)
        {
            CurveDetectionResultData curveDetectionResultData = _paintCurvePresenterData.CurveDetectionController.GetSameSample(new CurveData()
            {
                Points = positions
            });
            if(_paintCurvePresenterData.PaintCurveInfoView != null)
            {
                _paintCurvePresenterData.PaintCurveInfoView.SetInfo(new Data.PaintCurveInfoViewData()
                {
                    ResultData = curveDetectionResultData,
                    End = end
                });
            }
            if (end)
            {
                OnPaintEnd?.Invoke(curveDetectionResultData);
            }
            else
            {
                OnPaintMove?.Invoke(curveDetectionResultData);
            }
        }
        private Vector3 FromScreenToWorldPoint(Vector2 screenPoint)
        {
            Vector3 newVector = _paintCurvePresenterData.Camera.ScreenToWorldPoint(screenPoint);
            newVector += _paintCurvePresenterData.Camera.transform.forward;
            return newVector;
        }
    }
}
