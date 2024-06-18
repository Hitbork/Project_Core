using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using LoadSceneData.User;

namespace FlappyBirdUpdated
{
    public class LogicScript : MonoBehaviour
    {
        UserData userData = new UserData();

        public bool isInLevelConstructor = false;
        public int playerScore;
        public TMP_Text scoreText;
        public GameObject gameOverScreen, gameFinishScreen, playNextLevelButton;

        private void Start()
        {
            userData.Load();

            isInLevelConstructor = SceneManager.GetActiveScene().name == "LevelConstructor";
        }

        private void Update()
        {
            if (!isInLevelConstructor)
                if (Input.GetKeyDown(KeyCode.R)) restartGame();
        }

        [ContextMenu("Increase Score")]
        public void addScore(int scoreToAdd)
        {
            playerScore = playerScore + 1;
            scoreText.text = playerScore.ToString();
        }
   
        public void restartGame() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);
   
        public void gameOver() => gameOverScreen.SetActive(true);

        public void FinishGame()
        {
            if (!isInLevelConstructor)
            {
                int indexOfCurrentLevel = GetLevelNumber() - 1;

                if (userData.indexOfLastUncoveredLevel <= indexOfCurrentLevel)
                {
                    userData.indexOfLastUncoveredLevel = indexOfCurrentLevel + 1;
                    userData.Save();
                }
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

