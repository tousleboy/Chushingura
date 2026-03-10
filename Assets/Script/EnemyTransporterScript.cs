using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTransporterScript : MonoBehaviour
{
    public GameObject pair;
    EnemyTransporterScript ets;
    public bool active;
    // Start is called before the first frame update
    void Start()
    {
        ets = pair.GetComponent<EnemyTransporterScript>();
        active = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Enemy" && active)
        {
            Rigidbody2D rbody = collision.gameObject.GetComponent<Rigidbody2D>();
            rbody.linearVelocity = new Vector2(0, rbody.linearVelocity.y);
            collision.gameObject.transform.position = pair.transform.position;
            ets.active = false;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            ets.active = true;
        }
    }
}
