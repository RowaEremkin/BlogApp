
using System.Collections.Generic;
using UnityEngine;

namespace Rowa.CurveDetection.Detection.Methods
{
    public class CurveDetecionMethodVectors : ICurveDetectionMethod
    {
        public float Calculate(List<Vector2> curve1, List<Vector2> curve2)
        {
            if(curve1 == null || curve2 == null) return float.MaxValue;
            int maxLength = Mathf.Max(curve1.Count, curve2.Count);
            List<Vector2> curveInterpoated1 = InterpolateCurve(curve1, maxLength);
            List<Vector2> curveInterpoated2 = InterpolateCurve(curve2, maxLength);

            List<float> directions1 = GetVectorDirections(curveInterpoated1);
            List<float> directions2 = GetVectorDirections(curveInterpoated2);

            float totalDifference = 0;
            for (int i = 0; i < maxLength - 1; i++)
            {
                float diff = Mathf.Abs(directions1[i] - directions2[i]);
                totalDifference += Mathf.Min(diff, 2 * Mathf.PI - diff);
            }

            return totalDifference;
        }
        public static List<float> GetVectorDirections(List<Vector2> curve)
        {
            List<float> directions = new List<float>();

            for (int i = 1; i < curve.Count; i++)
            {
                Vector2 direction = curve[i] - curve[i - 1];
                float angle = (float)Mathf.Atan2(direction.y, direction.x);
                directions.Add(angle);
            }

            return directions;
        }
        public static List<Vector2> InterpolateCurve(List<Vector2> curve, int newSize)
        {
            List<Vector2> interpolatedCurve = new List<Vector2>();
            float interval = (float)(curve.Count - 1) / (newSize - 1);

            for (int i = 0; i < newSize; i++)
            {
                float pos = i * interval;
                int idx = (int)Mathf.Floor(pos);
                float t = pos - idx;

                if (idx + 1 < curve.Count)
                {
                    Vector2 interpolatedPoint = Vector2.Lerp(curve[idx], curve[idx + 1], t);
                    interpolatedCurve.Add(interpolatedPoint);
                }
                else
                {
                    interpolatedCurve.Add(curve[idx]);
                }
            }

            return interpolatedCurve;
        }

    }
}