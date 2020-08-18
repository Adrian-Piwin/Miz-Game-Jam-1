using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellDestroyScript : MonoBehaviour
{
    void OnCollisionEnter2D (Collision2D other){
        if (other.gameObject.layer == 9 || other.gameObject.layer == 0 || other.gameObject.tag == "Player"){
            Destroy(gameObject);
        }
    }
}
