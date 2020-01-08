using System.Collections;
using System.Collections.Generic;
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

        private int _systemToLoad = 0;
        void Start()
        {
            PlayerPrefsHelper.CheckDate();
            InitFirebase();
            
            NativeDialogManager.Instance.OnInitializeDone += OnSystemLoad;
            NetworkCaller.Instance.OnInitializeDone += OnSystemLoad;
            Sound2DManager.Instance.OnInitializeDone += OnSystemLoad;
            _systemToLoad++;
            _systemToLoad++;
            _systemToLoad++;

            NativeDialogManager.Instance.Initialize();
            NetworkCaller.Instance.Initialize();
            Sound2DManager.Instance.Initialize();
        }

        private void OnSystemLoad()
        {
            _systemsLoaded++;
            if (_systemsLoaded >= _systemToLoad)
            {
                StartCoroutine(GoToHomeScreen());
            }
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

                    // Set a flag here for indicating that your project is ready to use Firebase.
                }
                else
                {
                    UnityEngine.Debug.LogError($"Could not resolve all Firebase dependencies: {dependencyStatus}");
                    // Firebase Unity SDK is not safe to use here.
                }
            });
        }

        IEnumerator GoToHomeScreen()
        {
            var hasAcceptTerms = !PlayerPrefsHelper.IsFirstTimeUser();
            yield return new WaitForSeconds(3.0f);
            SceneManager.LoadScene(hasAcceptTerms ? "HomeScene" : "AcceptTermsScene");
            yield return null;
        }
    }
}
