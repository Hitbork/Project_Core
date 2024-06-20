using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using LoadSceneData.User;
using System.Globalization;

namespace FlappyBirdUpdated
{
    public class LogicScript : MonoBehaviour
    {
        UserData userData = new UserData();

        public bool isInLevelConstructor = false;
        public int playerScore;
        public TMP_Text scoreText;
        public GameObject gameOverScreen, gameFinishScreen, playNextLevelButton;

        private double timer = 0;
        private bool isGameEnded = false;

        private void Start()
        {
            userData.Load();

            isInLevelConstructor = SceneManager.GetActiveScene().name == "LevelConstructor";
        }

        private void Update()
        {
            timer += Time.deltaTime;

            if (!isGameEnded)
                UpdateTime();

            if (!isInLevelConstructor)
                if (Input.GetKeyDown(KeyCode.R)) RestartGame();
        }

        [ContextMenu("Increase Score")]
        public void UpdateTime() => scoreText.text = System.Math.Round(timer, 1).ToString("0.0", CultureInfo.InvariantCulture);
   
        public void RestartGame() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        public void GameOver()
        {
            isGameEnded = true;

            gameOverScreen.SetActive(true);
        }

        public void FinishGame()
        {
            isGameEnded = true;

            if (!isInLevelConstructor)
            {
                int indexOfCurrentLevel = GetLevelNumber() - 1;

                 userData.LevelFinished(indexOfCurrentLevel, timer);
            }

            gameFinishScreen.SetActive(true);

            if (GetLevelNumber() == 9)
                playNextLevelButton.SetActive(false);
        }

        private int GetLevelNumber()
        {
            // This method is used while playing default levels
            // Getting current scene name
            string currentSceneName = SceneManager.GetActiveScene().name;

            // Trimming last digit from current scene name 
            return (int)char.GetNumericValue(currentSceneName[currentSceneName.Length - 1]);
        }

        public void PlayNextLevel()
        {
            // This method is used while playing default levels
            // Getting current scene name
            string currentSceneName = SceneManager.GetActiveScene().name;

            int nextLevelNumber = GetLevelNumber() + 1;

            // Setting next level scene name by remove last digit from current and adding string form of next number level
            string nextLevelSceneName = currentSceneName.Remove(currentSceneName.Length - 1) + nextLevelNumber.ToString();

            // Opening the scene
            SceneManager.LoadScene(nextLevelSceneName);
        }

        public void OpenLevelsMenu() => SceneManager.LoadScene("LevelsMenu");

        public void OpenMainMenu() => SceneManager.LoadScene("MainMenu");
    }
}

