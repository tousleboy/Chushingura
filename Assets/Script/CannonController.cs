using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonController : MonoBehaviour
{
    public GameObject objPrefab;
    public float delayTime = 3.0f;
    public float fireSpeedX = -4.0f;
    public float fireSpeedY = 0.0f;
    public float length = 8.0f;
    public int life = 3;
    bool stop = false;

    GameObject player;
    GameObject gateObj;
    float passedTimes = 0;

    AudioSource soundPlayer;
    public AudioClip dokahn;
    public AudioClip damage;

    // Start is called before the first frame update
    void Start()
    {
        Transform tr = transform.Find("gate");
        gateObj = tr.gameObject;
        player = GameObject.FindGameObjectWithTag("Player");
        soundPlayer = GetComponent<AudioSource>(); 
    }

    // Update is called once per frame
    void Update()
    {
        if(stop)
        {
            return;
        }

        passedTimes += Time.deltaTime;
        if(CheckLength(player.transform.position))
        {
            if(passedTimes > delayTime)
            {
            passedTimes = 0;
            soundPlayer.PlayOneShot(dokahn);
            Vector3 pos = new Vector3(gateObj.transform.position.x, gateObj.transform.position.y, gateObj.transform.position.z);
            GameObject obj = Instantiate(objPrefab, pos, Quaternion.identity);
            Rigidbody2D rbody = obj.GetComponent<Rigidbody2D>();
            Vector2 v = new Vector2(fireSpeedX, fireSpeedY);
            rbody.AddForce(v, ForceMode2D.Impulse);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Attack")
        {
            life -= collision.gameObject.GetComponent<AttackManager>().val;
            soundPlayer.PlayOneShot(damage);
            GetComponent<Renderer>().material.color = Color.red;
            Invoke("ColorReset", 0.1f);
        }
        if(life <= 0)
        {
            StartCoroutine("Die");
        }
    }

    bool CheckLength(Vector2 targetPos)
    {
        bool ret = false;
        float d = Vector2.Distance(transform.position,targetPos);
        if(length >= d)
        {
            ret = true;
        }
        return ret;
    }

    void ColorReset()
    {
        GetComponent<Renderer>().material.color = Color.white;
    }

    IEnumerator Die()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        stop = true;
        float speed = -200f;
        Quaternion now = transform.rotation;
        while(Quaternion.Angle(now, transform.rotation) < 90)
        {
            transform.Rotate(new Vector3(0, 0, speed * Time.deltaTime));
            yield return null;
        }
    }
}

