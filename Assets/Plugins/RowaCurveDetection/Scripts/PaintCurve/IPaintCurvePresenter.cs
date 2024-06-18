using Rowa.CurveDetection.Detection;
using System;

namespace Rowa.CurveDetection.Paint
{
    public interface IPaintCurvePresenter
    {
        public event Action<CurveDetectionResultData> OnPaintMove;
        public event Action<CurveDetectionResultData> OnPaintEnd;
    }
}