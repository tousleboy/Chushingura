using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public float leftLimit = 0.0f;
    public float rightLimit = 0.0f;
    public float topLimit = 0.0f;
    public float bottomLimit = 0.0f;

    public bool locked = false;
    public bool resetPos = false;
    public bool cannotGoBack = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if(player != null)
        {
            float x = player.transform.position.x;
            float y = player.transform.position.y;
            float z = transform.position.z;

            if(resetPos)
            {
                float range = 0.1f;
                float speed = 20f;
                if(x - transform.position.x > range)
                {
                    transform.Translate(speed * Time.deltaTime, 0, 0);
                }
                else if(x - transform.position.x < range * -1)
                {
                    transform.Translate(-1f * speed * Time.deltaTime, 0, 0);
                }
                else
                {
                    locked = false;
                    resetPos = false;
                }
            }
            if(locked)
            {
                return;
            }

            x = System.Math.Max(x, leftLimit);
            x = System.Math.Min(x, rightLimit);
            y = System.Math.Max(y, bottomLimit);
            y = System.Math.Min(y, topLimit);

            Vector3 v3 = new Vector3(x, y, z);
            transform.position = v3;
            if(cannotGoBack) leftLimit = x;
        } 
    }
}

