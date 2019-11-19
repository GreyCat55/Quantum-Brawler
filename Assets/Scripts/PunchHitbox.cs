using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchHitbox : MonoBehaviour {
    public float xvelocity = 0;
    Rigidbody2D rb;
    Animator anim;
    public float decayTime;
    ParticleSystem sparks;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        Invoke("disintegrate", decayTime);
        rb.AddForce(new Vector2(xvelocity/2f,0), ForceMode2D.Impulse);
        sparks = gameObject.GetComponent<ParticleSystem>();
        sparks.Stop();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player") && !collision.gameObject.CompareTag("punch"))
        {
            sparks.Play();
            rb.AddForce(new Vector2(xvelocity * 10, 5), ForceMode2D.Impulse);
            disintegrate();
        }

    }

    void disintegrate()
    {
        Destroy(gameObject, decayTime);
    }
}
