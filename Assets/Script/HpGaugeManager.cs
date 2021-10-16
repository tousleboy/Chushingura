using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpGaugeManager : MonoBehaviour
{
    GameObject green;
    GameObject red;
    float max;
    float prevRate;
    float nowRate;
    // Start is called before the first frame update
    void Start()
    {
        green = transform.Find("Green").gameObject;
        red = transform.Find("Red").gameObject;
        max = (float)PlayerController.maxLife;
        prevRate = 1;
        nowRate = 1;
    }

    // Update is called once per frame
    void Update()
    {
        nowRate = (float)PlayerController.life / max;
        if(nowRate != prevRate)
        {
            StartCoroutine("Reduce");
            //Debug.Log(nowRate);
            prevRate = nowRate;
        }
    }

    IEnumerator Reduce()
    {
        float interval = 0.2f;
        green.transform.localScale = new Vector3(nowRate, 1, 1);
        //Debug.Log("green");
        yield return new WaitForSeconds(interval);
        red.transform.localScale = new Vector3(nowRate, 1 , 1);
        //Debug.Log("red");
    }
}
