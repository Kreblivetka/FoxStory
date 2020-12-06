using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{   
    //[SerializeField]//to see it in unity
    //Start() variables
    private Rigidbody2D rb;
    private Animator anim;
    private Collider2D coll;

    
    //FSM
    private enum State {idle, running, jumping, falling, hurt }
    private State state = State.idle;

    //Inspector variables
    [SerializeField] private LayerMask ground;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpforce = 10f;
    [SerializeField] private int cherries = 0;
    [SerializeField] private Text CherrieText;
    [SerializeField] private float HurtForce = 10f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
    }
    
    private void Update()
    {   
        if(state != State.hurt)
        {   
            Movement();
        }
       
        AnimationState();
        anim.SetInteger("state", (int)state);//sets animation bored on enum
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Collactable") 
        {
            Destroy(collision.gameObject);
            cherries += 1;
            CherrieText.text = cherries.ToString();
        }   
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            if (state == State.falling)
            {
                //Destroy(other.gameObject);
                enemy.JumpedOn();
                Jump();
            }
            else 
            {
                state = State.hurt;
                if (other.gameObject.transform.position.x > transform.position.x)
                {
                    //enemy to my right, damaged move to the right
                    rb.velocity = new Vector2(-HurtForce, rb.velocity.y);
                }
                else 
                {
                    //emeny is to my left, damaged move to left
                    rb.velocity = new Vector2(HurtForce, rb.velocity.y);
                }
            }
        }
    }

    private void Movement()
    {
        float hDirections = Input.GetAxis("Horizontal");
        //run left
        if (hDirections < 0)
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
            transform.localScale = new Vector2(-1, 1);

        }
        //run right
        else if (hDirections > 0)
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
            transform.localScale = new Vector2(1, 1);

        }
        //jump
        if (Input.GetButtonDown("Jump") && coll.IsTouchingLayers(ground))
        {
            Jump();
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpforce);
            state = State.jumping;
    }
    private void AnimationState()
    {
        if (state == State.jumping)
        {
            if (rb.velocity.y < 1f)
            {
                state = State.falling;
            }
        }
        else if (state == State.falling)
        {
            if (coll.IsTouchingLayers(ground))
            {
                state = State.idle;
            }
        }
        else if (state == State.hurt)
        {
            if (Mathf.Abs(rb.velocity.x) < 1f)
            {
                state = State.idle;
            }
        }
        else if (Mathf.Abs(rb.velocity.x) > 2f)
        {
            state = State.running;
        }
        else
        {
            state = State.idle;
        }
    }


    
}
