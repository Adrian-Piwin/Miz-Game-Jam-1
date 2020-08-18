﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceSpellDestroy : MonoBehaviour
{
    private int bounceMax = 3;
    void OnCollisionEnter2D (Collision2D other){
        if (other.gameObject.tag == "Player"){
            Destroy(gameObject);
        }

        if (other.gameObject.layer == 9 && bounceMax == 0)
            Destroy(gameObject);
        else bounceMax --;
    }
}