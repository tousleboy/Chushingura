using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    int state = 0;
    string[] states;
    public GameObject[] anchors;
    int nowPos = 0;
    bool wait;
    bool shote = true;
    GameObject player;
    Vector2 playerPos;
    Animator animator;
    PlayerController pc;

    public float idleLength = 1.0f;

    public GameObject[] bullets;
    public GameObject gate;
    public float fireSpeedX = 5.0f;
    public float fireSpeedY = 0f;

    public int life = 3;
    bool dead = false;
    public string sutezerihu = "";

    AudioSource soundPlayer;
    public AudioClip shoot;
    public AudioClip move;
    public AudioClip damage;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        states = new string[] {"idle", "teleport", "attack"};
        animator = GetComponent<Animator>();
        pc = player.GetComponent<PlayerController>();
        Wait();
        Invoke("QuitWait", idleLength);
        soundPlayer = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(dead || PlayerController.gameState != "playing") return;

        playerPos = player.transform.position;
        
        float xPosition = transform.position.x;
        float xPlayerPosition = playerPos.x;
        
        if(xPosition - xPlayerPosition > 0)
        {
            transform.localScale = new Vector2(-1, 1);
        }
        else if(xPosition - xPlayerPosition < 0)
        {
            transform.localScale = new Vector2(1, 1);
        }
        if(life <= 0 && !dead)
        {
            Die();
            return;
        }

        if(pc.falter && state == 2)
        {
            state = 1;
            animator.SetTrigger("teleport");
            if(IsInvoking("QuitWait"))
            {
                CancelInvoke("QuitWait");
            }
        }

        if(wait) return;

        if(shote)
        {
            animator.SetTrigger("attack");
            state = 2;
            shote = false;
            return;
        }

        int r = Random.Range(0, states.Length);
        if(state == r) r = (r + 1) % states.Length;

        state = r;

        if(states[r] == "idle")
        {
            Wait();
            Invoke("QuitWait", idleLength);
        }
        else
        {
            animator.SetTrigger(states[r]);
        }

        


    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Attack")
        {
            AttackManager am = collision.gameObject.GetComponent<AttackManager>();
            life -= am.val;
            soundPlayer.PlayOneShot(damage);
            GetComponent<Renderer>().material.color = Color.red;
            if(life > 0) animator.SetTrigger("teleport");
            state = 1;
            if(IsInvoking("QuitWait"))
            {
                CancelInvoke("QuitWait");
            }
            Invoke("ColorReset", 0.1f);
        }
    }

    IEnumerator SlowMotion()
    {
        Time.timeScale = 0.2f;
        yield return new WaitForSeconds(0.2f);
        Time.timeScale = 1.0f;
    }

    void Wait()
    {
        wait = true;
    }

    void QuitWait()
    {
        wait = false;
    }

    void Teleport()
    {
        int length = anchors.Length;
        int r = Random.Range(0, length);
        if(r == nowPos) r = (r + 1) % length;

        Vector2 Position = anchors[r].transform.position;
        transform.position = Position;

        nowPos = r;
    }

    void Shoot()
    {
        int i = Random.Range(0, 10);
        int length = bullets.Length;
        Vector3 pos = new Vector3(gate.transform.position.x, gate.transform.position.y, gate.transform.position.z);
        GameObject obj = Instantiate(bullets[i % length], pos, Quaternion.identity);
        obj.transform.localScale = new Vector2(obj.transform.localScale.x * transform.localScale.x, obj.transform.localScale.y);
        Rigidbody2D rbody = obj.GetComponent<Rigidbody2D>();
        if(pc.onGround)
        {
            Vector2 v = new Vector2(fireSpeedX * transform.localScale.x, fireSpeedY);
            rbody.AddForce(v, ForceMode2D.Impulse);
        }
        else
        {
            obj.transform.Rotate(0, 0, 45 * transform.localScale.x);
            Vector2 v = new Vector2(fireSpeedX * transform.localScale.x, fireSpeedX);
            rbody.AddForce(v, ForceMode2D.Impulse);
        }
    }

    void ColorReset()
    {
        GetComponent<Renderer>().material.color = Color.white;
    }

    void Die()
    {
        PlayerController.defeats += 1;
        dead = true;
        animator.SetTrigger("die");

        GetComponent<CapsuleCollider2D>().enabled = false;
        StartCoroutine("SlowMotion");
        if(sutezerihu != "") PlayerController.messages = sutezerihu;

        Destroy(gameObject, 1.0f);
    }

    void ShootSound()
    {
        soundPlayer.PlayOneShot(shoot);
    }

    void MoveSound()
    {
        soundPlayer.PlayOneShot(move);
    }
}
