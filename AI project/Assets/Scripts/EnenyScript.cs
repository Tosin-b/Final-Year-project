using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnenyScript : MonoBehaviour
{
    [SerializeField]
    Transform player;

    [SerializeField]
    float agrorange;

    [SerializeField]
    float movespeed;
    
    public Animator animator;

    Rigidbody2D rb;
    private int health =10;

    // Start is called before the first frame update
    void Start()

    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }


    // Update is called once per frame
    void Update()
    {
        float distFromPlayer = Vector2.Distance(transform.position, player.position);
        enenyMovement();
        Debug.Log("distFromPlayer:" + distFromPlayer);

        if(distFromPlayer < agrorange)
        {
            chase();
          
        }
        else
        {
            stopchasing();
        
        }
    }

    private void stopchasing()
    {
        rb.velocity = new Vector2(0, 0);
    }

    private void chase()
    {
        if (transform.position.x < player.position.x)
        {
            animator.SetFloat("run-crab",Mathf.Abs(transform.position.x));
            rb.velocity = new Vector2(movespeed, 0);
            transform.localScale = new Vector2(-1, 1);
            
        }
        else if (transform.position.x > player.position.x)
        {
            animator.SetFloat("run-crab",Mathf.Abs(transform.position.x));
            rb.velocity = new Vector2(-movespeed, 0);
            transform.localScale = new Vector2(1, 1);

        }
    }
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Bullet"))
        {
            Destroy(collision.gameObject);
            health--;
            if(health <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    private void enenyMovement()
    {
        if(transform.position.x > 0.01) 
            animator.SetFloat("run-crab",transform.position.x);
        else if(transform.position.x<0.01)
            animator.SetFloat("run-crab", transform.position.x);
    }
}
