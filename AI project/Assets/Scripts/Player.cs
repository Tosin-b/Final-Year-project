using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;


public class Player : Agent
{

    public float Jumpforce = 1;
    public float MoveMentSpeed = 25.0f;


    [SerializeField]
    public GameObject bulletPrefabs;

    // [SerializeField]
    //Transform enemy;
    
    [SerializeField]
    Scorebonus bonus;

    //[SerializeField]
    //GameObject checkingEnemyTag;
    [SerializeField]
    scoreManager Player_points;

    [SerializeField]
    float castDistnce;

    [SerializeField]
    float castDistnce1;

    [SerializeField]
    float agrorange;

    [SerializeField]
    float castDistnce1Left;

    [SerializeField]
    levelUp level;

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
    bool endgame;

    public Vector2 pivotPoint = Vector2.zero;
    public float range = 5.0f;
    public float angle = 45.0f;
    public  float angleLeft = 45.0f;

    bool wall;

    private Vector2 startPoint = Vector2.zero;

    public void Awake()
    {
        previousPosition = transform.position;
    }

    private void Start()
    {
       
        bonus = FindObjectOfType<Scorebonus>();

        
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        Player_points = FindObjectOfType<scoreManager>();
    }
    
    public override void OnEpisodeBegin()
    {
        Debug.Log(endgame);
        if (endgame)

        {
            
            Debug.Log("endgame has ben entered");
            endgame = false;
            respawn.Startagain();
            level.LevelingUp();


            Debug.Log(levelModifierScript.moveSpeedModifier + " moveSpeedModifier");
            Debug.Log(levelModifierScript.enemyDamageModifier + " enemyDamageModifier");
            
        }
       
       else if (Player_health <= 0)
        {
            respawn.again();
            levelModifierScript.DecreaseModifier();
            level.LevelDecrement();
            //bonus.resetPoints();
            Debug.Log(levelModifierScript.moveSpeedModifier + " moveSpeedModifier");
            Debug.Log(levelModifierScript.enemyDamageModifier + " enemyDamageModifier");
            
        }

        else if (this.transform.position.x < -10)
        {
            wall = false;
            //Debug.Log("your in");
            respawn.again();
            levelModifierScript.DecreaseModifier();
            level.LevelDecrement();
            bonus.resetPoints();
            Debug.Log(levelModifierScript.moveSpeedModifier + "moveSpeedModifier");
            Debug.Log(levelModifierScript.enemyDamageModifier + "enemyDamageModifier");
            
        }
       
        else if (this.transform.position.y < -8)
        {
            // Debug.Log("am i falling test");
            respawn.again();
            levelModifierScript.DecreaseModifier();
            level.LevelDecrement();
            bonus.resetPoints();
            Debug.Log(levelModifierScript.moveSpeedModifier + "moveSpeedModifier");
            Debug.Log(levelModifierScript.enemyDamageModifier + "enemyDamageModifier");
         
        }
    }
    
    public override void CollectObservations(VectorSensor sensor)
    {
        int observ_player_score = Player_points.score;
        Vector2 endpos = transform.position + Vector3.right * castDistnce;
        RaycastHit2D hit = Physics2D.Linecast(transform.position, endpos, 1 << LayerMask.NameToLayer("Action"));
        Vector2 health = new Vector2(Player_health, 12);
        
        //Debug.Log("Scaled health:  " + health.magnitude);


        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(hit);
        sensor.AddObservation(transform.position.x);
        sensor.AddObservation(transform.position.y);
        sensor.AddObservation(Player_health);
        sensor.AddObservation(observ_player_score);


    }
    public void Update()
    {
        //Debug.Log("sore player script: " + Player_points.score);
        //Debug.Log(transform.position.x);
        currentPosition = transform.position;
        //Debug.Log(endgame);
        // Debug.Log("Current position: " + currentPosition);
        // Debug.Log("Previous Position: " + previousPosition);

        if (currentPosition.x > previousPosition.x)
        {
           // Debug.Log("Well done you are progressing  :)" + "CurrentPosition: " + currentPosition + "previousPosition: " + previousPosition);
            AddReward(0.0001f);
        }
        else if (currentPosition.x < previousPosition.x)
        {
            //Debug.Log("You are going backwards o_o" + "CurrentPosition: " + currentPosition + "previousPosition: " + previousPosition);
            AddReward(-0.0002f);
           //RequestDecision();

        }

        previousPosition = currentPosition;

    }


