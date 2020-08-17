using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerationScript : MonoBehaviour
{
    public Transform levelOne;
    public Transform[] levelsEasy;
    public Transform[] levelsMedium;
    public Transform[] levelsHard;
    public Transform startPosition;

    private Vector3 lastEndPosition;
    private Transform lastLevelTransform;
    private Transform behindLevelTransform;
    private Vector3 behindLevelEndPosition;
    private int difficulty = 0; // 0 easy, 1 medium, 2 hard
    private int levelIndex = 0;
    private bool isGameOver = false;

    // Start is called before the first frame update
    void Awake()
    { 
        spawnFirstLevel();
    }

    void Update(){
        if (startPosition.position.x >= behindLevelEndPosition.x && !isGameOver){
            if (behindLevelTransform != null) Destroy(behindLevelTransform.gameObject);
            spawnLevel();
        }
    }

    private void spawnLevel(){
        // Choose levels from list
        Transform chosenLevel = levelOne;

        switch (difficulty){
            case 0: 
                if (levelIndex >= levelsEasy.Length){
                    difficulty = 1;
                    levelIndex = 0;
                    goto case 1;
                }else{
                    chosenLevel = levelsEasy[levelIndex];
                    break;
                }
            case 1: 
                if (levelIndex >= levelsMedium.Length){
                    difficulty = 2;
                    levelIndex = 0;
                    goto case 2;
                }else{
                    chosenLevel = levelsMedium[levelIndex];
                    break;
                }
            case 2: 
                if (levelIndex >= levelsHard.Length){
                    gameWon();
                }else{
                    chosenLevel = levelsHard[levelIndex];
                }
                break;
                
            default:
                break;
        }

        levelIndex += 1;

        behindLevelTransform = lastLevelTransform;
        behindLevelEndPosition = lastEndPosition;
        lastLevelTransform = spawnLevel(lastEndPosition, chosenLevel);
        lastEndPosition = lastLevelTransform.Find("EndPosition").position;
    }

    private void spawnFirstLevel(){
        lastLevelTransform = spawnLevel(new Vector3(-12,0,0), levelOne);
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
        Debug.Log("winner");
    }

}
