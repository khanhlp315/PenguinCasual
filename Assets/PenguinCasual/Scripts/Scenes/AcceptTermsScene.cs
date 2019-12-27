using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Penguin.Scenes
{
    public class AcceptTermsScene : MonoBehaviour
    {
        void Start()
        {
            
        }
        
        // Update is called once per frame
        private void GoToHomeScreen()
        {
            SceneManager.LoadScene("HomeScene");
        }
    }
}
