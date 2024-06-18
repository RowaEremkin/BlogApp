using System;

namespace Rowa.CurveDetection.AddSample
{
    public interface IAddSampleView
    {
        event Action<string> OnCurveAdd;
        public void ResetInput();
    }
}