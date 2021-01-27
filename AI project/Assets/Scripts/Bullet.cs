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
    }
}
