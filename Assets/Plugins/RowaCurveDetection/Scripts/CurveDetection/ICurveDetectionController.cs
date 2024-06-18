using System.Collections.Generic;
using UnityEngine;

namespace Rowa.CurveDetection.Detection
{
    public interface ICurveDetectionController
    {
        public CurveDetectionResultData GetSameSample(CurveData curveData);
    }
}