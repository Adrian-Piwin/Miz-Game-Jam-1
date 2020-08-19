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
    public GameMenuScript menuScript;

    private Animator animator;
    private GameObject cameraEnd;
    private bool isGamePlaying = false;
    private float horizontal;
    private float vertical;
    private bool canSwingSword = true;
    private bool isJumping = false;
    private bool isAlive;
    private bool isHit = false;
    private bool isGameWon = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        cameraEnd = GameObject.Find("CameraEnd");
        isAlive = true;
    }

    void Update()
    {
        if (!isGameWon){
            horizontal = Input.GetAxisRaw("Horizontal");
            vertical = Input.GetAxisRaw("Vertical");
        }else{
            if (transform.position.x > cameraEnd.transform.position.x){
                GameObject.Find("GameMenuScript").GetComponent<GameMenuScript>().loadWinScreen();
            }
        }

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

    // Player swinging sword
    IEnumerator swingSword(){
        yield return new WaitForSeconds(swordSwingTime);
        sword.GetComponent<SwordScript>().updateSwingState(false);
        if (swordCooldown > swordSwingTime)
            yield return new WaitForSeconds(swordCooldown - swordSwingTime);
        canSwingSword = true;
    }

    // Player jump
    IEnumerator jump(){
        isJumping = true;
        canSwingSword = false;
        animator.Play("jumpanim");
        yield return new WaitForSeconds(jumpTime);
        isJumping = false;
        canSwingSword = true;
    }

    // Start game
    private void StartGame(){
        isGamePlaying = true;
        animator.SetBool("isMoving", true);
        cameraScript.enableCamera(true);
        levelGenerationScript.startGeneration();
    }

    // Take damage, end game if no health left
    private void GameOver(){
        if (!isHit)
            takeDamage();
        if (playerHearts == 0){
            isGamePlaying = false;
            isAlive = false;
            animator.SetBool("isMoving", false);
            body.bodyType = RigidbodyType2D.Static;
            animator.Play("deathanim");
            cameraScript.enableCamera(false);
            levelGenerationScript.stopGeneration();
            menuScript.toggleMenu(true);
        }else{
            body.AddForce(transform.right * -1 * hitSomethingForce, ForceMode2D.Impulse);
            animator.Play("playerhit");
        }
    }

    // Player finished all levels
    // Let player go off screen
    public void GameWon(){
        isGameWon = true;
        horizontal = 1;
        vertical = 0;
        GameObject.Find("OutOfBounds").SetActive(false);

    }

    // Remove heart from UI
    private void takeDamage(){
        if (isAlive){
            isHit = true;
            StartCoroutine(noLongerHit());
            playerHearts --;
            Texture newTexture = Resources.Load<Texture>("heartempty");
            playerHeartUI.GetChild(playerHearts).gameObject.GetComponent<RawImage>().texture = newTexture;
        }
    }

    // Hit grace period
    IEnumerator noLongerHit(){
        yield return new WaitForSeconds(0.3f);
        isHit = false;
    }

    // Collision with enemies
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

    // Collision for jumpables
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

    // Get Alive state
    public bool getAliveState(){
        return isAlive;
    }

    // Get game state
    public bool getGameState(){
        return isGamePlaying;
    }

}
