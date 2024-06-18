
using System.Collections.Generic;
using UnityEngine;

namespace Rowa.CurveDetection.Detection
{
    [System.Serializable]
    public class CurveDetectionResultData
    {
        public CurveSampleData SampleData;
        public float SameDistance;
        public CurveDetectionAdditionData AdditionData;
        public Dictionary<CurveSampleData, float> Same;
    }
    [System.Serializable]
    public class CurveDetectionAdditionData
    {
        public float Size;
        public float Length;
        public float Bendability;
        public float Threshold;
    }
}
