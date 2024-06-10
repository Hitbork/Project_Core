using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FlappyBirdDefault
{
    public class BirdScript : MonoBehaviour
    {
        public Rigidbody2D myRigidBody;
        public float flapStrength;
        public LogicScript logic;
        public bool birdIsAlive = true;

        // Start is called before the first frame update
        void Start()
        {
            logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>();
        }

        // Update is called once per frame
        void Update()
        {
            // If bird reached top of screen player gets death
            if (this.transform.position.y > logic.mainCam.topBorder)
            {
                DeathOfPlayer();
                return;
            }

            // If bird reached bottom of screen player gets death
            if (this.transform.position.y < logic.mainCam.bottomBorder)
            {
                DeathOfPlayer();
                Destroy(this);
                return;
            }

            // Making the bird jump as player hits spacebar
            if (Input.GetKeyDown(KeyCode.Space) && birdIsAlive)
            {
                myRigidBody.velocity = Vector2.up * flapStrength;
            }
        }

        // Death of player in classic flappy bird always calls gameOver method
        private void DeathOfPlayer()
        {
            birdIsAlive = false;
            logic.gameOver();
        }

        private void OnCollisionEnter2D(Collision2D collision) => DeathOfPlayer();
    }
}