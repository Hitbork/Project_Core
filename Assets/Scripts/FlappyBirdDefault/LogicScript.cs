using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

namespace FlappyBirdDefault
{
    public class LogicScript : UIControllerClass
    {
        public int playerScore;
        public TMP_Text scoreText;
        public GameObject gameOverScreen;
        public MainCam mainCam = new MainCam();

        // Setting borders to have access to them from other scripts
        private void Start()
        {
            mainCam.SetBorders();
        }

        // Adding score to the top left
        [ContextMenu("Increase Score")]
        public void addScore(int scoreToAdd)
        {
            playerScore = playerScore + 1;
            scoreText.text = playerScore.ToString();
        }

        // Restarting the scene
        public void restartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        // Showing game over screen
        public void gameOver()
        {
            gameOverScreen.SetActive(true);
        }

        public class MainCam
        {
            public float topBorder { get; private set; }
            public float bottomBorder { get; private set; }
            public void SetBorders()
            {
                Camera cameraComponent = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

                topBorder = cameraComponent.orthographicSize;
                bottomBorder = -1 * cameraComponent.orthographicSize;
            }
        }
    }
}
