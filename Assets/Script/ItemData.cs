using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData : MonoBehaviour
{
    public int val = 0;
    public float delay = 10f;
    public string type = "heal";
    // Start is called before the first frame update
    void Start()
    {
        if(delay > 0)
        {
            GetComponent<CircleCollider2D>().enabled = false;
            Invoke("Active", delay);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Active()
    {
        GetComponent<CircleCollider2D>().enabled = true;
    }
}
