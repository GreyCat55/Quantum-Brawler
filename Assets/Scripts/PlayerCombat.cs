﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    private float timeBtwAttack;
    public float startTimeBtwAttack;

    private Animator anim;

    Rigidbody2D rb;

    public GameObject sparks;

    public Transform attackPos;
    public LayerMask whatIsEnemies;
    public float attackRange;
    public int damage;

    public Vector2 blink;

    // Use this for initialization
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(Mathf.Abs(anim.GetFloat("xvelocity")));
        if (timeBtwAttack <= 0)
        {
            if (Input.GetButton("Fire1") && anim.GetBool("grounded") == true)
            {

                if ((Mathf.Abs(anim.GetFloat("xvelocity")) > 2f))
                {
                    anim.SetTrigger("elbowcharge");
                    rb.velocity = new Vector2(rb.velocity.x * 6f, 2f);
                    Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsEnemies);
                    //Collider2D[] enemiesToDamage = Physics2D.OverlapBoxAll(attackPos.position, ,0, whatIsEnemies);
                    //Physics2D.OverlapBox();
                    Instantiate(sparks, transform.position, Quaternion.identity);
                    for (int i = 0; i < enemiesToDamage.Length; i++)
                    {
                        enemiesToDamage[i].GetComponent<CorrodingSoldier>().TakeDamage(damage + 15);
                    }

                }
                else
                {
                    anim.SetTrigger("punch1");
                    Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsEnemies);
                    for (int i = 0; i < enemiesToDamage.Length; i++)
                    {
                        enemiesToDamage[i].GetComponent<CorrodingSoldier>().TakeDamage(damage);
                    }
                }
                timeBtwAttack = startTimeBtwAttack;
            }
        }

        else
        {
            timeBtwAttack -= Time.deltaTime;
        }

        if (Input.GetButton("Fire2") && anim.GetBool("grounded") == false)
        {
            anim.SetTrigger("fallkick");
            rb.velocity = new Vector2(rb.velocity.x * 2, -10);
            Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(new Vector3(attackPos.position.x, attackPos.position.y - 0.35f, attackPos.position.z), attackRange * 2f, whatIsEnemies);
            Instantiate(sparks, transform.position, Quaternion.identity);
            for (int i = 0; i < enemiesToDamage.Length; i++)
            {
                enemiesToDamage[i].GetComponent<CorrodingSoldier>().TakeDamage(damage - 3);
            }
        }
    }



    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }
}
