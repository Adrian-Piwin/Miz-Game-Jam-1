using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMageScript : MonoBehaviour
{
    public float fireRate;
    public float fireSpeed;
    public Transform firePoint;
    public Transform spell;

    public Transform topTarget;
    public Transform botTarget;

    private GameObject player;
    private EnemySightScript enemySightScript;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        enemySightScript = GetComponent<EnemySightScript>();
        StartCoroutine(shootPlayer());
    }

    IEnumerator shootPlayer(){
        while (true){
            yield return new WaitForSeconds(fireRate);
            if (enemySightScript.getSightState() && player.GetComponent<PlayerController>().getAliveState()){
                Fire();
            }
        }
    }

    private void Fire(){
        Vector2 direction;
        if (Random.Range(0,2) == 0){
            direction = topTarget.position - firePoint.position;
        }else{
            direction = botTarget.position - firePoint.position;
        }

        direction.Normalize();

        Transform spellObj = Instantiate(spell, firePoint.position, Quaternion.identity);
        spellObj.gameObject.GetComponent<Rigidbody2D>().velocity = direction * fireSpeed;
    }
}
