using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenuScript : MonoBehaviour
{
    public GameObject gameOverMenu;

    public void playClicked(){
        SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
    }

    public void menuClicked(){
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    public void loadWinScreen(){
        SceneManager.LoadScene("GameWon", LoadSceneMode.Single);
    }

    public void toggleMenu(bool toggle){
        gameOverMenu.SetActive(toggle);
    }
}
