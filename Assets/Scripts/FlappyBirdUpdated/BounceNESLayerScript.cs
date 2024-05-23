using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceNESLayerScript : MonoBehaviour
{
    public Rigidbody2D birdRigidBody;
    
    private void Start()
    {
        if (GameObject.FindGameObjectWithTag("Player")!= null)
        {
            birdRigidBody = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        } 
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {        
        if (this.gameObject.name == "BounceNESRight")
        {
            birdRigidBody.velocity = new Vector2(15, 0);
            return;
        }

        if (this.gameObject.name == "BounceNESLeft")
        {
            birdRigidBody.velocity = new Vector2(-15, 0);
            return;
        }

        if (this.gameObject.name == "BounceNESUp")
        {
            birdRigidBody.velocity = new Vector2(0, 15);
            return;
        }

        if (this.gameObject.name == "BounceNESDown")
        {
            birdRigidBody.velocity = new Vector2(0, -15);
            return;
        }
    }
}
