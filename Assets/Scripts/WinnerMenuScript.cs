using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinnerMenuScript : MonoBehaviour
{
    public void menuClicked(){
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
}
