using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBar : MonoBehaviour
{
    public Transform playerIcon;

    public void showProgress(int level){
        float newX = 0;

        switch (level){
            case 0:
                newX = -180;
                break;
            case 1:
                newX = 0;
                break;
            case 2:
                newX = 180;
                break;
        }

        playerIcon.localPosition = new Vector3(newX, playerIcon.localPosition.y, playerIcon.localPosition.z);
    }
}
