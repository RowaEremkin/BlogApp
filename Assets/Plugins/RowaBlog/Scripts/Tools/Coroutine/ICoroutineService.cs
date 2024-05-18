using System.Collections;

namespace RowaBlog.Tools.Coroutine
{
    public interface ICoroutineService
    {
        public UnityEngine.Coroutine StartOnceCoroutine(IEnumerator enumerator);
        public UnityEngine.Coroutine StartCoroutine(IEnumerator enumerator);
        public bool StopCoroutine(IEnumerator enumerator);
        public void StopAllCoroutines();
    }
}