using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Penguin
{
    public static class Runner
    {
        static CoroutineRunner _coroutineRunner;
        static List<Action> _mainThreadActionQueue = new List<Action>();
        static List<Action> _updateActionQueue = new List<Action>();
        static List<Action> _lateUpdateActionQueue = new List<Action>();
        static readonly object _lockCall = new object();

        static Runner()
        {
            GameObject go = new GameObject("_Runner");
            GameObject.DontDestroyOnLoad(go);

            _coroutineRunner = go.AddComponent<CoroutineRunner>();
            _coroutineRunner.StartCoroutine(MainThreadUpdater());
            _coroutineRunner.OnUpdate += Update;
            _coroutineRunner.OnLateUpdate += LateUpdate;
        }

        public static Coroutine StartCoroutine(IEnumerator coroutine)
        {
            return _coroutineRunner.StartCoroutine(coroutine);
        }

        public static void StopCoroutine(Coroutine coroutine)
        {
            if (coroutine != null)
            {
                _coroutineRunner.StopCoroutine(coroutine);
            }
        }

        public static void CallOnMainThread(Action func)
        {
            if (func == null)
            {
                throw new System.Exception("Function can not be null");
            }

            lock (_lockCall)
            {
                _mainThreadActionQueue.Add(func);
            }
        }

        public static void ScheduleUpdate(Action action)
        {
            _updateActionQueue.Add(action);
        }

        public static void UnscheduleUpdate(Action action)
        {
            _updateActionQueue.Remove(action);
        }

        public static void ScheduleLateUpdate(Action action)
        {
            _lateUpdateActionQueue.Add(action);
        }

        public static void UnscheduleLateUpdate(Action action)
        {
            _lateUpdateActionQueue.Remove(action);
        }

        static IEnumerator MainThreadUpdater()
        {
            while (true)
            {
                lock (_lockCall)
                {
                    if (_mainThreadActionQueue.Count > 0)
                    {
                        for (int i = 0; i < _mainThreadActionQueue.Count; i++)
                        {
                            _mainThreadActionQueue[i].Invoke();
                        }

                        _mainThreadActionQueue.Clear();
                    }
                }

                yield return new WaitForEndOfFrame();
            }
        }

        static void Update()
        {
            for (int i = 0; i < _updateActionQueue.Count; i++)
            {
                _updateActionQueue[i].Invoke();
            }
        }

        static void LateUpdate()
        {
            for (int i = 0; i < _lateUpdateActionQueue.Count; i++)
            {
                _lateUpdateActionQueue[i].Invoke();
            }
        }

        class CoroutineRunner : MonoBehaviour
        {
            public Action OnUpdate;
            public Action OnLateUpdate;

            void Update()
            {
                OnUpdate?.Invoke();
            }

            void LateUpdate()
            {
                OnLateUpdate?.Invoke();
            }
        }
    }
}