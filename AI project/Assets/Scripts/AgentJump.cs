﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentJump : MonoBehaviour
{
    NewBehaviourScript player;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<NewBehaviourScript>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void jumpDirection(bool isFacingLeft)
    {
        float dirX;
        float dirY;

        if (isFacingLeft)
        {
            dirX = 1f;
        }
        else
        {
            dirY = -1f;
        }
       // player.controlsignal

    }
}