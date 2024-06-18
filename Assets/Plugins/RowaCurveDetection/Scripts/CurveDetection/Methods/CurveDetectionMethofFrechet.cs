
using System.Collections.Generic;
using UnityEngine;

namespace Rowa.CurveDetection.Detection.Methods
{
    public class CurveDetecionMethodFrechet : ICurveDetectionMethod
    {
        public float Calculate(List<Vector2> curve1, List<Vector2> curve2)
        {
            int n = curve1.Count;
            int m = curve2.Count;
            float[,] ca = new float[n, m];

            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                    ca[i, j] = -1.0f;

            return ComputeFrechet(ca, curve1, curve2, n - 1, m - 1);
        }

        private static float ComputeFrechet(float[,] ca, List<Vector2> curve1, List<Vector2> curve2, int i, int j)
        {
            if (ca[i, j] > -1)
                return ca[i, j];

            if (i == 0 && j == 0)
            {
                ca[i, j] = Vector2.Distance(curve1[0], curve2[0]);
            }
            else if (i > 0 && j == 0)
            {
                ca[i, j] = Mathf.Max(ComputeFrechet(ca, curve1, curve2, i - 1, 0), Vector2.Distance(curve1[i], curve2[0]));
            }
            else if (i == 0 && j > 0)
            {
                ca[i, j] = Mathf.Max(ComputeFrechet(ca, curve1, curve2, 0, j - 1), Vector2.Distance(curve1[0], curve2[j]));
            }
            else if (i > 0 && j > 0)
            {
                float minPrevious = Mathf.Min(
                    Mathf.Min(ComputeFrechet(ca, curve1, curve2, i - 1, j),
                             ComputeFrechet(ca, curve1, curve2, i, j - 1)),
                             ComputeFrechet(ca, curve1, curve2, i - 1, j - 1));
                ca[i, j] = Mathf.Max(minPrevious, Vector2.Distance(curve1[i], curve2[j]));
            }
            else
            {
                ca[i, j] = float.PositiveInfinity;
            }

            return ca[i, j];
        }
    }
}
