using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullBossScript : MonoBehaviour
{   
    [Header("Health")]
    public float health = 10f;

    [Header("Phase One")]
    public float moveSpeed;
    public float stopTimeMin;
    public float stopTimeMax;
    public float fireRate;
    public float fireSpeed;
    [Header("Phase Two")]

    public float moveSpeedPhaseTwo;
    public float stopTimeMinPhaseTwo;
    public float stopTimeMaxPhaseTwo;
    public float fireRatePhaseTwo;
    public float fireSpeedPhaseTwo;

    [Header("References")]
    public Transform firePoint;
    public Transform spell;
    public Rigidbody2D rb;
    public GameObject deathParticle;

    private Animator animator;
    private GameObject player;
    private EnemySightScript enemySightScript;
    private Transform bodyTransform;
    private bool lastDirection = false;
    private float maxHealth;
    private new GameObject camera;
    private IEnumerator shootTimer;
    private bool phaseTwo = false;

    // Start is called before the first frame update
    void Start()
    {   
        maxHealth = health;
        bodyTransform = transform.parent.transform;
        animator = GetComponent<Animator>();
        animator.SetBool("hasEntered", true);
        player = GameObject.Find("Player");
        enemySightScript = GetComponent<EnemySightScript>();
        camera = GameObject.Find("Main Camera");
        shootTimer = shootPlayer();
        StartCoroutine(shootTimer);
    }

    IEnumerator shootPlayer(){
        while (health != 0){
            yield return new WaitForSeconds(fireRate);
            if (enemySightScript.getSightState() && player.GetComponent<PlayerController>().getAliveState()){
                animator.Play("skullshoot");
                yield return new WaitForSeconds(0.1f);
                Fire();
                yield return new WaitForSeconds(0.2f);
                move();
            }
        }
    }

    private void Fire(){
        Vector2 direction = player.transform.position - firePoint.position;
        direction.Normalize();

        Transform spellObj = Instantiate(spell, firePoint.position + (Vector3)(direction * 0.5f), Quaternion.identity);
        spellObj.parent = camera.transform;
        spellObj.gameObject.GetComponent<Rigidbody2D>().velocity = direction * fireSpeed;

        spellObj.GetComponent<SpellDestroyScript>().updateBossState(true);
        spellObj.GetComponent<SpellDestroyScript>().toggleCollision(gameObject ,true);
    }

    private void move(){
        Vector2 direction;

        if (!phaseTwo){
            if (lastDirection){
                direction = bodyTransform.position - new Vector3(bodyTransform.position.x, -7, bodyTransform.position.z);
            }else{
                direction = bodyTransform.position - new Vector3(bodyTransform.position.x, 7, bodyTransform.position.z);
            }
            lastDirection = !lastDirection;
        }else{
            direction = player.transform.position - bodyTransform.position;
        }
        
        direction.Normalize();
        rb.velocity = direction * moveSpeed;

        if (!phaseTwo)
            StartCoroutine(stopMoving(Random.Range(stopTimeMin, stopTimeMax)));
        else 
            StartCoroutine(stopMoving(Random.Range(stopTimeMinPhaseTwo, stopTimeMaxPhaseTwo)));
    }

    // Stop moving
    IEnumerator stopMoving(float time){
        yield return new WaitForSeconds(time);
        rb.velocity = Vector2.zero;
    }

    // Take damage
    public void takeDamage(){
        SoundManagerScript.PlaySound("bosshit");
        animator.Play("skulldamage");
        health--;

        if (health == maxHealth/2){ // Change phase
            SoundManagerScript.PlaySound("bossphase");
            phaseTwo = true;

            moveSpeed = moveSpeedPhaseTwo;
            stopTimeMin = stopTimeMinPhaseTwo;
            stopTimeMax = stopTimeMaxPhaseTwo;
            fireRate = fireRatePhaseTwo;
            fireSpeed = fireSpeedPhaseTwo;

            StopCoroutine(shootTimer);
            animator.Play("skullPhaseChange");
            StartCoroutine(phaseTransition());
        }

        if (health == 0){ // Die
            SoundManagerScript.PlaySound("bossphase");
            StopCoroutine(shootTimer);
            animator.SetBool("isDying", true);
            StartCoroutine(destroyBoss());
        }
    }

    IEnumerator phaseTransition(){
        yield return new WaitForSeconds(1f);
        StartCoroutine(shootTimer);
    }

    IEnumerator destroyBoss(){
        yield return new WaitForSeconds(1.2f);
        GameObject.Find("LevelGeneration").GetComponent<LevelGenerationScript>().bossDefeated();
        Instantiate(deathParticle, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    // Stop velocity when hit
    void OnCollisionEnter2D(Collision2D other){
        rb.velocity = Vector2.zero;
    }
}
