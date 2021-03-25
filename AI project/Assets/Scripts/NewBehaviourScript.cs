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

    [SerializeField]
    Transform enemy;

    //[SerializeField]
    //GameObject checkingEnemyTag;

    [SerializeField]
    float castDistnce;

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
            Debug.Log("Well done you are progressing  :)" + "CurrentPosition: " + currentPosition + "previousPosition: " + previousPosition);
            AddReward(0.05f);
        }
        else if (currentPosition.x < previousPosition.x)
        {
            Debug.Log("You are going backwards o_o" + "CurrentPosition: " + currentPosition + "previousPosition: " + previousPosition);
            AddReward(-0.1f);

        }

        previousPosition = currentPosition;

    }


    public float forceMultiplier = 25;
    public override void OnActionReceived(ActionBuffers actions)
    {

        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = actions.ContinuousActions[0];
        controlSignal.y = actions.ContinuousActions[1];

        bulletDirection();
        Falling();
        autoshoot();
        isJumping();
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


        transform.position += new Vector3(controlSignal.x, 0, 0) * Time.deltaTime * MoveMentSpeed;



        if (controlSignal.x > 0.01)
        {
            LeftRight();
            animator.SetFloat("running", Mathf.Abs(transform.position.x));
            Debug.Log("Player is moving");
        }
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



    }



    public void Falling()
    {
        if (transform.position.y < -8)
        {
            respawn.falloff();
        }
    }

    public void isJumping()
    {
        if (transform.position.y == 1)
        {
            animator.SetTrigger("isjumping");
        }

        if (transform.position.y == 1 && Mathf.Abs(rigidbody.velocity.y) < 0.001f)
        {
            Debug.Log("testing");
            rigidbody.AddForce(new Vector2(0, Jumpforce), ForceMode2D.Impulse);
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
                Debug.Log("shooter");
                shootBullet();
                //sets a reward
                AddReward(1.0f);
            }

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

        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Horizontal");
        continuousActionsOut[1] = Input.GetAxis("Vertical");

    }

    

}



