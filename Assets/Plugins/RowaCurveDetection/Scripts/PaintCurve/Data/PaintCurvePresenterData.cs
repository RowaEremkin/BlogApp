using Rowa.CurveDetection.Detection;
using TMPro;
using UnityEngine;

namespace Rowa.CurveDetection.Paint
{
    public class PaintCurvePresenterData
    {
        public IPaintCurveView PaintCurveView;
        public ICurveDetectionController CurveDetectionController;
        public IPaintCurveInfoView PaintCurveInfoView;
        public LineRenderer LineRenderer;
        public Camera Camera;
        public ParticleSystem ParticleSystem;
    }
}