using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Penguin.Scenes
{
    public class SplashScene : MonoBehaviour
    {
        void Start()
        {
            InitCrashlytics();
            StartCoroutine(GoToHomeScreen());
        }

        private void InitCrashlytics()
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

        // Update is called once per frame
        IEnumerator GoToHomeScreen()
        {
            var hasAcceptTerms = PlayerPrefs.HasKey("HasAcceptTerms");
            yield return new WaitForSeconds(3.0f);
            SceneManager.LoadScene(hasAcceptTerms ? "HomeScene" : "AcceptTermsScene");
            yield return null;
        }
    }
}
