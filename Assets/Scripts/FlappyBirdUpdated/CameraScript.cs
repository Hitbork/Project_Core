using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FlappyBirdUpdated
{
    public class CameraScript : MonoBehaviour
    {
        private Transform player = null;
        public Vector3 offset;
        public float damping;

        private Vector3 velocity = Vector3.zero;

        // Start is called before the first frame update
        void Start()
        {
            SetPlayer();
        }

        private void SetPlayer()
        {
            try
            {
                player = GameObject.FindGameObjectWithTag("Player").transform;
            }
            catch
            {
                Debug.Log("Camera hasn't found the player");
            }

        }

        // Update is called once per frame
        void Update()
        {
            if (player == null)
            {
                SetPlayer();
            }

            Vector3 movePosition = player.position + offset;
            transform.position = Vector3.SmoothDamp(transform.position, movePosition, ref velocity, damping);
        }
    }
}