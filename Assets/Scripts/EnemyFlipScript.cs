using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlipScript : MonoBehaviour
{

    private EnemySightScript enemySightScript;
    private Transform player;

    // Start is called before the first frame update
    void Start()
    {
        enemySightScript = GetComponent<EnemySightScript>();
        player = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (enemySightScript.getSightState()){
            if (player.position.x < transform.position.x){
                transform.localRotation = Quaternion.Euler(0, 180, 0); // facing left
            }else{
                transform.localRotation = Quaternion.Euler(0, 0, 0); // facing right
            }
        }
    }
}
