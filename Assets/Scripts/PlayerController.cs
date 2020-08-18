using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Settings")]
    public float speed = 10.0f;
    public float swordCooldown = 1f;
    public float swordSwingTime = 0.3f;
    public float jumpTime = 0.5f;
    public float hitSomethingForce = 100f;
    public int playerHearts = 3;

    [Header("References")]
    public Rigidbody2D body;
    public GameObject sword;
    public GameObject coolDownBar;
    public CameraMovement cameraScript;
    public LevelGenerationScript levelGenerationScript;
    public Transform playerHeartUI;

    private Animator animator;
    private bool isGamePlaying = false;
    private float horizontal;
    private float vertical;
    private bool canSwingSword = true;
    private bool isJumping = false;
    private bool isAlive;
    private bool isHit = false;

    // Start is called before the first frame update
    void Start()
    {
        //body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        isAlive = true;
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        // Start game on movement
        if ((horizontal != 0 || vertical != 0) && !isGamePlaying && isAlive){
            StartGame();
        }
        
        // Sword swing
        if (Input.GetMouseButtonDown(0) && canSwingSword && isGamePlaying){
            canSwingSword = false;
            coolDownBar.GetComponent<CooldownScript>().enableCooldown(swordCooldown);
            sword.GetComponent<SwordScript>().updateSwingState(true);
            StartCoroutine(swingSword());
        }

        // Jump
        if (Input.GetKeyDown("space") && isGamePlaying){
            StartCoroutine(jump());
        }
    }

    void FixedUpdate() {
        if (isGamePlaying && !isHit){
            Vector2 movement = new Vector2(horizontal, vertical);
            movement = movement.normalized * Time.deltaTime * speed;
            body.velocity = movement;
        }  
    }

    IEnumerator swingSword(){
        yield return new WaitForSeconds(swordSwingTime);
        sword.GetComponent<SwordScript>().updateSwingState(false);
        if (swordCooldown > swordSwingTime)
            yield return new WaitForSeconds(swordCooldown - swordSwingTime);
        canSwingSword = true;
    }

    IEnumerator jump(){
        isJumping = true;
        canSwingSword = false;
        animator.Play("jumpanim");
        yield return new WaitForSeconds(jumpTime);
        isJumping = false;
        canSwingSword = true;
    }

    private void StartGame(){
        isGamePlaying = true;
        animator.SetBool("isMoving", true);
        cameraScript.enableCamera(true);
        levelGenerationScript.startGeneration();
    }

    private void GameOver(){
        takeDamage();
        if (playerHearts == 0){
            isGamePlaying = false;
            isAlive = false;
            animator.SetBool("isMoving", false);
            body.bodyType = RigidbodyType2D.Static;
            animator.Play("deathanim");
            cameraScript.enableCamera(false);
            levelGenerationScript.stopGeneration();
        }else{
            body.AddForce(transform.right * -1 * hitSomethingForce, ForceMode2D.Impulse);
            animator.Play("playerhit");
        }
    }

    private void takeDamage(){
        isHit = true;
        StartCoroutine(noLongerHit());
        playerHearts --;
        Texture newTexture = Resources.Load<Texture>("heartempty");
        playerHeartUI.GetChild(playerHearts).gameObject.GetComponent<RawImage>().texture = newTexture;
    }

    IEnumerator noLongerHit(){
        yield return new WaitForSeconds(0.3f);
        isHit = false;
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

    void OnTriggerEnter2D (Collider2D other){
        switch (other.gameObject.layer){
            case 4: // Water
                if (!isJumping){
                    GameOver();
                }
                break;
            default:
                break;
        }
    }

    public bool getAliveState(){
        return isAlive;
    }

    public bool getGameState(){
        return isGamePlaying;
    }

}
