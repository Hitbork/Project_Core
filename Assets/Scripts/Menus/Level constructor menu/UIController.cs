using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LoadSceneData.Level;
using System.IO;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

namespace LevelConstructorMenu
{
    public class UIController : UIControllerClass
    {
        private LevelData levelData = new LevelData();
        [SerializeField] GameObject levelButtonsHolder;
        [SerializeField] Button levelButton;


        // Start is called before the first frame update
        void Start()
        {
            // Reading names of levels
            string[] pathsOfLevels = Directory.GetFiles(Application.persistentDataPath + "/LevelsOfUser", "*.json");

            // Instatiating buttons
            int yPos = 275;
            for (int i = 0; i < pathsOfLevels.Length; i++)
            {
                GameObject.Instantiate(levelButton, levelButtonsHolder.transform);
                GameObject currentLevelButton = GameObject.FindGameObjectsWithTag("LevelButton")[i];
                currentLevelButton.GetComponent<Button>().onClick.AddListener(LevelBtnClicked);
                currentLevelButton.transform.position = new Vector3(levelButtonsHolder.transform.position.x, levelButtonsHolder.transform.position.y + yPos);
                currentLevelButton.GetComponentInChildren<TMP_Text>().text = GetLevelName(pathsOfLevels[i]);
                yPos -= 75;
            }
        }

        private string GetLevelName(string path)
        {
            string nameOfJson = path.Split('\\')[1];
            return nameOfJson.Remove(nameOfJson.Length - 5, 5);
        }

        public void LevelBtnClicked()
        {
            levelData.levelName.Value = EventSystem.current.currentSelectedGameObject.GetComponentInChildren<TMP_Text>().text;
            OpenLevelConstructor();
        }

        public void OpenLevelConstructor()
        {
            levelData.Save();
            SceneManager.LoadScene("LevelConstructor");
        }
    }
}
