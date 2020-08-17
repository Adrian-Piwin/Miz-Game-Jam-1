using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 10.0f;
    public float swordCooldown = 1f;
    public float swordSwingTime = 0.3f;
    public GameObject sword;
    public GameObject coolDownBar;

    private Animator animator;
    private bool gameStarted = false;
    private Rigidbody2D body;
    private float horizontal;
    private float vertical;
    private bool canSwingSword = true;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        // Start game on movement
        if ((horizontal != 0 || vertical != 0) && !gameStarted){
            gameStarted = true;
            StartGame();
        }

        if (Input.GetMouseButtonDown(0) && canSwingSword){
            canSwingSword = false;
            coolDownBar.GetComponent<CooldownScript>().enableCooldown(swordCooldown);
            sword.GetComponent<SwordScript>().updateSwingState(true);
            StartCoroutine(swingSword());
        }
    }

    void FixedUpdate() {
        Vector2 movement = new Vector2(horizontal, vertical);
        movement = movement.normalized * Time.deltaTime * speed;
        body.velocity = movement;
    }

    IEnumerator swingSword(){
        yield return new WaitForSeconds(swordSwingTime);
        sword.GetComponent<SwordScript>().updateSwingState(false);
        if (swordCooldown > swordSwingTime)
            yield return new WaitForSeconds(swordCooldown - swordSwingTime);
        canSwingSword = true;
    }

    private void StartGame(){
        animator.SetBool("isMoving", true);
    }

    private void GameOver(){
        Debug.Log("ded");
    }

    void OnCollisionEnter2D (Collision2D other){
        switch (other.gameObject.layer){
            case 8: // Enemy   
                GameOver();
                break;
            case 9: // Walls
                GameOver();
                break;
            default:
                break;
        }

    }

}
