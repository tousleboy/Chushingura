using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    Rigidbody2D rbody;
    public LayerMask groundLayer;

    public float range = 8.0f;
    public float maai = 1.5f;
    public float speed = 6.0f;
    public float knockBackPw = 5.0f;
    bool onGround;
    bool lockOn = false;
    bool moving = false;
    bool dead = false;
    bool knockBack = false;

    public bool attacking = false;
    public bool blocking = false;
    public bool damaged = false;
    public bool daramaticWhenDie = true;
    public string sutezerihu = "";
    bool goAttack = false;
    public GameObject attackZone;
    AttackManager am;
    public int slashP = 30;
    public int stingP = 30;
    public int blockP = 15;
    public int duckP = 15;

    Animator animator;

    GameObject player;
    PlayerController pc;
    Vector2 playerPos;
    bool isPlayerNear = false;

    public int life = 3;

    AudioSource soundPlayer;
    public AudioClip slash;
    public AudioClip sting;
    public AudioClip block;
    public AudioClip damage;
    
    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();

        am = attackZone.GetComponent<AttackManager>();
        animator = GetComponent<Animator>();

        player = GameObject.FindGameObjectWithTag("Player");
        pc = player.GetComponent<PlayerController>();
        playerPos = player.transform.position;
        soundPlayer = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(life <= 0 && !dead)
        {
            Die();
        }
        if(dead || damaged)
        {
            return;
        }
        if(pc.onGround)
        {
            playerPos = player.transform.position;
        }
        if(!lockOn && CheckLength(playerPos, range))
        {
            lockOn = true;
        }
        
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
    }

    void FixedUpdate()
    {
        if(knockBack)
        {
            rbody.velocity = new Vector2(0, rbody.velocity.y);
            Vector2 force = new Vector2(knockBackPw * player.transform.localScale.x, 0);
            rbody.AddForce(force, ForceMode2D.Impulse);
            knockBack = false;
        }
        if(dead || damaged)
        {
            return;
        }
        onGround = Physics2D.Linecast(transform.position, transform.position -(transform.up * 0.1f), groundLayer);
        isPlayerNear = CheckLength(playerPos, maai);

        //animator.SetBool("jump", !onGround);

        /*if(damaged || dead || backStepping)
        {
            return;
        }*/

        if(onGround) rbody.velocity = new Vector2(0.0f, rbody.velocity.y);

        if(PlayerController.gameState != "playing")
        {
            moving = false;
            animator.SetBool("move", false);
            return;
        }

        if(goAttack)
        {
            goAttack = false;
            Combo();
            attacking = true;
        }
        else if(lockOn && onGround && !attacking)
        {
            if(isPlayerNear)
            {
                rbody.velocity = new Vector2(0.0f, rbody.velocity.y);
                goAttack = true;
                moving = false;
                animator.SetBool("move", false);
            }
            else
            {
                rbody.velocity = new Vector2(speed * transform.localScale.x, rbody.velocity.y);
                moving = true;
                animator.SetBool("move", true);
            }
        }

        if(moving && pc.attacking && !damaged)
        {
            if(pc.attackState == "slash")
            {
                animator.SetTrigger("block");
                attacking = true;
                moving = false;
                animator.SetBool("move", false);
            }
            if(pc.attackState == "sting")
            {
                animator.SetTrigger("duck");
                attacking = true;
                moving = false;
                animator.SetBool("move", false);
            }
            animator.ResetTrigger("exit");
            
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Attack")
        {
            AttackManager amp = collision.gameObject.GetComponent<AttackManager>();
            if(amp.state == "slash" && blocking)
            {
                Debug.Log("block success");
                soundPlayer.PlayOneShot(block);
            }
            else
            {
                life -= amp.val;
                soundPlayer.PlayOneShot(damage);
                Damaged();
            }
        }
    }

    IEnumerator SlowMotion()
    {
        Time.timeScale = 0.2f;
        yield return new WaitForSeconds(0.2f);
        Time.timeScale = 1.0f;
    }

    bool CheckLength(Vector2 targetPos, float length)
    {
        float d = Vector2.Distance(transform.position, targetPos);
        return d < length && d != 0;
    }

    void Damaged()
    {
        animator.SetTrigger("damaged");
        attacking = false;
        knockBack = true;

    }

    void Die()
    {
        PlayerController.defeats += 1;
        animator.SetTrigger("die");
        rbody.simulated = false;
        GetComponent<CapsuleCollider2D>().enabled = false;
        dead = true;
        if(daramaticWhenDie) StartCoroutine("SlowMotion");
        if(sutezerihu != "") PlayerController.messages = sutezerihu;

        Destroy(gameObject, 1.0f);
    }

    void Combo()
    {
        int n = Random.Range(1, 5);
        if(!isPlayerNear && (n % 2 == 0))
        {
            animator.SetTrigger("exit");
            attacking = false;
            return;
        }
        
        if(n % 4 == 0) animator.SetTrigger("slash");
        else if(n % 4 == 1) animator.SetTrigger("sting");
        else if(n % 4 == 2) animator.SetTrigger("block");
        else if(n % 4 == 3) animator.SetTrigger("duck");
        /*else
        {
            animator.SetTrigger("exit");
            attacking = false;
            
        }*/
    }

    void ComboInterrupt()
    {
        int n = Random.Range(1, 5);
        if(n % 4 == 0) animator.SetTrigger("slash");
        else if(n % 4 == 1) animator.SetTrigger("sting");
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
