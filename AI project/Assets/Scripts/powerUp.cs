﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class powerUp : MonoBehaviour
{
    private Rigidbody2D rb;
    Player player;
    public float newhealth;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<Player>();
        
        
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Damage();
          
            Destroy(gameObject);
        }
    }

    private void Damage()
    {
        player.Player_health = player.Player_health + 1;
       
        
    }
}
