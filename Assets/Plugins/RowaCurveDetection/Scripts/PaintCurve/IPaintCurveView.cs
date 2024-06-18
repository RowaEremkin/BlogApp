using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rowa.CurveDetection.Paint
{
    public interface IPaintCurveView
    {
        public List<Vector2> Points { get; }
        public event Action<Vector2> OnPaintStart;
        public event Action<Vector2> OnPaintAddPoint;
        public event Action<PaintCurveData> OnPaintEnd;
    }
}