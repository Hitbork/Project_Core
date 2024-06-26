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
        public GameObject currentTimeTxt, timeRecordTxt;

        public float velocityOfBNES = 15;

        public GameObject player;

        private bool isGameEnded = false;
        private double timer = 0;
        private Vector2 bounceDirection = new Vector2(-20, -20);

        private void Start()
        {
            userData.Load();

            isInLevelConstructor = SceneManager.GetActiveScene().name == "LevelConstructor";

            SetPlayerGO();
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
        public void UpdateTime() => scoreText.text = System.Math.Round(timer, 2).ToString("0.00", CultureInfo.InvariantCulture);

        public void RestartGame() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        public void GameOver()
        {
            isGameEnded = true;

            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>().SetBirdUnactive();

            gameOverScreen.SetActive(true);
        }

        public void FinishGame()
        {
            PlayerScript playerScript = player.GetComponent<PlayerScript>();

            if (!playerScript.birdIsAlive) return;

            isGameEnded = true;
            playerScript.SetBirdUnactive();

            if (!isInLevelConstructor)
            {
                int indexOfCurrentLevel = GetLevelNumber() - 1;

                userData.LevelFinished(indexOfCurrentLevel, timer);
            }

            ActivateFinishScreen();
        }

        private void ActivateFinishScreen()
        {
            gameFinishScreen.SetActive(true);

            // Showing current time
            currentTimeTxt.GetComponent<TMP_Text>().text = $"Current time: {System.Math.Round(timer, 2).ToString("0.00", CultureInfo.InvariantCulture)}";

            // Showing time record
            timeRecordTxt.GetComponent<TMP_Text>().text = $"Time record: {System.Math.Round(userData.timeRecordsInLevels[GetLevelNumber() - 1], 2).ToString("0.00", CultureInfo.InvariantCulture)}!";

            // Toggling off "play next level" button if it is last level
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

        public void BouncePlayer()
        {
            if (player == null)
                SetPlayerGO();

            player.GetComponent<Rigidbody2D>().velocity = bounceDirection;
        }

        public void Contact() => Debug.Log("There is a contact with non-killing object");

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

        private void SetPlayerGO()
        {
            try
            {
                player = GameObject.FindGameObjectWithTag("Player");
            }
            catch
            {
                Debug.Log("SomeManager hasn't found the playerGO");
            }
        }

        public void SetBounceDirection(string BNESLogic)
        {
            float x = 0, y = 0;

            switch (BNESLogic)
            {
                case "U":
                    y = velocityOfBNES;
                    break;
                case "UR":
                    x = velocityOfBNES;
                    y = velocityOfBNES;
                    break;
                case "R":
                    x = velocityOfBNES;
                    break;
                case "DR":
                    x = velocityOfBNES;
                    y = -1 * velocityOfBNES;
                    break;
                case "D":
                    y = -1 * velocityOfBNES;
                    break;
                case "DL":
                    x = -1 * velocityOfBNES;
                    y = -1 * velocityOfBNES;
                    break;
                case "L":
                    x = -1 * velocityOfBNES;
                    break;
                case "UL":
                    x = -1 * velocityOfBNES;
                    y = velocityOfBNES;
                    break;
            }

            bounceDirection = new Vector2(x, y);
        }

        public void OpenLevelsMenu() => SceneManager.LoadScene("LevelsMenu");

        public void OpenMainMenu() => SceneManager.LoadScene("MainMenu");
    }
}

