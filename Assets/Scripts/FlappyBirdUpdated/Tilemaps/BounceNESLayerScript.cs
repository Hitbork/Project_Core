using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FlappyBirdUpdated
{
    public class BounceNESLayerScript : MonoBehaviour
    {
        public Rigidbody2D birdRigidBody = null;
        private string layerName;

        private void Start()
        {
            // Defining the name of each layer
            layerName = this.gameObject.name;

            // Trying to find rigidbody of player
            SetRigidbody();
        }

        private void SetRigidbody()
        {
            try
            {
                birdRigidBody = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
            }
            // Catching the error because gameobject 
            // tagged player may be set unactive
            catch
            {
                Debug.Log("BounceNESLayer couldn't find the bird");
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (birdRigidBody == null)
            {
                SetRigidbody();
            }

            Vector2Int tempVector = new Vector2Int(0, 0);

            // Defining the direction of vector
            switch (layerName)
            {
                // Direction: Upwards
                case "BounceNESUp":
                    tempVector.y = 15;
                    break;

                // Direction: Upwards-right
                case "BounceNESUpRight":
                    tempVector.x = 15;
                    tempVector.y = 15;
                    break;

                // Direction: Right
                case "BounceNESRight":
                    tempVector.x = 15;
                    break;

                // Direction: Downwards-right
                case "BounceNESDownRight":
                    tempVector.x = 15;
                    tempVector.y = -15;
                    break;

                // Direction: Downwards
                case "BounceNESDown":
                    tempVector.y = -15;
                    break;

                // Direction: Dowwards-left
                case "BounceNESDownLeft":
                    tempVector.x = -15;
                    tempVector.y = -15;
                    break;

                // Direction: Left
                case "BounceNESLeft":
                    tempVector.x = -15;
                    break;

                // Direction: Dowwards-left
                case "BounceNESUpLeft":
                    tempVector.x = -15;
                    tempVector.y = 15;
                    break;

                // Direction: undefined
                default:
                    Debug.Log("Wrong name of layer");
                    break;
            }

            birdRigidBody.velocity = tempVector;
        }
    }
}
