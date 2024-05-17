using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceLayerScript : MonoBehaviour
{
    public BirdUpdatedScript birdScript;

    private void Start()
    {
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            birdScript = GameObject.FindGameObjectWithTag("Player").GetComponent<BirdUpdatedScript>();
        } 
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (this.gameObject.name == "BounceRight")
        {
            birdScript.Bounce(-10, 0);
            return;
        }

        if (this.gameObject.name == "BounceLeft")
        {
            birdScript.Bounce(15, 0);
            return;
        }
    }
}
