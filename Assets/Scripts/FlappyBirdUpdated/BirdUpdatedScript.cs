using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdUpdatedScript : MonoBehaviour
{
    public Rigidbody2D myRigidBody2D;
    public Transform transformOfBird;
    public float multiplierToRotation = 2f, 
        speed = 5.0f, 
        multiplierToVelocity = 2f;
    public LogicScript logic;
    private bool birdIsAlive = true, 
        isRotatingBack = false;

    // Start is called before the first frame update
    void Start()
    { 
        logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>();
        myRigidBody2D.velocity = Vector2.right * speed;
    }

    // Update is called once per frame
    void Update()
    {
        float angleOfBird = transformOfBird.rotation.eulerAngles.z;
        if (isRotatingBack)
        {
            if (angleOfBird != 0)
            {
                var trans = transformOfBird.rotation.eulerAngles;
                if (angleOfBird <= 180 && angleOfBird >= 0)
                {
                    if (angleOfBird <= multiplierToRotation)
                    {
                        angleOfBird = 0;
                        isRotatingBack = false;
                    }
                    else
                        angleOfBird -= multiplierToRotation * Time.deltaTime;
                }
                else
                {
                    if (angleOfBird >= 360 - multiplierToRotation)
                    {
                        angleOfBird = 0;
                        isRotatingBack = false;
                    }
                    else
                        angleOfBird += multiplierToRotation * Time.deltaTime;
                }
                trans.z = angleOfBird;
                transformOfBird.rotation = Quaternion.Euler(trans.x, trans.y, trans.z);
            }
            Debug.Log("Rotating");
        }
        else
        {
            if (angleOfBird >= 160 && angleOfBird <= 200)
            {
                myRigidBody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
                isRotatingBack = true;
            }
            Debug.Log("UNTI");
        }

        if (myRigidBody2D.velocity.x > 5.0f && birdIsAlive)
        {
            myRigidBody2D.velocity -= Vector2.left * multiplierToVelocity * Time.deltaTime;
        }

        if (myRigidBody2D.velocity.x < 5.0f && birdIsAlive)
        {
            myRigidBody2D.velocity += Vector2.right * multiplierToVelocity * Time.deltaTime;
        }
        
        if (Input.GetKeyDown(KeyCode.Space) && birdIsAlive)
        {
            myRigidBody2D.velocity += Vector2.up * 5;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "GroundLayer")
        {
            logic.gameOver();
            birdIsAlive = false;
        }
    }
}
