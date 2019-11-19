using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    float move;
    Rigidbody2D rb;
    Animator anim;
    bool facingRight = true;
    bool grounded = true;


    // Use this for initialization
    void Start () {
        rb = gameObject.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
        move = Input.GetAxis("Horizontal");
        anim.SetFloat("xvelocity", Mathf.Abs(Input.GetAxis("Horizontal")));
        anim.SetFloat("yvelocity", rb.velocity.y);
        rb.velocity = new Vector2(move * 8, rb.velocity.y);
        anim.SetBool("grounded", grounded);
        if (move < 0 && facingRight == true)
        {
            Flip();
        }

        else if (move > 0 && facingRight == false)
        {
            Flip();
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (grounded == true)
            {
                anim.SetTrigger("jump");
                grounded = false;
                rb.velocity = new Vector2(move * 8, 8);
            }
        }
        if (rb.velocity.y == 0)
        {
            grounded = true;
        }
        else
        {
            grounded = false;
            if (rb.velocity.y < 0)
            {
                anim.SetTrigger("fall");
                anim.ResetTrigger("fallkick");
            }
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

}
