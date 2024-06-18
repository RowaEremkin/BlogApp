using UnityEngine;

namespace Rowa.CurveDetection.Detection
{
    [CreateAssetMenu(fileName = "CurveDetectionSetting", menuName = "ScriptableObjects/CurveDetectionSetting", order = 1)]
    public class CurveDetectionSetting : ScriptableObject
    {
        [field: SerializeField] public float MaxDistanceThreshold = 100;
        [field: SerializeField] public bool AddReversed = true;
    }
}
