using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Respawn : MonoBehaviour
{
    public int respawn;
    Player health;
    // Start is called before the first frame update
    void Start()
    {
        health = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
       // Startover();
        //falloff();
    }

    private void Startover()
    {
        if(health.Player_health <= 0)
        {
            SceneManager.LoadScene(respawn);
        }
    }
    public void falloff()
    {
        if(transform.position.y <= -8)
        {
            SceneManager.LoadScene(respawn);
            Debug.Log("testing respawn");

        }
    }
    public void Startagain()
    {
        Debug.Log("i am in the Start again function");
        SceneManager.LoadScene(respawn);
        levelModifierScript.IncreaseModifier();


    }
    public void again()
    {
        SceneManager.LoadScene(respawn);
    }
}
