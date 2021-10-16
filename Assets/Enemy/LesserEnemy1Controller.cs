using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LesserEnemy1Controller : MonoBehaviour
{
    public float range = 8.0f;
    public float speed = 5.0f;
    public string attackType = "slash";
    Vector2 originalPos;
    GameObject player;
    Vector2 playerPos;
    public float maai = 1.5f;
    public bool attacking = false;
    public bool blocking = false;
    public GameObject attackZone;
    AttackManager am;

    Animator animator;

    Rigidbody2D rbody;

    bool dead = false;

    AudioSource soundPlayer;
    public AudioClip slash;
    public AudioClip sting;
    public AudioClip damage;

    public GameObject katami;
    // Start is called before the first frame update
    void Start()
    {
        originalPos = transform.position;
        player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
        rbody = GetComponent<Rigidbody2D>();
        am = attackZone.GetComponent<AttackManager>();
        soundPlayer = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(attacking || dead)
        {
            return;
        }
        float x = transform.position.x;
        playerPos = player.transform.position;
        if((x - originalPos.x) * transform.localScale.x >= range)
        { 
            transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
        }

        float d = (playerPos.x - transform.position.x) * transform.localScale.x;
        if(d > 0 && d <= maai)
        {
            animator.SetTrigger(attackType);
        }
    }

    void FixedUpdate()
    {
        if(!attacking)
        {
            rbody.velocity = new Vector2(transform.localScale.x * speed, rbody.velocity.y);
        }
        else
        {
            rbody.velocity = new Vector2(0, rbody.velocity.y);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Attack")
        {
            AttackManager ame = collision.gameObject.GetComponent<AttackManager>();
            if(ame.state == "slash" && blocking)
            {
                Debug.Log("block success");
            }
            else
            {
                soundPlayer.PlayOneShot(damage);
                Die();
            }
        }
    }

    void Die()
    {
        Vector3 pos = new Vector3(transform.position.x /*+ 0.5f * player.transform.localScale.x*/, transform.position.y + 0.5f, transform.position.z);
        GameObject obj = Instantiate(katami, pos, Quaternion.identity);
        PlayerController.defeats += 1;
        TriggerReset();
        animator.SetTrigger("die");
        GetComponent<CapsuleCollider2D>().enabled = false;
        rbody.simulated = false;
        dead = true;
        Destroy(gameObject, 1.0f);
    }

    void TriggerReset()
    {
        animator.ResetTrigger("slash");
        animator.ResetTrigger("sting");
        animator.ResetTrigger("block");
        animator.ResetTrigger("duck");
    }

    void SetSlash()
    {
        am.state = "slash";
    }
    void SetSting()
    {
        am.state = "sting";
    }


    void SlashSound()
    {
        soundPlayer.PlayOneShot(slash);
    }

    void StingSound()
    {
        soundPlayer.PlayOneShot(sting);
    }


}