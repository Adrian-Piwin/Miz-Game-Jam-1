using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CooldownScript : MonoBehaviour
{
    private SpriteRenderer rend1;
    private SpriteRenderer rend2;


    // Start is called before the first frame update
    void Start()
    {
        rend1 = GetComponent<SpriteRenderer>();
        rend2 = transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
        StartCoroutine(toggleRenderer(false,0f));
    }

    public void enableCooldown (float time){
        StartCoroutine(toggleRenderer(true,0f));
        StartCoroutine(scaleLerp(transform.GetChild(0).transform.localScale, new Vector3(0.975f,transform.GetChild(0).transform.localScale.y,transform.GetChild(0).transform.localScale.z), time));
        StartCoroutine(toggleRenderer(false,time));
    }

    private IEnumerator scaleLerp(Vector3 a, Vector3 b, float time){
        float i = 0.0f;
        float rate = (1.0f / time) * 1;
        while (i < 1.0f){
            i += Time.deltaTime * rate;
            transform.GetChild(0).transform.localScale = Vector3.Lerp(a, b ,i);
            yield return null;
        }
    }

    private IEnumerator toggleRenderer(bool toggle, float time){
        yield return new WaitForSeconds(time);
        rend1.enabled = toggle;
        rend2.enabled = toggle;

        if (!toggle)
            transform.GetChild(0).transform.localScale = new Vector3(0f,transform.GetChild(0).transform.localScale.y,transform.GetChild(0).transform.localScale.z);
    }
}
