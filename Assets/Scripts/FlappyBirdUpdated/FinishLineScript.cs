using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLineScript : MonoBehaviour
{
    public UpdLogicScript logic;

    // Start is called before the first frame update
    void Start()
    {
        SetLogicScript();
    }
    private void SetLogicScript()
    {
        try
        {
            logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<UpdLogicScript>();
        }
        // Catching the error because gameobject 
        // tagged player may be set unactive
        catch
        {
            Debug.Log($"{this.name} couldn't find logic script");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (logic == null)
        {
            SetLogicScript();
        }

        logic.FinishGame();
    }
}
