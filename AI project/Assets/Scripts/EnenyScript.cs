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

    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float distFromPlayer = Vector2.Distance(transform.position, player.position);
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
            rb.velocity = new Vector2(movespeed, 0);
            transform.localScale = new Vector2(-1, 1);
        }
        else if (transform.position.x > player.position.x)
        {
            rb.velocity = new Vector2(-movespeed, 0);
            transform.localScale = new Vector2(1, 1);
        }
    }
}
