using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public bool isMoving = false;
    public float speed = 0.5f;

    // Update is called once per frame
    void Update()
    {
        if (isMoving){
            transform.position = transform.position + new Vector3(speed * Time.deltaTime,0,0);
        }
    }

    public void enableCamera(bool toggle){
        isMoving = toggle;
    }
}
