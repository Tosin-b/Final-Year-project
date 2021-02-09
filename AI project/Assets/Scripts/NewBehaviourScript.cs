using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript: MonoBehaviour
{

    public float Jumpforce = 1;
    public float MoveMentSpeed =25.0f ;

    [SerializeField]
    public GameObject bulletPrefabs;

    private Rigidbody2D rigidbody;
    public Animator animator;
    private bool IsShhoting;

    [SerializeField]
    Transform bulletSpawnpos;

    bool isFacingLeft;
   

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        var movement = Input.GetAxis("Horizontal");
         transform.position += new Vector3(movement, 0, 0) * Time.deltaTime * MoveMentSpeed;
        Debug.Log(movement);
        bulletDirection();
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
            animator.Play("Shoot");
            shootBullet();
           
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
            GameObject b = Instantiate(bulletPrefabs);
            Debug.Log("SHOWBULLET");
            b.GetComponent<Bullet>().StartShoot(isFacingLeft);
            b.transform.position = bulletSpawnpos.transform.position;
            //b.transform.position = new Vector3(transform.position.x + 0.4f, 5f , 0) * Time.deltaTime * MoveMentSpeed;
        }
        
        void bulletDirection()
        {
            if (movement > 0.01)
            {
                isFacingLeft = true;
            }
            else if (movement < -0.01)
            {
                isFacingLeft = false;
            }
        }
        void Player_damage()
        {
           // if(Col
           //WaitForSeconds()
        }
    }
   

}
 
    
   
