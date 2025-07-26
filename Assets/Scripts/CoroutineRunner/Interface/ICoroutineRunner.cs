using System.Collections;
using UnityEngine;

namespace CoroutineRunner.Interface
{
    public interface ICoroutineRunner
    {
        public Coroutine StartCoroutine(IEnumerator coroutine);
        
        public void StopCoroutine(IEnumerator coroutine);
    }
}