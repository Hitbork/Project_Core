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

            while (activeLevels[index] && index != activeLevels.Length - 1)
                index++;

            if (index == activeLevels.Length - 1)
                return;

            for (int i = index; i < levelButtons.Length; i++)
            {
                levelButtons[i].GetComponent<Button>().interactable = false;
                Instantiate(AccessToLevelDeniedPrefab, levelButtons[i].transform);
            }
        }

        public void LevelButtonClick()
        {
            string sceneName = EventSystem.current.currentSelectedGameObject.name.Remove(EventSystem.current.currentSelectedGameObject.name.Length - 3);

            if (activeLevels[((int)char.GetNumericValue(sceneName[sceneName.Length - 1]))-1])
                SceneManager.LoadScene(sceneName);
            else
                Debug.Log("Level can't be opened");
        }
    }
}
