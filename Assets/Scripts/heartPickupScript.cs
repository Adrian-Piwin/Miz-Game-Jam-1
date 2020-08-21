using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class heartPickupScript : MonoBehaviour
{
    public GameObject particles;
    private PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    void OnTriggerEnter2D(Collider2D other){
        if (other.gameObject.tag == "Player"){
            playerController.healthPickup();
            Instantiate(particles, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
