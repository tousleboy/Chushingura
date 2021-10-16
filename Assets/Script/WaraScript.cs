using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaraScript : MonoBehaviour
{
    public string type = "slash";
    AudioSource soundPlayer;
    public AudioClip damaged;
    // Start is called before the first frame update
    void Start()
    {
        soundPlayer = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Attack")
        {
            if(type == collision.gameObject.GetComponent<AttackManager>().state)
            {
                soundPlayer.PlayOneShot(damaged);
                GetComponent<Renderer>().material.color = Color.red;
                Invoke("ColorReset", 0.1f);
                StartCoroutine("Die");
            }
        }
    }

    void ColorReset()
    {
        GetComponent<Renderer>().material.color = Color.white;
    }

    IEnumerator Die()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        float speed = -200f;
        Quaternion now = transform.rotation;
        while(Quaternion.Angle(now, transform.rotation) < 90)
        {
            transform.Rotate(new Vector3(0, 0, speed * Time.deltaTime));
            yield return null;
        }
    }
}
