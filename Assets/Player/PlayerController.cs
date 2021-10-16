using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float runSpeed = 5.0f;
    float axisH;

    Rigidbody2D rbody;

    public bool onGround;
    public LayerMask groundLayer;

    Animator animator;

    public bool attacking = false;
    public bool falter = false;
    public string attackState;
    string attackTrigger;
    bool goAttack = false;
    public GameObject attackZone;
    AttackManager am;

    public bool blocking = false;
    public bool ducking = false;
    bool goJump = false;
    bool jumpStop = false;
    public float jumpPw = 10.0f;

    public static string gameState;

    public static int life;
    public static int maxLife = 6;
    public static string messages = "";

    public bool damaged = false;

    public static int money = 0;
    public static int defeats = 0;

    AudioSource soundPlayer;
    public AudioClip slash;
    public AudioClip sting;
    public AudioClip block;
    public AudioClip damage;
    public AudioClip jarajara;
    public AudioClip eat;

    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        am = attackZone.GetComponent<AttackManager>();

        life = maxLife;

        money = 0;
        defeats = 0;

        soundPlayer = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(gameState);
        if(gameState != "playing")
        {

            return;
        }

        axisH = Input.GetAxisRaw("Horizontal");
        float axisV = Input.GetAxisRaw("Vertical");

        if(axisH > 0.0f)
        {
            transform.localScale = new Vector2(1, 1);
        }
        else if(axisH < 0.0f)
        {
            transform.localScale = new Vector2(-1, 1);
        }

        if(Input.GetKeyDown("j") && !falter)
        {
            attackTrigger = "slash";
            //am.state = "slash";
            goAttack = true;
        }
        if(Input.GetKeyDown("k") && !falter)
        {
            attackTrigger = "sting";
            //am.state = "sting";
            goAttack = true;
        }

        if(Input.GetButtonDown("Jump"))
        {
            Jump();
        }
        if(Input.GetButtonUp("Jump") && !onGround && rbody.velocity.y > 0)
        {
            jumpStop = true;
        }

        if(axisV > 0 && !attacking)
        {
            blocking = true;

        }
        else
        {
            blocking = false;
        }
        if(axisV < 0 && !attacking)
        {
            ducking = true;
        }
        else
        {
            ducking = false;
        }

        if(life <= 0)
        {
            Die();
        }
    }

    void FixedUpdate()
    {
        /*if(gameState != "playing")
        {
            return;
        }*/

        onGround = Physics2D.Linecast(transform.position, transform.position -(transform.up * 0.1f), groundLayer);

        if(attacking || blocking || ducking) axisH = 0.0f;

        if(onGround || axisH != 0)//走り
        {
            rbody.velocity = new Vector2(axisH * runSpeed, rbody.velocity.y);
        }

        if(goJump && onGround)
        {
            Vector2 jump = new Vector2(0, jumpPw);
            rbody.AddForce(jump, ForceMode2D.Impulse);
            goJump = false;
        }

        if(jumpStop)
        {
            //rbody.velocity = new Vector2(rbody.velocity.x, 0.0f);
            StartCoroutine("JumpStop");
            jumpStop = false;
        }

        if(!attacking && !falter)
        {
            animator.SetBool("onground", onGround);
            if(onGround)
            {
                animator.SetBool("duck", ducking);
                animator.SetBool("block", blocking);
                if(goAttack)
                {
                    animator.SetTrigger(attackTrigger);
                    goAttack = false;
                }
                else if(axisH == 0)
                {
                    animator.SetBool("move", false);
                }
                else
                {
                    animator.SetBool("move", true);
                }
            }
        }

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Attack" && !damaged)
        {
            AttackManager ame = collision.gameObject.GetComponent<AttackManager>();
            if(ame.state == "slash" && blocking)
            {
                Debug.Log("block success");
                soundPlayer.PlayOneShot(block);
            }
            else
            {
                life -= ame.val;
                soundPlayer.PlayOneShot(damage);
                StartCoroutine("Damaged");
            }
        }
        if(collision.gameObject.tag == "Item")
        {
            ItemData id = collision.gameObject.GetComponent<ItemData>();
            if(id.type == "heal")
            {
                soundPlayer.PlayOneShot(eat);
                life = Mathf.Min(life + id.val, maxLife);
            }
            if(id.type == "money")
            {
                soundPlayer.PlayOneShot(jarajara);
                money += id.val;
            }
            Destroy(collision.gameObject);
        }
        if(collision.gameObject.tag == "Dead")
        {
            life = 0;
        }
        if(collision.gameObject.tag == "Goal")
        {
            gameState = "gameclear";
            axisH = 1.0f;
        }
    }

    IEnumerator JumpStop()
    {
        float speed = 60.0f;
        while(rbody.velocity.y >= 0)
        {
            rbody.velocity = new Vector2(rbody.velocity.x, rbody.velocity.y - speed * Time.deltaTime);
            yield return null;
        }
    }

    IEnumerator Damaged()
    {
        int i;
        int n = 6;
        float t;
        float speed = 5.0f;
        int genten = 1;
        Renderer R = GetComponent<Renderer>();
        damaged = true;
        animator.SetTrigger("damaged");
        GameManager.battleScore -= genten;
        for(i = 0; i < n; i++)
        {
            for(t = 0; t <= 1; t += speed * Time.deltaTime)
            {
                R.material.color = Color.Lerp(Color.white, Color.clear, t);
                yield return null;
            }
            for(t = 0; t <= 1; t += speed * Time.deltaTime)
            {
                R.material.color = Color.Lerp(Color.clear, Color.white, t);
                yield return null;
            }
        }
        damaged = false;
    }

    void Die()
    {
        int genten = 4;
        GameManager.battleScore -= genten;
        animator.SetTrigger("die");
        GetComponent<CapsuleCollider2D>().enabled = false;
        rbody.simulated = false;
        gameState = "gameover";
    }

    void Jump()
    {
        goJump = true;
    }

    void SetSlash()
    {
        am.state = "slash";
        attackState = "slash";
    }
    void SetSting()
    {
        am.state = "sting";
        attackState = "sting";
    }

    void SlashSound()
    {
        soundPlayer.PlayOneShot(slash);
    }

    void StingSound()
    {
        soundPlayer.PlayOneShot(sting);
    }

    void Falter()
    {
        falter = true;
    }

    void QuitFalter()
    {
        falter = false;
    }
}
