using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorrodingSoldier : MonoBehaviour {

    public int health;

    public GameObject sparks;

    GameObject targetedPlayer;
    Rigidbody2D rb, rbTarget;
    Animator anim;
    float topSpeed = 4f;
    Vector3 theScale;
    private float range;
    // Use this for initialization
    void Start () {
        targetedPlayer = GameObject.FindGameObjectWithTag("Player");
        rb = gameObject.GetComponent<Rigidbody2D>();
        rbTarget = targetedPlayer.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();
        theScale = transform.localScale;
    }
	
	// Update is called once per frame
	void Update () {
        if (health <= 0)
        {
            Destroy(gameObject);
        }

        anim.SetFloat("xV", Mathf.Abs(rb.velocity.x));
        ChaseTargetedPlayer();
        Debug.Log(rb.velocity.x);
        range = Vector2.Distance(transform.position, targetedPlayer.transform.position);
    }

    void ChaseTargetedPlayer()
    {
        theScale = transform.localScale;
        if (targetedPlayer.transform.position.x < transform.position.x)
        {
            theScale.x = -1f;
            transform.localScale = new Vector3(2* theScale.x,2,1);
        }
        if (targetedPlayer.transform.position.x >= transform.position.x)
        {
            theScale.x = 1f;
            transform.localScale = new Vector3(2 * theScale.x, 2, 1);
        }

        if(range <= 1.5f)
        {
            anim.SetBool("chase", false);
            topSpeed = 0f;
        }
        else
        {
            anim.SetBool("chase", true);
            topSpeed = 4f;
        }
        rb.velocity = new Vector2(topSpeed * theScale.x, rb.velocity.y);
    }

    public void TakeDamage(int damage)
    {
        Instantiate(sparks, transform.position, Quaternion.identity);
        health -= damage;
        Debug.Log("ouch");
    }
}
