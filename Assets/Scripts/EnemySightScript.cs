using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySightScript : MonoBehaviour
{
    private new Camera camera;
    private bool onSight = false;

    // Start is called before the first frame update
    void Start()
    {
        camera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {   
        Vector3 viewPos = camera.WorldToViewportPoint(transform.position);
        if (viewPos.x > 0.5F && viewPos.x < 1.0f){
            onSight = true;
        }
        else if (viewPos.x <= 0.5F && viewPos.x > 0.0f){
            onSight = true;
        }
        else{
            onSight = false;
        }
    }

    public bool getSightState(){
        return onSight;
    }
}
