using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStatusManager : MonoBehaviour
{
    public bool isTestableMode = false;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.P)) TurnOnTestableMode();
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.L)) TurnOffTestableMode();
    }

    public void TurnOnTestableMode()
    {
        GameObject.FindGameObjectWithTag("Camera").GetComponent<CameraScript>().Invoke();
    }

    public void TurnOffTestableMode()
    {

    }
}
