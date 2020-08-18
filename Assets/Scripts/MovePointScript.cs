using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePointScript : MonoBehaviour
{
    public float speed;
    private Rigidbody2D body;
    private PlayerController playerController;
    private Animator anim;
    private EnemySightScript enemySightScript;
    private Vector3 targetPos;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        enemySightScript = GetComponent<EnemySightScript>();
        targetPos = transform.GetChild(0).transform.position;
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {   
        if (playerController.getAliveState() && enemySightScript.getSightState()){
            body.velocity = ((targetPos - transform.position) * speed);
        }
        else{
            body.velocity = Vector2.zero;
            anim.enabled = false;   
        }
    }

}
