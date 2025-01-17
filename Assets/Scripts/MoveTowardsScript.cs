﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTowardsScript : MonoBehaviour
{
    public float speed;
    private GameObject target;
    private Rigidbody2D body;
    private PlayerController playerController;
    private Animator anim;
    private EnemySightScript enemySightScript;
    private bool isFollowing = true;
    

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        enemySightScript = GetComponent<EnemySightScript>();
        target = GameObject.Find("Player");
        playerController = target.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {   
        if (playerController.getAliveState() && enemySightScript.getSightState() && isFollowing){
            anim.enabled = true;   

            if (transform.position.x < target.transform.position.x){
                StartCoroutine(stopFollowing());
            }

            body.velocity = ((target.transform.position - transform.position) * speed);
        }
        else{
            body.velocity = Vector2.zero;
            anim.enabled = false;   
        }
    }

    IEnumerator stopFollowing(){
        yield return new WaitForSeconds(3.5f);
        isFollowing = false;
    }
}
