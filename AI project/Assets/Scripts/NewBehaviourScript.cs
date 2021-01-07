using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript: MonoBehaviour
{

    public float Jumpforce = 1;
    public float MoveMentSpeed = 1.0f;
    private Rigidbody2D rigidbody;
    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        var movement = Input.GetAxis("Horizontal");
        transform.position += new Vector3(movement, 0, 0) * Time.deltaTime * MoveMentSpeed;

        if (Input.GetButtonDown("Jump") && Mathf.Abs(rigidbody.velocity.y) < 0.001f)
        {
            Debug.Log("testing");
            rigidbody.AddForce(new Vector2(0, Jumpforce), ForceMode2D.Impulse);
        }
    }
}