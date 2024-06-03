using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

namespace FlappyBirdUpdated
{
    public class LogicScript : MonoBehaviour
    {
        public int playerScore;
        public TMP_Text scoreText;
        public GameObject gameOverScreen, gameFinishScreen;
   
        [ContextMenu("Increase Score")]
        public void addScore(int scoreToAdd)
        {
            playerScore = playerScore + 1;
            scoreText.text = playerScore.ToString();
        }
   
        public void restartGame()
        {
            if (!SceneManager.GetActiveScene().name.Contains("FlappyBirdUpdated"))
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
   
        public void gameOver()
        {
            gameOverScreen.SetActive(true);
        }
   
        public void FinishGame()
        {
            gameFinishScreen.SetActive(true);
        }

        public void PlayNextLevel()
        {
            string currentSceneName = SceneManager.GetActiveScene().name;
            int currentLevelNumber = (int)char.GetNumericValue(currentSceneName[currentSceneName.Length - 1]);
            int nextLevelNumber = currentLevelNumber + 1;
            string nextLevelSceneName = currentSceneName.Remove(currentSceneName.Length - 1) + nextLevelNumber.ToString();
            SceneManager.LoadScene(nextLevelSceneName);
        }

        public void OpenLevelsMenu()
        {
            SceneManager.LoadScene("LevelsMenu");
        }

        public void OpenMainMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}

