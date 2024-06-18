
using Rowa.CurveDetection.Detection;
using Rowa.CurveDetection.Paint.Data;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Rowa.CurveDetection.Paint
{
    public class PaintCurveInfoView : MonoBehaviour, IPaintCurveInfoView
    {
        [SerializeField] TextMeshProUGUI textResult;
        [SerializeField] TextMeshProUGUI textSame;
        [SerializeField] TextMeshProUGUI textAdditionInfo;
        public void SetInfo(PaintCurveInfoViewData data)
        {
            if (data.ResultData == null)
            {
                if (textResult) textResult.text = "";
                if (textSame) textSame.text = "";
                if (textAdditionInfo) textAdditionInfo.text = "";
            }
            else
            {
                float score = (1 - data.ResultData.SameDistance) * data.ResultData.AdditionData.Size * 100;

                if (textResult) textResult.text = $"{(data.ResultData.SampleData == null ? "null" : data.ResultData.SampleData.Key)} Score: {score.ToString("N0")}";
                if (textResult != null)
                {
                    if (data.End)
                    {
                        textResult.color = data.ResultData.SampleData == null ? Color.red : Color.green;
                    }
                    else
                    {
                        textResult.color = data.ResultData.SampleData == null ? Color.gray : Color.white;
                    }
                }

                string same = "";
                foreach (KeyValuePair<CurveSampleData, float> item in data.ResultData.Same)
                {
                    same += $"{item.Key.Key} - {item.Value.ToString("0.00")}\n";
                }
                if (textSame) textSame.text = same;

                if (textAdditionInfo) textAdditionInfo.text = $"Distance: {(1 - data.ResultData.SameDistance).ToString("0.00")}\nSize: {data.ResultData.AdditionData.Size.ToString("0.00")}\nBendability: {data.ResultData.AdditionData.Bendability.ToString("0")}\nThreshold: {data.ResultData.AdditionData.Threshold.ToString("0")}";
            }
        }
    }
}
