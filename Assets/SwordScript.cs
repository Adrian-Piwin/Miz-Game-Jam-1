using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordScript : MonoBehaviour
{
    public bool isSwinging = false;
    private Animator animator;

    void Start(){
        animator = GetComponent<Animator>();
    }

    void OnTriggerEnter2D (Collider2D other){
        if ((other.gameObject.layer == 8) && isSwinging){ // Enemy
            Destroy(other.gameObject);
        }
    }

    public void updateSwingState (bool state){
        isSwinging = state;
        if (isSwinging){
            animator.Play("swordswing");
        }
    }
}
