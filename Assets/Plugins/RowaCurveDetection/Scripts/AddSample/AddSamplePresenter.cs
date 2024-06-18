using Rowa.CurveDetection.Detection;
using Rowa.CurveDetection.Paint;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Rowa.CurveDetection.AddSample
{
    public class AddSamplePresenter : IAddSamplePresenter, IDisposable
    {
        #region Fields

        private readonly IAddSampleView _addSampleView;
        private readonly ICurveSampleStorage _curveSampleStorage;
        private readonly IPaintCurveView _paintCurveView;

        #endregion

        #region Init\Dispose
        public AddSamplePresenter(IAddSampleView addSampleView, ICurveSampleStorage curveSampleStorage, IPaintCurveView paintCurveView)
        {
            _addSampleView = addSampleView;
            _curveSampleStorage = curveSampleStorage;
            _paintCurveView = paintCurveView;
            _addSampleView.OnCurveAdd += CurveAdd;
        }

        public void Dispose()
        {
            _addSampleView.OnCurveAdd -= CurveAdd;
        }

        #endregion

        #region Event Handlers

        private void CurveAdd(string name)
        {
            if (string.IsNullOrEmpty(name)) return;
            _curveSampleStorage.AddSample(new CurveSampleData()
            {
                Points = _paintCurveView.Points,
                Key = name
            });
            _addSampleView.ResetInput();
        }

        #endregion
    }
}
