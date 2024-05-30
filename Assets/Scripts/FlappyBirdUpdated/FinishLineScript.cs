using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLineScript : MonoBehaviour
{
    public Rigidbody2D birdRigidBody = null;

    // Start is called before the first frame update
    void Start()
    {
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
            Debug.Log($"{this.name} couldn't find the bird");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (birdRigidBody == null)
        {
            SetRigidbody();
        }


    }
}
