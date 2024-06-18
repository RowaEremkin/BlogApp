using UnityEngine;

namespace Rowa.CurveDetection.Paint
{
    [CreateAssetMenu(fileName = "PaintCurveSetting", menuName = "ScriptableObjects/PaintCurveSetting", order = 1)]
    public class PaintCurveSetting : ScriptableObject
    {
        [field: SerializeField] public float NextPointDistance = 1f;
    }
}
