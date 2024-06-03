using System.Collections;
using System.Collections.Generic;
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
    }
}

