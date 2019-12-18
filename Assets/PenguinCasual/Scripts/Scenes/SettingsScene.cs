using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsScene : MonoBehaviour
{
    private InputField _nameInputField;
    public void GoToHomeScene()
    {
        SceneManager.LoadScene("HomeScene");
    }

    public void TurnOnSound()
    {
        //TODO: Implement Turn on sound
    }
    public void TurnOffSound()
    {
        //TODO: Implement Turn on sound
    }
    
    public void OpenRequest()
    {
        //TODO: show license
    }
    
    public void OpenTerms()
    {
        //TODO: show license
    }
    
    public void OpenPolicy()
    {
        Application.OpenURL("https://my.cybird.ne.jp/sp-inq/agreement");
    }
    
    public void OpenLicense()
    {
        //TODO: show license
    }

    public void ChangeName()
    {
        if (_nameInputField.text.Length >= 6)
        {
            Debug.Log(_nameInputField.text);
        }
    }
}
