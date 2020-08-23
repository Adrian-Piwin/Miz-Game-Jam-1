using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{   
    public Toggle tutorialToggle;

    void Start(){
        if (PlayerPrefs.GetInt("tutorial", 1) == 1)
            tutorialToggle.isOn = true;
        else tutorialToggle.isOn = false;
    }

    public void playClicked(){
        SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
    }

    public void exitClicked(){
        Application.Quit();
    }

    public void tutorialToggled(){
        if (tutorialToggle.isOn){
            PlayerPrefs.SetInt("tutorial", 1);
        }else{
            PlayerPrefs.SetInt("tutorial", 0);
        }
        
    }
}
