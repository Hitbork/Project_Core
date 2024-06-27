using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using LoadSceneData.User;
using System.Globalization;
using UnityEngine.Events;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

namespace FlappyBirdUpdated
{
    public class LogicScript : MonoBehaviour
    {
        public static LogicScript instance;

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

        public static Dictionary<int, UnityAction> tileActions = new Dictionary<int, UnityAction>()
        {
            [1] = GameOver,
            [2] = FinishGame,
            [3] = BouncePlayer
        };

        public enum Actions
        {
            GameOver = 1,
            FinishGame = 2,
            BouncePlayer = 3
        }
        
        void Start()
        {
            if (instance == null) instance = this;
            else Destroy(this);

            userData.Load();

            isInLevelConstructor = SceneManager.GetActiveScene().name == "LevelConstructor";

            SetPlayerGO();
        }

        void Update()
        {
            timer += Time.deltaTime;

            if (!isGameEnded)
                UpdateTime();

            if (!isInLevelConstructor)
                if (Input.GetKeyDown(KeyCode.R)) RestartGame();
        }

        public static void GameOver() => instance.GameOver(true);

        public static void FinishGame() => instance.FinishGame(true);

        public static void BouncePlayer() => instance.BouncePlayer(true);

        private CustomTile GetCustomTile(GameObject tilemapGameObject, Vector3 vector)
        {
            Tilemap currentTilemap = tilemapGameObject.GetComponent<Tilemap>();
            TileBase tileBase = currentTilemap.GetTile(currentTilemap.WorldToCell(vector));
            CustomTile currentCustomTile = CustomTileManager.instance.tiles[1];

            foreach (CustomTile customTile in CustomTileManager.instance.tiles)
            {
                if (customTile.tile == tileBase)
                {
                    currentCustomTile = customTile;
                }
            }

            if (tileBase == null) Debug.Log("Tilebase hasn't been found!");

            return currentCustomTile;
        }

        private int GetLevelNumber()
        {
            // This method is used while playing default levels
            // Getting current scene name
            string currentSceneName = SceneManager.GetActiveScene().name;

            // Trimming last digit from current scene name 
            return (int)char.GetNumericValue(currentSceneName[currentSceneName.Length - 1]);
        }

        private void ActivateFinishScreen()
        {
            gameFinishScreen.SetActive(true);

            // Showing current time
            currentTimeTxt.GetComponent<TMP_Text>().text = $"Current time: {System.Math.Round(timer, 2).ToString("0.00", CultureInfo.InvariantCulture)}";

            if (isInLevelConstructor)
            {
                timeRecordTxt.SetActive(false);
            }
            else
            {
                // Showing time record
                timeRecordTxt.GetComponent<TMP_Text>().text = $"Time record: {System.Math.Round(userData.timeRecordsInLevels[GetLevelNumber() - 1], 2).ToString("0.00", CultureInfo.InvariantCulture)}!";
                // Toggling off "play next level" button if it is last level
                if (GetLevelNumber() == 9)
                    playNextLevelButton.SetActive(false);
            }
        }

        private void CheckForPlayerInstance()
        {
            if (player == null)
                SetPlayerGO();
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

        public UnityAction GetTileAction(int num)
        {
            return tileActions[num];
        }

        public void BouncePlayer(bool flag)
        {
            if (isInLevelConstructor) CheckForPlayerInstance();

            player.GetComponent<Rigidbody2D>().velocity = bounceDirection;
        }

        public void Contact() => Debug.Log("There is a contact with non-killing object");

        public void FinishGame(bool flag)
        {
            if (isInLevelConstructor) CheckForPlayerInstance();

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

        public void GameOver(bool flag)
        {
            if (isInLevelConstructor) CheckForPlayerInstance();

            isGameEnded = flag;

            player.GetComponent<PlayerScript>().SetBirdUnactive();

            gameOverScreen.SetActive(flag);
        }

        public void GetEvent(Collision2D collision, out UnityEvent unityEvent)
        {
            // Defyning unity event
            unityEvent = new UnityEvent();

            // Defining contact point of collision

            foreach (ContactPoint2D contact in collision.contacts)
            {
                Vector3 contactVector = Vector3.zero;

                contactVector.x = contact.point.x - 0.01f * contact.normal.x;
                contactVector.y = contact.point.y - 0.01f * contact.normal.y;

                CustomTile customTile = GetCustomTile(collision.gameObject, contactVector);

                if (customTile.isBouncable)
                    SetBounceDirection(customTile.xBounceableDirection, customTile.yBounceableDirection);

                try
                {
                    unityEvent.AddListener(GetTileAction((int)customTile.action));
                }
                catch
                {
                    Debug.Log($"Tile {customTile.id} hasn't any action!");
                }
            }
        }

        public void OpenLevelsMenu() => SceneManager.LoadScene("LevelsMenu");

        public void OpenMainMenu() => SceneManager.LoadScene("MainMenu");

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

        public void RestartGame() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        public void SetBounceDirection(int x, int y)
        {
            float currentX = 0;
            float currentY = 0;

            if (x < 0)
                currentX = -1 * velocityOfBNES;
            else if (x > 0)
                currentX = velocityOfBNES;

            if (y < 0)
                currentY = -1 * velocityOfBNES;
            else if (y > 0)
                currentY = velocityOfBNES;

            bounceDirection = new Vector2(currentX, currentY);
        }

        public void StartGame()
        {
            isGameEnded = false;

            timer = 0;
        }

        public void UpdateTime() => scoreText.text = System.Math.Round(timer, 2).ToString("0.00", CultureInfo.InvariantCulture);
    }
}

