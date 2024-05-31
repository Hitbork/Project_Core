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
            if (this.transform.position.y > 17)
            {
                birdIsAlive = false;
                logic.gameOver();
                return;
            }

            if (this.transform.position.y < -17)
            {
                birdIsAlive = false;
                logic.gameOver();
                Destroy(this);
                return;
            }

            if (Input.GetKeyDown(KeyCode.Space) && birdIsAlive)
            {
                myRigidBody.velocity = Vector2.up * flapStrength;
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            logic.gameOver();
            birdIsAlive = false;
        }
    }
}