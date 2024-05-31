using UnityEngine;

public class FinishLineScript : MonoBehaviour
{
    [SerializeField] UpdLogicScript logic;
    [SerializeField] BirdUpdatedScript birdUpdatedScript;

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
            logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<UpdLogicScript>();
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
            birdUpdatedScript = GameObject.FindGameObjectWithTag("Player").GetComponent<BirdUpdatedScript>();
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
            logic.FinishGame();
        }
    }
}
