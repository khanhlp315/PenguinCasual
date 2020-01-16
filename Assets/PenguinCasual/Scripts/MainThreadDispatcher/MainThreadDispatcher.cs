using System.Collections;
using System.Collections.Generic;
using Penguin.Utilities;
using UnityEngine;
using UnityEngine.Events;

namespace Penguin.MainThreadDispatcher
{
    public class MainThreadDispatcher: MonoSingleton<MainThreadDispatcher, MainThreadDispatcherConfig>
    {
        private static readonly Queue<UnityAction> _executionQueue = new Queue<UnityAction>();

        public void Update() {
            lock(_executionQueue) {
                while (_executionQueue.Count > 0) {
                    _executionQueue.Dequeue().Invoke();
                }
            }
        }

        public void Enqueue(IEnumerator action) {
            lock (_executionQueue) {
                _executionQueue.Enqueue (() => {
                    StartCoroutine (action);
                });
            }
        }

        public void Enqueue(UnityAction action)
        {
            Enqueue(ActionWrapper(action));
        }
        IEnumerator ActionWrapper(UnityAction action)
        {
            Debug.Log("Running on main thraed");
            action();
            yield return null;
        }

        public override void Initialize()
        {
            OnInitializeDone?.Invoke();
        }
    }
}