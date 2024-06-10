using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FlappyBirdDefault
{
    public class PipeMoveScript : MonoBehaviour
    {
        public float MoveSpeed = 5;
        public float deadZone = -38;

        // Update is called once per frame
        void Update()
        {
            transform.position = transform.position + (Vector3.left * MoveSpeed) * Time.deltaTime;
        
            if (transform.position.x < deadZone)
            {
                Debug.Log("Pipe Deleted");
                Destroy(gameObject);
            }
        }
    }
}