using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerationScript : MonoBehaviour
{
    public Transform levelOne;
    public Transform levelTutorialOne;
    public Transform levelLast;
    public Transform levelHeal;
    public List<Transform> levelsTutorial;
    public List<Transform> levelsEasy;
    public List<Transform> levelsMedium;
    public List<Transform> levelsHard;
    public Transform startPosition;
    public GameObject bossObj;

    private bool isBossFight = false;
    private Vector3 lastEndPosition;
    private Transform lastLevelTransform;
    private Transform behindLevelTransform;
    private Vector3 behindLevelEndPosition;
    private int difficulty = 0; // 0 easy, 1 medium, 2 hard
    private bool isGameOver = false;
    private bool isGameWon = false;
    private PlayerController playerController;
    private CameraMovement cameraMovement;

    // Start is called before the first frame update
    void Awake()
    { 
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        cameraMovement = GameObject.Find("Main Camera").GetComponent<CameraMovement>();
        if (PlayerPrefs.GetInt("tutorial", 1) == 1){
            spawnFirstTutorialLevel();
            difficulty = -1;
        }else{
            spawnFirstLevel();
        }
    }

    void Update(){
        if (startPosition.position.x >= behindLevelEndPosition.x && !isGameOver){
            if (behindLevelTransform != null) Destroy(behindLevelTransform.gameObject);
            spawnLevel();
        }

        if (isGameWon){
            if (startPosition.position.x > lastEndPosition.x){
                cameraMovement.enableCamera(false);
                playerController.GameWon();
                isGameWon = false;
            }
        }
    }

    private void spawnLevel(){
        // Choose levels from list
        Transform chosenLevel = levelOne;
        int ranNum;

        if (!isBossFight){
            switch (difficulty){
                case -1:
                    if (levelsTutorial.Count == 0){
                        PlayerPrefs.SetInt("tutorial", 0);
                        difficulty = 0;
                        goto case 3;
                    }else{
                        chosenLevel = levelsTutorial[0];
                        levelsTutorial.RemoveAt(0);
                    }
                    break;
                case 0: 
                    if (levelsEasy.Count == 0){
                        difficulty = 1;
                        goto case 3;
                    }else{
                        ranNum = Random.Range(0, levelsEasy.Count);
                        chosenLevel = levelsEasy[ranNum];
                        levelsEasy.RemoveAt(ranNum);
                        break;
                    }
                case 1: 
                    if (levelsMedium.Count == 0){
                        difficulty = 2;
                        goto case 3;
                    }else{
                        ranNum = Random.Range(0, levelsMedium.Count);
                        chosenLevel = levelsMedium[ranNum];
                        levelsMedium.RemoveAt(ranNum);
                        break;
                    }
                case 2: 
                    if (levelsHard.Count == 0){
                        isBossFight = true;
                        StartCoroutine(spawnBoss());
                        goto case 3;
                    }else{
                        ranNum = Random.Range(0, levelsHard.Count);
                        chosenLevel = levelsHard[ranNum];
                        levelsHard.RemoveAt(ranNum);
                    }
                    break;
                case 3:
                    chosenLevel = levelHeal;
                    break;
                default:
                    break;
            }
        }

        if (!isGameOver){
            behindLevelTransform = lastLevelTransform;
            behindLevelEndPosition = lastEndPosition;
            lastLevelTransform = spawnLevel(lastEndPosition, chosenLevel);
            lastEndPosition = lastLevelTransform.Find("EndPosition").position;
        }
    }

    private void spawnFirstLevel(){
        lastLevelTransform = spawnLevel(new Vector3(-12,0,0), levelOne);
        lastEndPosition = lastLevelTransform.Find("EndPosition").position;
    }

    private void spawnFirstTutorialLevel(){
        lastLevelTransform = spawnLevel(new Vector3(-12,0,0), levelTutorialOne);
        lastEndPosition = lastLevelTransform.Find("EndPosition").position;
    }

    private Transform spawnLevel(Vector3 spawnPos, Transform level){
        Transform levelTransform = Instantiate(level, spawnPos + new Vector3(12,0,0), Quaternion.identity);
        levelTransform.parent = GameObject.Find("Grid").transform;
        return levelTransform;
    }

    public void startGeneration(){
        spawnLevel();
    }

    public void stopGeneration(){
        isGameOver = true;
    }

    private void gameWon(){
        isGameOver = true;
        isGameWon = true;
        Transform levelTransform = Instantiate(levelLast, lastEndPosition + new Vector3(12,0,0), Quaternion.identity);
        levelTransform.parent = GameObject.Find("Grid").transform;
    }

    IEnumerator spawnBoss(){
        GameObject.Find("Music Manager").GetComponent<MusicManagerScript>().SwitchBossMusic();
        yield return new WaitForSeconds(10f);
        // Instantiate boss
        Instantiate(bossObj, GameObject.Find("Main Camera").gameObject.transform);
    }

    public void bossDefeated(){
        GameObject.Find("Music Manager").GetComponent<MusicManagerScript>().endBossMusic();
        gameWon();
    }

}
