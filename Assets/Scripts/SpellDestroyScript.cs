using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellDestroyScript : MonoBehaviour
{
    public GameObject destroyParticle;
    public int bounceMax;
    public bool isReflectable;
    
    private bool canKillEnemy = false;
    private PlayerController playerController;
    private bool isBossSpell;

    void Start(){
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();

        StartCoroutine(destroyTimer(7f));
    }
    
    void OnCollisionEnter2D (Collision2D other){
        switch(other.gameObject.layer){
            case 0:
            case 9: // Walls
                if (bounceMax == 0){ 
                    Instantiate(destroyParticle, transform.position, Quaternion.identity);
                    Destroy(gameObject);
                }else bounceMax --;
                break;
            
            case 11: // Player
                if (!playerController.getDriftState()){ // Destroy and hit player
                    SoundManagerScript.PlaySound("enemyhit");
                    destroySpell();
                    playerController.playerHit(true);

                }else if(!isReflectable){ // Destroy without hitting player
                    destroySpell();

                }else{ // After reflecting, allowing damage to enemies
                    SoundManagerScript.PlaySound("reflection");
                    canKillEnemy = true;
                    if (isBossSpell) toggleCollision(GameObject.Find("SkullBoss"), false);
                }
                break;

            case 12: // Deal damage to skull boss
                if (canKillEnemy){
                    other.transform.GetChild(0).gameObject.GetComponent<SkullBossScript>().takeDamage();
                    destroySpell();
                }

                break;
        }
    }

    public void destroySpell(){
        Instantiate(destroyParticle, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    IEnumerator destroyTimer(float timer){
        yield return new WaitForSeconds(timer);
        destroySpell();
    }

    public bool getCanKillState(){
        return canKillEnemy;
    }

    public void updateBossState(bool isBoss){
        isBossSpell = isBoss;
    }

    public void toggleCollision(GameObject boss, bool toggle){
        Physics2D.IgnoreCollision(boss.GetComponent<Collider2D>(), GetComponent<Collider2D>(), toggle);
    }
}
