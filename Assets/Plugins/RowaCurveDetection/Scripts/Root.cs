using Rowa.CurveDetection.AddSample;
using Rowa.CurveDetection.Detection;
using TMPro;
using UnityEngine;

namespace Rowa.CurveDetection.Paint
{
    public class Root : MonoBehaviour
    {
        [SerializeField] private PaintCurveView paintCurveView;
        [SerializeField] private LineRenderer paintCurveLineRenderer;
        [SerializeField] private Camera mainCamera;
        [SerializeField] private CurveSampleStorage curveSampleStorage;
        [SerializeField] private PaintCurveInfoView paintCurveInfoView;
        [SerializeField] private AddSampleView addSampleView;
        [SerializeField] private CurveDetectionSetting curveDetectionSetting;
        [SerializeField] private ParticleSystem pointerParticle;
        private IPaintCurvePresenter paintCurvePresenter;
        private ICurveDetectionController curveDetectionController;
        private IAddSamplePresenter addSamplePresenter;
        private void Awake()
        {
            curveDetectionController = new CurveDetectionController(
                curveSampleStorage, 
                curveDetectionSetting, 
                new CurveDetection.Detection.Methods.CurveDetecionMethodVectors());
            paintCurvePresenter = new PaintCurvePresenter(new PaintCurvePresenterData()
            {
                CurveDetectionController = curveDetectionController,
                PaintCurveView = paintCurveView,
                LineRenderer = paintCurveLineRenderer,
                Camera = mainCamera,
                PaintCurveInfoView = paintCurveInfoView,
                ParticleSystem = pointerParticle
            });
            addSamplePresenter = new AddSamplePresenter(addSampleView, curveSampleStorage, paintCurveView);
        }
    }
}