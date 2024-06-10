using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace levelsMenu
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] GameObject AccessToLevelDeniedPrefab;
        
        // This variable is temporaly setted in case of testing
        private bool[] activeLevels;

        private GameObject[] levelButtons;

        private void Start()
        {
            GameObject parentObject = GameObject.Find("LevelsButtons");

            levelButtons = new GameObject[parentObject.transform.childCount];

            for (int i = 0; i < levelButtons.Length; i++)
            {
                levelButtons[i] = parentObject.transform.GetChild(i).gameObject;
            }

            SetArrayOfActiveLevels(4);
            SetActiveLevels();
        }

        // This method is temporaly used in case of testing
        private void SetArrayOfActiveLevels(int amountOfOpenLevels)
        {
            activeLevels = new bool[levelButtons.Length];

            for (int i = 0; i < amountOfOpenLevels; i++)
                activeLevels[i] = true;

            for (int i = amountOfOpenLevels; i < activeLevels.Length; i++)
                activeLevels[i] = false;
        }

        private void SetActiveLevels()
        {
            int index = 0;

            // Searching the index of first unactive level
            while (activeLevels[index] && index != activeLevels.Length - 1)
                index++;

            // Returning from method in case there are no unactive levels
            if (index == activeLevels.Length - 1)
                return;

            // For each unactive level turning off button and initiliazing the accessDenied picture on it
            for (int i = index; i < levelButtons.Length; i++)
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
            if (activeLevels[((int)char.GetNumericValue(sceneName[sceneName.Length - 1]))-1])
                SceneManager.LoadScene(sceneName);
            else
                Debug.Log("Level can't be opened");
        }
    }
}
