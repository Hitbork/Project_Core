using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineCameraScript : MonoBehaviour
{
    private Transform player = null;

    void Start()
    {
        SetPlayer();
        CinemachineVirtualCamera cam = this.GetComponent<CinemachineVirtualCamera>();
        cam.Follow = player;
    }

    private void SetPlayer()
    {
        try
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        catch
        {
            Debug.Log("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
        }

    }
}