    public float forceMultiplier = 25;
    public override void OnActionReceived(float[] vectorAction)
    {
       

        bool jumpleft = false;
        Vector3 controlSignal = Vector3.zero;

        controlSignal.x = vectorAction[0];
        controlSignal.y = vectorAction[1];
        bulletDirection();
        GroundcheckLeft();
        Groundcheck();
        Falling();
        LeftRight();
        autoshoot();
        autoshooLeft();
        healthReset();
        jumpingleft();
        jumpRigth();
        //isJumping();
        //Debug.Log(" controlSignal.y = " + controlSignal.y);

        if(vectorAction[1] == 1 && Mathf.Abs(rigidbody.velocity.y) < 0.001f)
        {
            vectorAction[1] = 0;
            animator.SetTrigger("isjumping");
            rigidbody.AddForce(new Vector2(0, Jumpforce), ForceMode2D.Impulse);
            rigidbody.AddForce(new Vector2(8f, 0), ForceMode2D.Impulse);
            //Debug.Log("Rigth jump is working fine");
            
        }
         else if (vectorAction[1] == 2 && Mathf.Abs(rigidbody.velocity.y) < 0.001f && jumpleft == true)
        {
           
            animator.SetTrigger("isjumping");
            rigidbody.AddForce(new Vector2(0, Jumpforce), ForceMode2D.Impulse);
            rigidbody.AddForce(new Vector2(-8f, 0), ForceMode2D.Impulse);
            jumpleft = false;
            vectorAction[1] = 0;
            //Debug.Log("left jumping is workig fine toon");
           
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
        void jumpRigth()
        {
            Vector2 endpos1 = transform.position + Vector3.right * castDistnce1;
            RaycastHit2D hit1 = Physics2D.Linecast(transform.position, endpos1, 1 << LayerMask.NameToLayer("Obstacle"));
            if(hit1.collider != null)
            {
                if (hit1.collider.gameObject.CompareTag("Obstacle") && isFacingLeft == true)
                {
                    vectorAction[1] = 1;
                }
            }
            else
            {
                Debug.DrawLine(transform.position, endpos1, Color.blue);

            }


            // I impleted the code below to try and change the angle of the raycast
            //RaycastHit2D test = Physics2D.Raycast(transform.position, -Vector2.up);
            //Ray ray = new Ray();
            //ray.origin = transform.position;
            //ray.direction = Quaternion.AngleAxis(12.0f, Vector3.forward) * Vector3.right;
            //Debug.DrawLine(transform.position, ray, Color.green);
            //Debug.DrawLine(test, Color.yellow);

            //if (hit1.collider != null)
            //{
            //    if (hit1.collider.gameObject.CompareTag("Obstacle"))
            //    {
            //        vectorAction[1] = 1;
            //    }
            //}
            //else
            //{
            //    Debug.DrawLine(transform.position, endpos1, Color.blue);

            //}

        }
        void Groundcheck()
        {
            int layerMask = ~(LayerMask.GetMask("tosin"));
            startPoint = transform.position + Vector3.right; // Update starting ray point.

            // Direct use.
            // Get normalized (of length = 1) distance vector.
             Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)).normalized;

            // Using function.
            // Vector2 direction = GetDirectionVector2D(angle);
            RaycastHit2D groundEnd;

             groundEnd = Physics2D.Raycast(startPoint, direction, range, layerMask); // Shot ray.


            if(groundEnd.collider != null)
            {
               // Debug.Log("RayCast: " + groundEnd.collider.gameObject.layer.ToString());

                switch (groundEnd.collider.gameObject.layer)

                {
                    case 11:
                        vectorAction[1] = 1;
                        
                    break;

                    default:
                        break;
                }
            }

            // Draw ray. For Debug we have to multiply our direction vector. 
            // Even if there is said Debug.DrawRay(start, dir), not Debug.DrawRay(start, end). Keep that in mind.
            Debug.DrawRay(startPoint, direction * range, Color.yellow);
        }
        void GroundcheckLeft()
        {
           
            int layerMask = ~(LayerMask.GetMask("tosin"));
            startPoint = transform.position + Vector3.left; // Update starting ray point.

            // Direct use.
            // Get normalized (of length = 1) distance vector.
            Vector2 direction = new Vector2(Mathf.Cos(angleLeft * Mathf.Deg2Rad), Mathf.Sin(angleLeft * Mathf.Deg2Rad)).normalized;

            // Using function.
            // Vector2 direction = GetDirectionVector2D(angle);
            RaycastHit2D groundEnd;

            groundEnd = Physics2D.Raycast(startPoint, direction, range, layerMask); // Shot ray.


            if (groundEnd.collider != null && isFacingLeft == false)
            {
               // Debug.Log("RayCast: " + groundEnd.collider.gameObject.layer.ToString());

                switch (groundEnd.collider.gameObject.layer)

                {
                    case 11:
                        jumpleft = true;
                        vectorAction[1] = 2;
                        //Debug.Log("testing");
                        break;

                   
                }
            }

            // Draw ray. For Debug we have to multiply our direction vector. 
            // Even if there is said Debug.DrawRay(start, dir), not Debug.DrawRay(start, end). Keep that in mind.
            Debug.DrawRay(startPoint, direction * range, Color.green);
        }
        void jumpingleft()
        {

            Vector2 endpos2 = transform.position + Vector3.left * castDistnce1Left;
            RaycastHit2D hit2 = Physics2D.Linecast(transform.position, endpos2, 1 << LayerMask.NameToLayer("Obstacle"));
            if (hit2.collider != null)
            {
                jumpleft = true;
                if (hit2.collider.gameObject.CompareTag("Obstacle") && isFacingLeft == false)
                {
                    vectorAction[1] = 2;
                }
            }
            else
            {
                Debug.DrawLine(transform.position, endpos2, Color.red);
            }
        }
        

    }
    /*
    public Vector2 GetDirectionVector2D(float angle)
    {
        return new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)).normalized;
    }
    */
   
    public void Falling()
    {
        if (this.transform.position.y < -8)
        {
            AddReward(-0.5f);
            EndEpisode();
        }


    }

  
    public void autoshoot()
    {
        
        Vector2 endpos = transform.position + Vector3.right * castDistnce;
        RaycastHit2D hit = Physics2D.Linecast(transform.position, endpos, 1 << LayerMask.NameToLayer("Action"));
        if (hit.collider != null)
        {
            if (hit.collider.gameObject.CompareTag("enemy") && Time.time > shootingTime + firerate && isFacingLeft== true)
            {

                shootingTime = Time.time;
                shootBullet();
                //Shooting Right
                AddReward(0.012f);
            }
            
        }
        else
        {
            Debug.DrawLine(transform.position, endpos, Color.black);
        }
    }
    public void autoshooLeft()
    {

        Vector2 endpos4 = transform.position + Vector3.left * castDistnce;
        RaycastHit2D hit4 = Physics2D.Linecast(transform.position, endpos4, 1 << LayerMask.NameToLayer("Action"));
        if (hit4.collider != null)
        {
            if (hit4.collider.gameObject.CompareTag("enemy") && Time.time > shootingTime + firerate && isFacingLeft == false)
            {

                shootingTime = Time.time;
                shootBullet();
                //sets a reward
                //Shooting Right
                AddReward(0.012f);
            }

        }
        else
        {
            Debug.DrawLine(transform.position, endpos4, Color.black);
        }
    }
    public void healthReset()
    {
        if (Player_health <= 0)
        {
            AddReward(-0.5f);
            EndEpisode();
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
           // Debug.Log("oof you hit the head");
            AddReward(-0.0002f);
           
            //respawn.Startagain();
        }
    }

    private void Procces(GameObject gameObject)
    {
        if (gameObject.CompareTag("enemy"))

        {

            float hurt = 2f;
            Player_health = Player_health - hurt- levelModifierScript.enemyDamageModifier;
            rigidbody.AddForce(new Vector2(-12f, 0), ForceMode2D.Impulse);
            //animator.Play("hurt");
            AddReward(-0.02f);
           

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("wallReward"))
        {
            Debug.Log("you hit the reward"); 
            AddReward(0.001f);
        }
        //this part works reminder to delte the other function playerrhitwall
        else if (collision.gameObject.CompareTag("start"))
        {
           Debug.Log("coooooooooooooooool");
            wall = true;
            Debug.Log(wall);
            AddReward(-0.001f);
            EndEpisode();
        }
        else if(collision.gameObject.CompareTag("end1"))
        {

            AddReward(0.8f);
            Debug.Log("i have fineshed the game");
            endgame = true;
            if(Player_health >= 11)
            {
                Debug.Log("you are really good at this");
                AddReward(0.5f);
            }
            else
            {
                AddReward(-0.03f);
            }
            if(Player_points.score >= 130)
            {
                Debug.Log("you have ht the max points");
                AddReward(0.5f);
            }
            EndEpisode();

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



