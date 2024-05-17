using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdUpdatedScript : MonoBehaviour
{
    public Rigidbody2D myRigidBody2D;
    public float speed = 5.0f;
    public LogicScript logic;
    private bool birdIsAlive = true;

    // Start is called before the first frame update
    void Start()
    { 
        logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>();
        myRigidBody2D.velocity = Vector2.right * speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (myRigidBody2D.velocity.x > 5.0f && birdIsAlive)
        {
            myRigidBody2D.velocity -= Vector2.left * 0.1f;
        }
        
        if (myRigidBody2D.velocity.x < 5.0f && birdIsAlive)
        {
            myRigidBody2D.velocity += Vector2.right * 0.1f;
        }

        if (Input.GetKeyDown(KeyCode.Space) && birdIsAlive)
        {
            myRigidBody2D.velocity += Vector2.up * 5;
        }
    }

    public void Bounce(float bounceX, float bounceY)
    {
        myRigidBody2D.velocity += Vector2.left * bounceX;
        myRigidBody2D.velocity += Vector2.up * bounceY;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "BounceLayer")
        {
            logic.gameOver();
            birdIsAlive = false;
        }
    }
}
