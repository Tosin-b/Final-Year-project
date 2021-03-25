using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    float speed;

    [SerializeField]
    int damage;
    private Rigidbody2D rb;
    private Vector2 ScreenBounds;

  
    

    [SerializeField]
    float timeToDestroy = 3;

   // [SerializeField] public GameObject explosion;
    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    

    public void StartShoot(bool isFacingLeft)
    {

        Rigidbody2D rb2d = GetComponent<Rigidbody2D>();
        if (isFacingLeft)
        {
            rb2d.velocity = new Vector2(speed, 0);
        }
        else
        {
            rb2d.velocity = new Vector2(-speed, 0);
        }
//
        Destroy(gameObject,timeToDestroy);
    }
    

}
