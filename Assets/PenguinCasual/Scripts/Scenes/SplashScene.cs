using System;
using System.Collections;
using System.Collections.Generic;
using Penguin.AppConfigs;
using Penguin.Dialogues;
using Penguin.Network;
using Penguin.Sound;
using Penguin.Utilities;
using PenguinCasual.Scripts.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Penguin.Scenes
{
    public class SplashScene : MonoBehaviour
    {
        private int _systemsLoaded = 0;

        private readonly List<ISystem> _systems = new List<ISystem>();

        private void Awake()
        {
            _systems.Add(NativeDialogManager.Instance);
            _systems.Add(AppConfigManager.Instance);
            _systems.Add(NetworkCaller.Instance);
            _systems.Add(Sound2DManager.Instance);
        }

        private bool _isFirebaseInitializeDone = false;
        void Start()
        {
            PlayerPrefsHelper.CheckDate();
            InitFirebase();
            StartCoroutine(InitSystems());
            StartCoroutine(WaitToGoToHomeScene());
        }

        private void OnSystemLoad()
        {
            _systemsLoaded++;
        }

        public IEnumerator WaitToGoToHomeScene()
        {
            yield return new WaitForSeconds(3.0f);
            while (_systemsLoaded < _systems.Count)
            {
                yield return null;
            }
            var hasAcceptTerms = !PlayerPrefsHelper.IsFirstTimeUser();
            SceneManager.LoadScene(hasAcceptTerms ? "HomeScene" : "AcceptTermsScene");
            yield return null;
        }

        private void InitFirebase()
        {
            Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
                var dependencyStatus = task.Result;
                if (dependencyStatus == Firebase.DependencyStatus.Available)
                {
                    // Create and hold a reference to your FirebaseApp,
                    // where app is a Firebase.FirebaseApp property of your application class.
                    // Crashlytics will use the DefaultInstance, as well;
                    // this ensures that Crashlytics is initialized.
                    Firebase.FirebaseApp app = Firebase.FirebaseApp.DefaultInstance;
                    _isFirebaseInitializeDone = true;
                }
                else
                {
                    UnityEngine.Debug.LogError($"Could not resolve all Firebase dependencies: {dependencyStatus}");
                    // Firebase Unity SDK is not safe to use here.
                }
            });
        }

        private IEnumerator InitSystems()
        {
            while (!_isFirebaseInitializeDone)
            {
                yield return null;
            }
            
            for(int i = 0; i < _systems.Count; ++i)
            {
                var system = _systems[i];
                system.OnInitializeDone += () => { _systemsLoaded++; };
                system.Initialize();
                while (_systemsLoaded <= i)
                {
                    yield return null;
                }
            }
        }
    }
}
