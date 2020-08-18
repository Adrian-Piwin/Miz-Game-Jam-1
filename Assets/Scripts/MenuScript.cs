using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{

    public void playClicked(){
        SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
    }

    public void exitClicked(){
        Application.Quit();
    }
}
