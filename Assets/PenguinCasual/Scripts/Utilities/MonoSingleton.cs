using System;
using UnityEngine;
using UnityEngine.Events;

namespace Penguin.Utilities
{
    public abstract class MonoSingleton<T>: MonoBehaviour where T: MonoBehaviour
    {
        protected static T _instance;
        private static object _lock = new object();

        public static T Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance != null)
                    {
                        return _instance;
                    }

                    var go = new GameObject();
                    _instance = go.AddComponent<T>();
                    _instance.gameObject.name = "[" + typeof(T) + "]";
                    return _instance;
                }
            }
        }

        protected virtual void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
        }

        public UnityAction OnInitializeDone;

        public abstract void Initialize();
    }
}