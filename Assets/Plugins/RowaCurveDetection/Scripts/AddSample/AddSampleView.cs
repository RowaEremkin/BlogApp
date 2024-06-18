using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Rowa.CurveDetection.AddSample
{
    public class AddSampleView : MonoBehaviour, IAddSampleView
    {
        #region Fields

        [Header("Require")]
        [SerializeField] private TMP_InputField _inputName;
        [SerializeField] private Button _buttonAdd;

        #endregion

        #region Events

        public event Action<string> OnCurveAdd;

        #endregion

        #region Unity

        private void Awake()
        {
            _buttonAdd.onClick.AddListener(Add);
        }

        #endregion

        #region Event Handlers

        private void Add()
        {
            OnCurveAdd?.Invoke(_inputName.text);
        }

        #endregion

        #region Public

        public void ResetInput()
        {
            _inputName.SetTextWithoutNotify("");
        }

        #endregion
    }
}
