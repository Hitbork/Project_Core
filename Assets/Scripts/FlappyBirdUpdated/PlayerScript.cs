using FlappyBirdUpdated.LevelConstructor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;


namespace FlappyBirdUpdated
{
    public class PlayerScript : MonoBehaviour
    {
        public Rigidbody2D myRigidBody2D;
        public float speed = 5.0f;
        public bool birdIsAlive { get; private set; } = true;

        // Start is called before the first frame update
        void Start()
        {
            if (LogicScript.instance.isInLevelConstructor) LogicScript.instance.StartGame();
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

        private void OnCollisionEnter2D(Collision2D collision)
        {
            // Check if collision was collision of gameobject with GroundLayer tag.
            // All other collision logics are written in their scripts
            if (collision.gameObject.tag == "GroundLayer" && birdIsAlive)
            {
                LogicScript.instance.GetEvent(collision, out UnityEvent currentEvent);

                currentEvent?.Invoke();
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            UnityEvent currentEvent = new UnityEvent();

            currentEvent.AddListener(LogicScript.FinishGame);

            currentEvent?.Invoke();
        }

        public void SetBirdUnactive() => birdIsAlive = false;
    }
}