using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class UIControllerClass : MonoBehaviour
{
    public void OpenMainMenu() => SceneManager.LoadScene("MainMenu");
}
