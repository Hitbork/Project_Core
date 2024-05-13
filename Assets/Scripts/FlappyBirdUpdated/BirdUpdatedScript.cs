using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdUpdatedScript : MonoBehaviour
{
    public Rigidbody2D myRigidBody2D;
    public float speed = 5.0f;
    private bool birdIsAlive = true;

    // Start is called before the first frame update
    void Start()
    {
        myRigidBody2D.velocity = Vector2.right * speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && birdIsAlive)
        {
            myRigidBody2D.velocity += Vector2.up * 5;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        birdIsAlive = false;
    }
}
