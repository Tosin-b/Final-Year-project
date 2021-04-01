using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;


public class NewBehaviourScript : Agent
{

    public float Jumpforce = 1;
    public float MoveMentSpeed = 25.0f;


    [SerializeField]
    public GameObject bulletPrefabs;

   // [SerializeField]
    //Transform enemy;

    //[SerializeField]
    //GameObject checkingEnemyTag;

    [SerializeField]
    float castDistnce;

    [SerializeField]
    float castDistnce1;

    [SerializeField]
    float agrorange;

    private Rigidbody2D rigidbody;
    public Animator animator;
    private bool IsShhoting;

    [SerializeField]
    Transform bulletSpawnpos;

    [SerializeField]
    public float Player_health = 10f;

    bool isFacingLeft;

    float shootingTime = 0.23f;

    public float firerate = 0.4f;

    public Respawn respawn;
    Vector3 previousPosition;
    Vector3 currentPosition;

   
    public bool jumpingFlag = false;

    public void Awake()
    {
        previousPosition = transform.position;
    }

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    public override void OnEpisodeBegin()
    {
        Falling();
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        Vector2 endpos = transform.position + Vector3.right * castDistnce;
        RaycastHit2D hit = Physics2D.Linecast(transform.position, endpos, 1 << LayerMask.NameToLayer("Action"));

        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(hit);
        sensor.AddObservation(transform.position.x);
        sensor.AddObservation(transform.position.y);


    }
    public void Update()
    {
        currentPosition = transform.position;
        // Debug.Log("Current position: " + currentPosition);
        // Debug.Log("Previous Position: " + previousPosition);

        if (currentPosition.x > previousPosition.x)
        {
           // Debug.Log("Well done you are progressing  :)" + "CurrentPosition: " + currentPosition + "previousPosition: " + previousPosition);
            AddReward(0.2f);
        }
        else if (currentPosition.x < previousPosition.x)
        {
            //Debug.Log("You are going backwards o_o" + "CurrentPosition: " + currentPosition + "previousPosition: " + previousPosition);
            AddReward(-0.4f);
            RequestDecision();

        }

        previousPosition = currentPosition;

    }


    public float forceMultiplier = 25;
    public override void OnActionReceived(float[] vectorAction)
    {
          Vector3 controlSignal = Vector3.zero;

        controlSignal.x = vectorAction[0];
        controlSignal.y = vectorAction[1];

        bulletDirection();
        Falling();
        autoshoot();
        jumpDirection();
        //isJumping();
        //Debug.Log(" controlSignal.y = " + controlSignal.y);

        if(vectorAction[1] == 1 && Mathf.Abs(rigidbody.velocity.y) < 0.001f)
        {
            vectorAction[1] = 0;
            rigidbody.AddForce(new Vector2(0, Jumpforce), ForceMode2D.Impulse);
            Debug.Log("checking for jump to end");
            
        }
        void LeftRight()
        {
            Vector3 characterscale = transform.localScale;
            if (controlSignal.x < 0)
            {
                characterscale.x = -1;
            }
            if (controlSignal.x > 0)
            {
                characterscale.x = 1;
            }
            transform.localScale = characterscale;
        }

        //rigidbody.AddForce(controlSignal.x * forceMultiplier, ForceMode2D.Force);
       transform.position += new Vector3(controlSignal.x, 0, 0) * Time.deltaTime * MoveMentSpeed;
        
         
       

        if (controlSignal.x > 0.01)
        {
            LeftRight();
            animator.SetFloat("running", Mathf.Abs(transform.position.x));
           // Debug.Log("Player is moving");

        }else if (controlSignal.x <-0.01) 
        {
            LeftRight();
            animator.SetFloat("running", Mathf.Abs(transform.position.x));
          //  Debug.Log("Player is moving");
        }

        // add code for swithing to the left
        void bulletDirection()
        {
            if (controlSignal.x > 0.01)
            {
                isFacingLeft = true;
            }
            else if (controlSignal.x < -0.01)
            {
                isFacingLeft = false;
            }
        }
        void jumpDirection()
        {
            Vector2 endpos1 = transform.position + Vector3.right * castDistnce1;
            RaycastHit2D hit1 = Physics2D.Linecast(transform.position, endpos1, 1 << LayerMask.NameToLayer("Obstacle"));
            if(hit1.collider != null)
            {
                if (hit1.collider.gameObject.CompareTag("Obstacle"))
                {
                    vectorAction[1] = 1;
                }
            }
            else
            {
                Debug.DrawLine(transform.position, endpos1, Color.blue);

            }

        }
        

    }


    public void Falling()
    {
        if (transform.position.y < -8)
        {
            respawn.falloff();
        }
    }

  
    public void autoshoot()
    {
        
        Vector2 endpos = transform.position + Vector3.right * castDistnce;
        RaycastHit2D hit = Physics2D.Linecast(transform.position, endpos, 1 << LayerMask.NameToLayer("Action"));
        if (hit.collider != null)
        {
            if (hit.collider.gameObject.CompareTag("enemy") && Time.time > shootingTime + firerate)
            {

                shootingTime = Time.time;
                shootBullet();
                //sets a reward
                AddReward(1.0f);
            }
            

        }
        else
        {
            Debug.DrawLine(transform.position, endpos, Color.black);
        }
    }

    public void shootBullet()
    {
        animator.Play("Shoot");
        GameObject b = Instantiate(bulletPrefabs);
        b.GetComponent<Bullet>().StartShoot(isFacingLeft);
        b.transform.position = bulletSpawnpos.transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Procces(collision.gameObject);
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("wel done");
        }
         if (collision.gameObject.CompareTag("wallReward"))
        {
            Debug.Log("nice u hit the wall ------------");
        }
    }

    private void Procces(GameObject gameObject)
    {
        if (gameObject.CompareTag("enemy"))

        {

            float hurt = 2f;
            Player_health = Player_health - hurt;
            rigidbody.AddForce(new Vector2(-12f, 0), ForceMode2D.Impulse);
            animator.Play("hurt");
            AddReward(-0.5f);
            EndEpisode();

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("wallReward"))
        {
            Debug.Log("you hit te reward");
        }
    }


    public override void Heuristic(float[] actionsOut)
    
    {
        actionsOut[1] = 0;
        actionsOut[0] = Input.GetAxis("Horizontal");
        if (Input.GetKey(KeyCode.UpArrow) == true){
            actionsOut[1] = 1;
        }

    }

    

}



