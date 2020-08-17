using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardSpellScript : MonoBehaviour
{
    void OnCollisionEnter2D (Collision2D other){
        if (other.gameObject.layer == 9){
            Destroy(gameObject);
        }
    }
}
