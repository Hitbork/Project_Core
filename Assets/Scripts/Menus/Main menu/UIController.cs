using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using LoadSceneData.User;

namespace MainMenu
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] GameObject howToPlayPanel;

        private void Update()
        {
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.J) && Input.GetKeyDown(KeyCode.L)) CheatSavingData();
        }

        private void CheatSavingData()
        {
            UserData userData = new UserData();
            userData.indexOfLastUncoveredLevel = 10;
            userData.userName = "cheat";
            userData.userPassword = "cheat";
            userData.Save();
        }

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
            SceneManager.LoadScene("LevelConstructorMenu");
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
