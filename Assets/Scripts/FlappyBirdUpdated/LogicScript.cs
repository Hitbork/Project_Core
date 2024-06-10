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
   
        public void restartGame() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);
   
        public void gameOver() => gameOverScreen.SetActive(true);
   
        public void FinishGame() =>  gameFinishScreen.SetActive(true);

        public void PlayNextLevel()
        {
            // This method is used while playing default levels
            // Getting current scene name
            string currentSceneName = SceneManager.GetActiveScene().name;
            
            // Setting next level number by trimming last digit from current scene name 
            int nextLevelNumber = (int)char.GetNumericValue(currentSceneName[currentSceneName.Length - 1]) + 1;

            // Setting next level scene name by remove last digit from current and adding string form of next number level
            string nextLevelSceneName = currentSceneName.Remove(currentSceneName.Length - 1) + nextLevelNumber.ToString();

            // Opening the scene
            SceneManager.LoadScene(nextLevelSceneName);
        }

        public void OpenLevelsMenu() => SceneManager.LoadScene("LevelsMenu");

        public void OpenMainMenu() => SceneManager.LoadScene("MainMenu");
    }
}

