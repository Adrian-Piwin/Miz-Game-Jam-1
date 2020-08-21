using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDestroyScript : MonoBehaviour
{   
    public GameObject destroyParticle;
    
    void OnCollisionEnter2D (Collision2D other){
        if (other.gameObject.layer == 11){ // Player
            destroyEnemy();
        }
    }

    public void destroyEnemy(){
        Instantiate(destroyParticle, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

}
