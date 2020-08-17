using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySightScript : MonoBehaviour
{
    private Transform target;
    private Camera camera;
    private bool onSight = false;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("Player").transform;
        camera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {   
        Vector3 viewPos = camera.WorldToViewportPoint(target.position);
        if (viewPos.x > 0.5F && viewPos.x > 0.0f){
            transform.localRotation = Quaternion.Euler(0, 0, 0); // facing right
            onSight = true;
        }
        else if (viewPos.x >= 0.5F && viewPos.x < 1.0f){
            transform.localRotation = Quaternion.Euler(0, 180, 0); // facing left
            onSight = true;
        }
        else{
            onSight = false;
        }
        Debug.Log(onSight);
    }

    public bool getSightState(){
        return onSight;
    }
}
