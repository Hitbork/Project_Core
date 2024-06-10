using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MainMenu
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] GameObject howToPlayPanel;

        public void PlayBTNClick()
        {
            SceneManager.LoadScene("LevelsMenu");
        }

        public void ClassicFlappyBirdClick()
        {
            SceneManager.LoadScene("FlappyBirdDefault");
        }

        public void LevelConstructorClick()
        {
            SceneManager.LoadScene("LevelConstructor");
        }

        public void HowToPlayClick()
        {
            howToPlayPanel.SetActive(!howToPlayPanel.activeSelf);
        }

        public void CancelHTPClick()
        {
            howToPlayPanel.SetActive(false);
        }

        public void QuitClick()
        {
            Application.Quit();
        }
    }
}
