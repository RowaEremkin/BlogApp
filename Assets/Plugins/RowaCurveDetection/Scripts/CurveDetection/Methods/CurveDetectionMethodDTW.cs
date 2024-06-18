
using System.Collections.Generic;
using UnityEngine;

namespace Rowa.CurveDetection.Detection.Methods
{
    public class CurveDetecionMethodDTW : ICurveDetectionMethod
    {
        public float Calculate(List<Vector2> curve1, List<Vector2> curve2)
        {
            int n = curve1.Count;
            int m = curve2.Count;
            float[,] dtw = new float[n + 1, m + 1];

            for (int i = 0; i <= n; i++)
                dtw[i, 0] = float.PositiveInfinity;
            for (int j = 0; j <= m; j++)
                dtw[0, j] = float.PositiveInfinity;
            dtw[0, 0] = 0;

            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= m; j++)
                {
                    float cost = Vector2.Distance(curve1[i - 1], curve2[j - 1]);
                    dtw[i, j] = cost + Mathf.Min(Mathf.Min(dtw[i - 1, j], dtw[i, j - 1]), dtw[i - 1, j - 1]);
                }
            }

            return dtw[n, m];
        }
    }
}