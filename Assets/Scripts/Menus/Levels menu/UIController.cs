using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using LoadSceneData.User;

namespace levelsMenu
{
    public class UIController : UIControllerClass
    {
        UserData userData = new UserData();

        [SerializeField] GameObject AccessToLevelDeniedPrefab;
        private int amountOfLevels = 10;

        private GameObject[] levelButtons;

        private void Start()
        {
            userData.Load();

            GameObject parentObject = GameObject.Find("LevelsButtons");

            levelButtons = new GameObject[parentObject.transform.childCount];

            for (int i = 0; i < levelButtons.Length; i++)
            {
                levelButtons[i] = parentObject.transform.GetChild(i).gameObject;
            }

            amountOfLevels = levelButtons.Length;

            SetActiveLevels();
        }

        private void SetActiveLevels()
        {
            // For each unactive level turning off button and initiliazing the accessDenied picture on it
            for (int i = userData.indexOfLastUncoveredLevel + 1; i < amountOfLevels; i++)
            {
                levelButtons[i].GetComponent<Button>().interactable = false;
                Instantiate(AccessToLevelDeniedPrefab, levelButtons[i].transform);
            }
        }

        public void LevelButtonClick()
        {
            // Getting scene name to open by getting three last digits "btn" from button's name down
            string sceneName = EventSystem.current.currentSelectedGameObject.name.Remove(EventSystem.current.currentSelectedGameObject.name.Length - 3);

            // Checking if level is active by getting last char of sceneName and searching for it's index in active levels array
            if (((int)char.GetNumericValue(sceneName[sceneName.Length - 1]))-1 <= userData.indexOfLastUncoveredLevel)
                SceneManager.LoadScene(sceneName);
            else
                Debug.Log("Level can't be opened");
        }
    }
}
