using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceSpellDestroy : MonoBehaviour
{
    public GameObject destroyParticle;
    private int bounceMax = 1;
    void OnCollisionEnter2D (Collision2D other){
        if (other.gameObject.tag == "Player"){
            Destroy(gameObject);
        }

        if ((other.gameObject.layer == 9 || other.gameObject.layer == 0) && bounceMax == 0){
            Instantiate(destroyParticle, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }else bounceMax --;
    }
}
