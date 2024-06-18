using System.Collections.Generic;
using UnityEngine;

namespace Rowa.CurveDetection.Detection
{
    public class CurveSampleStorage : MonoBehaviour, ICurveSampleStorage
    {
        [SerializeField] List<CurveSampleData> _samples = new List<CurveSampleData>();
        public List<CurveSampleData> Samples => _samples;

        public void AddSample(CurveSampleData sample)
        {
            if (sample == null || string.IsNullOrEmpty(sample.Key) || sample.Points.Count <= 1) return;
            _samples.Add(sample);
            _samples.Sort((s1,s2)=>s1.Key.CompareTo(s2.Key));
        }
    }
}
