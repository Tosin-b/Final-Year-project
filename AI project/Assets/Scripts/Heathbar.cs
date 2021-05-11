using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Heathbar : MonoBehaviour
{
    private Image Healthbar;
    public float currentHealth=10f;
    private float MaxHealth = 14;
    Player Player;

    private void Start()
    {
        Healthbar = GetComponent<Image>();
        Player = FindObjectOfType<Player>();
    }
    private void Update()
    {
        update();
        currentHealth = Player.Player_health;
        Healthbar.fillAmount = currentHealth / MaxHealth;


    }

    private void update()
    {
       // currentHealth = currentHealth.g;
    }
}
