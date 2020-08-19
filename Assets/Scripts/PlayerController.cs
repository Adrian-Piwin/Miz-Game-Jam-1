using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Settings")]
    public float speed = 10.0f;
    public float driftCooldown = 1f;
    public float driftTime = 0.3f;
    public float jumpTime = 0.5f;
    public float hitSomethingForce = 100f;
    public int playerHearts = 3;

    [Header("References")]
    public Rigidbody2D body;
    public GameObject coolDownBar;
    public CameraMovement cameraScript;
    public LevelGenerationScript levelGenerationScript;
    public Transform playerHeartUI;
    public GameMenuScript menuScript;
    public GameObject destroyParticle;
    public ParticleSystem driftSparks;

    private Animator animator;
    private GameObject cameraEnd;
    private bool isGamePlaying = false;
    private float horizontal;
    private float vertical;
    private bool canDrift = true;
    private bool isDrifting = false;
    private bool isJumping = false;
    private bool isAlive;
    private bool isHit = false;
    private bool isGameWon = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        cameraEnd = GameObject.Find("CameraEnd");
        isAlive = true;
        driftSparks.Stop();
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
        
        // Drift
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDrift && isGamePlaying && !isJumping){
            canDrift = false;
            isDrifting = true;
            coolDownBar.GetComponent<CooldownScript>().enableCooldown(driftCooldown);
            animator.SetBool("isMoving", false);
            animator.SetBool("isDrifting", true);
            driftSparks.Play();
            StartCoroutine(startDrift());
        }
        
        // Stop drift if release shift
        if (Input.GetKeyUp(KeyCode.LeftShift) && isDrifting){
            stopDrifting();
        }

        // Jump
        if (Input.GetKeyDown("space") && isGamePlaying && !isDrifting){
            StartCoroutine(jump());
        }
    }

    void FixedUpdate() {
        if (isGamePlaying && !isHit){
            if (!isDrifting){
                float spd;
                if (horizontal == -1)
                    spd = speed * 1.8f;
                else spd = speed;

                Vector2 movement = new Vector2(horizontal, vertical);
                movement = movement.normalized * Time.deltaTime * spd;
                body.velocity = movement;
            }else{
                Vector2 movement = new Vector2(1, 0);
                movement = movement.normalized * Time.deltaTime * (speed * 1.5f);
                body.velocity = movement;
            }
        }
    }

    // Player swinging sword
    IEnumerator startDrift(){
        yield return new WaitForSeconds(driftTime);
        stopDrifting();
        if (driftCooldown > driftTime)
            yield return new WaitForSeconds(driftCooldown - driftTime);
        canDrift = true;
    }

    private void stopDrifting(){
        isDrifting = false;
        animator.SetBool("isDrifting", false);
        animator.SetBool("isMoving", true);
        driftSparks.Stop();
    }

    // Player jump
    IEnumerator jump(){
        isJumping = true;
        canDrift = false;
        animator.Play("jumpanim");
        yield return new WaitForSeconds(jumpTime);
        isJumping = false;
        canDrift = true;
    }

    // Start game
    private void StartGame(){
        isGamePlaying = true;
        animator.SetBool("isMoving", true);
        cameraScript.enableCamera(true);
        levelGenerationScript.startGeneration();
    }

    // Take damage, end game if no health left
    private void GameOver(bool isDestroyable){
        if ((!isHit && !isDestroyable) || (!isHit && isDestroyable && !isDrifting))
            takeDamage(isDestroyable);
            

        if (playerHearts == 0){
            isGamePlaying = false;
            isAlive = false;
            animator.SetBool("isMoving", false);
            body.bodyType = RigidbodyType2D.Static;
            animator.Play("deathanim");
            cameraScript.enableCamera(false);
            levelGenerationScript.stopGeneration();
            menuScript.toggleMenu(true);
        }
    }

    // Remove heart from UI
    private void takeDamage(bool isDestroyable){
        if (isAlive){
            isHit = true;
            cameraScript.enableCamera(false);
            StartCoroutine(noLongerHit());
            playerHearts --;
            Texture newTexture = Resources.Load<Texture>("heartempty");
            playerHeartUI.GetChild(playerHearts).gameObject.GetComponent<RawImage>().texture = newTexture;

            stopDrifting();
            if (!isDestroyable)
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

    // Hit grace period
    IEnumerator noLongerHit(){
        yield return new WaitForSeconds(0.3f);
        isHit = false;
        if (isGamePlaying)
            cameraScript.enableCamera(true);
    }

    // Collision with enemies
    void OnCollisionEnter2D (Collision2D other){
        switch (other.gameObject.layer){
            case 8: // Enemy   
                GameOver(true);
                if (other.gameObject.tag != "Projectile"){
                    Destroy(other.gameObject);
                    Instantiate(destroyParticle, other.transform.position, Quaternion.identity);
                }
                break;
            case 9: // Walls
                GameOver(false);
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
                    GameOver(false);
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
