using System.Collections.Generic;

namespace Rowa.CurveDetection.Detection
{
    public interface ICurveSampleStorage
    {
        public List<CurveSampleData> Samples { get; }
        public void AddSample(CurveSampleData sample);
    }
}