using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyJumper : MonoBehaviour
{

    [SerializeField]
    Transform player;

    [SerializeField]
    float agrorange;

    [SerializeField]
    float movespeed;

    [SerializeField]
    Transform Bullet;

    Rigidbody2D rb;

    [SerializeField]
    private int health = 3;

    private Material matwhite;
    private Material matDefault;
    private UnityEngine.Object explosionref;
    SpriteRenderer sr;
    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    
        rb = GetComponent<Rigidbody2D>();
        matwhite = Resources.Load("WhiteFlash", typeof(Material)) as Material;
        matDefault = sr.material;
        explosionref = Resources.Load("Explosion");
    }

    // Update is called once per frame
    void Update()
    {
        float distFromPlayer2 = Vector2.Distance(transform.position, player.position);
        
        Debug.Log("distFromPlayer:" + distFromPlayer2);

        if (distFromPlayer2 < agrorange)
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
           // animator.SetFloat("run-crab", Mathf.Abs(transform.position.x));
            rb.velocity = new Vector2(movespeed, 0);
            transform.localScale = new Vector2(-1, 1);

        }
        else if (transform.position.x > player.position.x)
        {
           // animator.SetFloat("run-crab", Mathf.Abs(transform.position.x));
            rb.velocity = new Vector2(-movespeed, 0);
            transform.localScale = new Vector2(1, 1);

        }
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            Destroy(collision.gameObject);
            health--;
            sr.material = matwhite;



            if (health <= 0)
            {
                killself();
            }
            else
            {
                Invoke("ResetMaterial", .1f);
            }
        }


    }



    private void ResetMaterial()
    {
        sr.material = matDefault;
    }
    private void killself()
    {
        GameObject explosionn = (GameObject)Instantiate(explosionref);
        explosionn.transform.position = new Vector3(transform.position.x, transform.position.y + .3f, transform.position.z);
        Destroy(gameObject);

    }


   
}
