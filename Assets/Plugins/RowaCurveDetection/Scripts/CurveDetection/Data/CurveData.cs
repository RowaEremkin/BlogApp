using System.Collections.Generic;
using UnityEngine;

namespace Rowa.CurveDetection.Detection
{
    [System.Serializable]
    public class CurveData
    {
        public List<Vector2> Points;
        public float CalculateLength()
        {
            float length = 0;
            if (Points == null || Points.Count <= 1) return length;
            Vector2 prevPosition = Points[0];
            Vector2 position = Vector2.zero;
            float distance = 0;
            for (int i = 1; i < Points.Count; i++)
            {
                position = Points[i];
                distance = Vector2.Distance(prevPosition, position);
                prevPosition = position;
                length += distance;
            }
            return length;
        }
        public float CalculateSize()
        {
            float maxSize = 0;
            if (Points == null || Points.Count <= 1) return maxSize;
            for (int i = 0; i < Points.Count; i++)
            {
                for (int j = 1; j < Points.Count; j++)
                {
                    float distance = Vector2.Distance(Points[i], Points[j]);
                    if (distance > maxSize)
                    {
                        maxSize = distance;
                    }
                }
            }
            return maxSize;
        }
        public float CalculateBendability()
        {
            float bendability = 0;
            if (Points == null || Points.Count <= 1) return bendability;
            Vector2 prevPosition = Points[0];
            Vector2 prevVector = Vector2.zero;
            Vector2 position = Vector2.zero;
            Vector2 vector = Vector2.zero;
            float angle = 0;
            for (int i = 1; i < Points.Count; i++)
            {
                position = Points[i];
                vector = position - prevPosition;
                if (prevVector != Vector2.zero)
                {
                    angle = Vector2.Angle(prevVector, vector);
                    bendability += angle;
                }
                prevPosition = position;
                prevVector = vector;
            }
            return bendability;
        }
        public CurveData GetNormilizedCurve()
        {
            CurveData curveData = new CurveData();
            List<Vector2> newList = new List<Vector2>();
            float maxSize = CalculateSize();
            if (maxSize <= 0) return curveData;
            foreach (Vector2 position in Points)
            {
                newList.Add(position / maxSize);
            }
            curveData.Points = newList;
            return curveData;
        }
    }
}
