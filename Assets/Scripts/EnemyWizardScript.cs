using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWizardScript : MonoBehaviour
{
    public float fireRate;
    public float fireSpeed;
    public Transform firePoint;
    public Transform spell;

    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        StartCoroutine(shootPlayer());
    }

    IEnumerator shootPlayer(){
        while (true){
            yield return new WaitForSeconds(fireRate);
            Fire();
        }
    }

    private void Fire(){
        Vector2 direction = player.transform.position - firePoint.position;
        direction.Normalize();

        Transform spellObj = Instantiate(spell, firePoint.position + (Vector3)(direction * 0.5f), Quaternion.identity);
        Vector3 rotationTarget = player.transform.position - firePoint.position;
        float zRotation = Mathf.Atan2( rotationTarget.y, rotationTarget.x )*Mathf.Rad2Deg;
        spellObj.transform.rotation = Quaternion.Euler(new Vector3 ( 0, 0, zRotation+45f));
        spellObj.gameObject.GetComponent<Rigidbody2D>().velocity = direction * fireSpeed;
    }
}
