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
            StartCoroutine(GoToHomeScreen());
        }

        // Update is called once per frame
        IEnumerator GoToHomeScreen()
        {
            yield return new WaitForSeconds(3.0f);
            SceneManager.LoadScene("HomeScene");
            yield return null;
        }
    }
}
