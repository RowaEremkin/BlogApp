
using Rowa.CurveDetection.Detection.Methods;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace Rowa.CurveDetection.Detection
{
    public class CurveDetectionController : ICurveDetectionController
    {
        #region Fields
        private readonly ICurveSampleStorage curveSampleStorage;
        private readonly CurveDetectionSetting curveDetectionSetting;
        private readonly ICurveDetectionMethod curveDetectionMethod;
        private static bool DebugGetSample = false;
        #endregion
        #region Init\Dispose
        public CurveDetectionController(
            ICurveSampleStorage curveSampleStorage, 
            CurveDetectionSetting curveDetectionSetting, 
            ICurveDetectionMethod curveDetectionMethod
            )
        {
            this.curveSampleStorage = curveSampleStorage;
            this.curveDetectionSetting = curveDetectionSetting;
            this.curveDetectionMethod = curveDetectionMethod;
        }
        #endregion
        #region Public
        public CurveDetectionResultData GetSameSample(CurveData curveData)
        {
            CurveSampleData bestSample = null;
            float bestDistance = float.MaxValue;
            string debugString = "Variants:\n";
            CurveData normilizedCurve = curveData.GetNormilizedCurve();
            float bendability = normilizedCurve.CalculateBendability();
            float maxDistanceThreshold = curveDetectionSetting.MaxDistanceThreshold * (bendability/90f);
            Dictionary<CurveSampleData, float> same = new Dictionary<CurveSampleData, float>();
            foreach (CurveSampleData sample in curveSampleStorage.Samples)
            {
                List<Vector2> samplePoints = sample.Points;
                for (int i = 0; i <= (curveDetectionSetting.AddReversed?1:0); i++)
                {
                    if (i == 1) samplePoints.Reverse();
                    float distance = curveDetectionMethod.Calculate(new CurveData()
                    {
                        Points = samplePoints
                    }.GetNormilizedCurve().Points, normilizedCurve.Points);
                    debugString += $"{sample.Key}{(i==1?"(Reversed)":"")} - distance: {distance}\n";
                    if (distance < bestDistance && maxDistanceThreshold > distance)
                    {
                        bestSample = sample;
                        bestDistance = distance;
                    }
                    if (!same.ContainsKey(sample))
                    {
                        same.Add(sample, distance);
                    }
                }
            }
            debugString = $"Final key: {(bestSample==null?"null":bestSample.Key)} - distance: {bestDistance}\n" + debugString;
            if(DebugGetSample) Debug.Log(debugString);
            List<KeyValuePair<CurveSampleData, float>> list = same.ToList();
            list.Sort((p1, p2) => p1.Value.CompareTo(p2.Value));
            same = list.ToDictionary(pair => pair.Key, pair => pair.Value);
            return new CurveDetectionResultData()
            {
                SampleData = bestSample,
                SameDistance = Mathf.Clamp01(bestDistance/maxDistanceThreshold),
                Same = same,
                AdditionData = new CurveDetectionAdditionData()
                {
                    Size = curveData.CalculateSize(),
                    Bendability = bendability,
                    Threshold = maxDistanceThreshold,
                    Length = curveData.CalculateLength(),
                }
            };
        }
        #endregion
    }
}
