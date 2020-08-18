﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHeadScript : MonoBehaviour
{
    public float chargeTime = 1f;
    public float distanceTime = 1f;
    public float speed = 2f;

    private GameObject player;
    private EnemySightScript enemySightScript;
    private Animation anim;
    private Rigidbody2D body;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        enemySightScript = GetComponent<EnemySightScript>();
        anim = GetComponent<Animation>();
        body = GetComponent<Rigidbody2D>();

        StartCoroutine(targetPlayer());
    }

    IEnumerator targetPlayer(){
        while (true){
            if (enemySightScript.getSightState() && player.GetComponent<PlayerController>().getAliveState()){
                anim.enabled = true;
                yield return new WaitForSeconds(chargeTime);
                Fire();
            }else{
                anim.enabled = false;
                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    private void Fire(){
        Vector2 direction = player.transform.position - transform.position;
        direction.Normalize();

        if (transform.position.x < player.transform.position.x)
            speed = speed / 2;
        body.velocity = direction * speed;
        StartCoroutine(stopMoving());
    }

    IEnumerator stopMoving(){
        yield return new WaitForSeconds(distanceTime);
        body.velocity = Vector2.zero;
    }
}