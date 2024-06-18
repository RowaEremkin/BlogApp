using System.Collections.Generic;
using UnityEngine;

namespace Rowa.CurveDetection.Paint
{
    [System.Serializable]
    public class PaintCurveData
    {
        public List<Vector2> Points;
        public double CompleteSeconds;
        public float CompleteLength;
    }
}