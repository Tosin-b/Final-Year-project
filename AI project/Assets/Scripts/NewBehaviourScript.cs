using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript: MonoBehaviour
{

    public float Jumpforce = 1;
    public float MoveMentSpeed =25.0f ;
    public GameObject bulletPrefabs;
    private Rigidbody2D rigidbody;
    public Animator animator;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        var movement = Input.GetAxis("Horizontal");
        transform.position += new Vector3(movement, 0, 0) * Time.deltaTime * MoveMentSpeed;
        //transform.localScale = new Vector2(-1, 1);
        animator.SetFloat("running",Mathf.Abs(movement));

        if (Input.GetButtonDown("Jump"))
        {
            animator.SetTrigger("isjumping");
        }

        if (Input.GetButtonDown("Jump") && Mathf.Abs(rigidbody.velocity.y) < 0.001f)
        {
            Debug.Log("testing");
            rigidbody.AddForce(new Vector2(0, Jumpforce), ForceMode2D.Impulse);
        }
        if(Input.GetKeyDown(KeyCode.S))
        {
            shootBullet();
            Debug.Log("testing shoot button");
        }

        Vector3 characterscale = transform.localScale;
        if(Input.GetAxis("Horizontal") < 0)
        {
            characterscale.x = -1;
        }
        if (Input.GetAxis("Horizontal") > 0)
        {
            characterscale.x = 1;
        }
        transform.localScale = characterscale;

        void shootBullet()
        {
            GameObject b = Instantiate(bulletPrefabs) as GameObject;
            b.transform.position = transform.position;
        }
    }
   
}
 
    
   
