using System.Collections.Generic;
using UnityEngine;

namespace Rowa.CurveDetection.Detection.Methods
{
    public interface ICurveDetectionMethod
    {
        float Calculate(List<Vector2> curve1, List<Vector2> curve2);
    }
}