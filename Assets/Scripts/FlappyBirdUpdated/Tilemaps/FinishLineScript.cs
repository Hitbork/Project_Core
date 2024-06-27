using UnityEngine;
using FlappyBirdUpdated;
public class FinishLineScript : MonoBehaviour
{
    [SerializeField] LogicScript logic;
    [SerializeField] PlayerScript birdUpdatedScript;

    // Start is called before the first frame update
    void Start()
    {
        SetLogicScript();
        SetBirdScript();
    }
    private void SetLogicScript()
    {
        try
        {
            logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>();
        }
        // Catching the error because gameobject 
        // tagged player may be set unactive
        catch
        {
            Debug.Log($"{this.name} couldn't find logic script");
        }

    }

    private void SetBirdScript()
    {
        try
        {
            birdUpdatedScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        }
        // Catching the error because gameobject 
        // tagged player may be set unactive
        catch
        {
            Debug.Log($"Layer: {this.name} couldn't find bird");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (logic == null)
            SetLogicScript();

        if (birdUpdatedScript == null)
            SetBirdScript();

        if (birdUpdatedScript.birdIsAlive)
        {
            birdUpdatedScript.SetBirdUnactive();
            LogicScript.FinishGame();
        }
    }
}
