using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    private Transform player = null;

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

        if (player != null)
        {
            Vector3 temp = transform.position;
            temp.x = player.position.x;
            temp.y = player.position.y;

            transform.position = temp;
        }
    }
}
