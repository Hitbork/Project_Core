using FlappyBirdUpdated.LevelConstructor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


namespace FlappyBirdUpdated
{
    public class PlayerScript : MonoBehaviour
    {
        public Rigidbody2D myRigidBody2D;
        public float speed = 5.0f;
        public LogicScript logic;
        public bool birdIsAlive { get; private set; } = true;
        [SerializeField] public SomeManager someManager;

        // Start is called before the first frame update
        void Start()
        {
            someManager = GameObject.Find("SomeManager").GetComponent<SomeManager>();
            logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>();
            myRigidBody2D.velocity = Vector2.right * speed;
        }

        // Update is called once per frame
        void Update()
        {
            // Bird always tends to the same speed
            if (myRigidBody2D.velocity.x > speed && birdIsAlive)
            {
                myRigidBody2D.velocity -= Vector2.right * 2f * Time.deltaTime;
            }

            // Bird always tends to the same speed
            if (myRigidBody2D.velocity.x < speed && birdIsAlive)
            {
                myRigidBody2D.velocity += Vector2.right * 2f * Time.deltaTime;
            }

            if (Input.GetKeyDown(KeyCode.Space) && birdIsAlive)
            {
                myRigidBody2D.velocity += Vector2.up * 5;
            }
        }

        public void SetBirdUnactive() => birdIsAlive = false;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            // Check if collision was collision of gameobject with GroundLayer tag.
            // All other collision logics are written in their scripts
            if (collision.gameObject.tag == "GroundLayer" && birdIsAlive)
            {
                ContactPoint2D contact = collision.contacts[0];

                if (someManager.IsDead(collision.gameObject, contact.point))
                {
                    logic.GameOver();
                    SetBirdUnactive();
                } else
                {
                    myRigidBody2D.velocity = new Vector2(-15, 0);
                }
            }
        }
    }
}