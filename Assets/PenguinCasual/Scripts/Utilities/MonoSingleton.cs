using System;
using PenguinCasual.Scripts.Utilities;
using UnityEngine;
using UnityEngine.Events;

namespace Penguin.Utilities
{
    public abstract class MonoSingleton<T1, T2>: MonoBehaviour, ISystem where T1: MonoBehaviour where T2: ScriptableObject
    {
        
        protected static T1 _instance;
        private static object _lock = new object();

        protected T2 _config;

        public static T1 Instance
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
                    _instance = go.AddComponent<T1>();
                    _instance.gameObject.name = "[" + typeof(T1) + "]";
                    return _instance;
                }
            }
        }

        protected virtual void Awake()
        {
            _config = Resources.Load<T2>($"Databases/{typeof(T2).Name}");
            DontDestroyOnLoad(gameObject);
        }

        

        public abstract void Initialize();
        public UnityAction OnInitializeDone { get; set; }
    }
}