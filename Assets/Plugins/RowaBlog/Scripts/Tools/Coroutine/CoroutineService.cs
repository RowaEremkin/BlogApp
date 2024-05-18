using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace RowaBlog.Tools.Coroutine
{
    public class CoroutineService : MonoBehaviour, ICoroutineService
    {
        public List<IEnumerator> _enumerators = new List<IEnumerator>();
        public UnityEngine.Coroutine StartOnceCoroutine(IEnumerator enumerator)
        {
            StopCoroutine(enumerator);
            return StartCoroutine(enumerator);
        }
        public new UnityEngine.Coroutine StartCoroutine(IEnumerator enumerator)
        {
            _enumerators.Add(enumerator);
            return base.StartCoroutine(enumerator);
        }
        public new bool StopCoroutine(IEnumerator enumerator)
        {
            if (_enumerators.Contains(enumerator))
            {
                _enumerators.Remove(enumerator);
                base.StopCoroutine(enumerator);
                return true;
            }
            return false;
        }
        public new void StopAllCoroutines()
        {
            _enumerators.Clear();
            base.StopAllCoroutines();
        }
    }
}
