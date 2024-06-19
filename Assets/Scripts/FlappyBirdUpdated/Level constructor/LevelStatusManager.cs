using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace FlappyBirdUpdated
{
    namespace LevelConstructor
    {
        public class LevelStatusManager : UIControllerClass
        {
            // In case to have not issues while turning on objects
            // Level editor's turning on/off separatly
            private LevelEditor levelEditor;

            [SerializeField]
            List<GameObject> gameObjectsForEditingToSetActive,
                gameObjectsForTestingToSetActive,
                gameObjectsForEditingToInstantiate,
                gameObjectsForTestingToInstantiate,
                gameObjectsForEditingToSetUnactive;

            public bool isEditingMode { get; private set; } = true;

            private List<string> tagsOfObjectsToDestroy = new List<string>();

            private void Start()
            {
                levelEditor = GameObject.Find("Grid").GetComponent<LevelEditor>();
                TurnOnEditingMode();
            }

            // Update is called once per frame
            void Update()
            {
                if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.M) && isEditingMode) TurnOnTestableMode();
                if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.L) && !isEditingMode) TurnOnEditingMode();
            }

            public void TurnOnTestableMode()
            {
                EventSystemDeactivateModule();
                isEditingMode = false;
                DestroyingObjects();
                levelEditor.enabled = false;
                SetActiveForEditing(false);
                InstatiatingObjects(gameObjectsForTestingToInstantiate);
            }

            public void TurnOnEditingMode()
            {
                EventSystemDeactivateModule();
                isEditingMode = true;
                DestroyingObjects();
                InstatiatingObjects(gameObjectsForEditingToInstantiate);
                levelEditor.enabled = true;
                SetUnactive(gameObjectsForEditingToSetUnactive);
                SetActiveForEditing(true);
            }

            public void DestroyingObjects()
            {
                foreach (string name in tagsOfObjectsToDestroy)
                {
                    // Destroying objects finding it by tags
                    Destroy(GameObject.FindGameObjectWithTag(name));
                }

                // Clearing list for next objects
                tagsOfObjectsToDestroy.Clear();
            }

            public void InstatiatingObjects(List<GameObject> gameObjects)
            {
                foreach (GameObject currentGameObject in gameObjects)
                {
                    // Instatiating objects with exception for cameras
                    if (!currentGameObject.CompareTag("MainCamera"))
                        Instantiate(currentGameObject, new Vector3(0, 0, 0), transform.rotation);
                    else
                    {
                        Instantiate(currentGameObject, new Vector3(0, 0, -10), transform.rotation);
                        currentGameObject.GetComponent<Camera>().orthographicSize = 6;
                    }

                    // Adding tags of object so we may destroy it later
                    tagsOfObjectsToDestroy.Add(currentGameObject.tag);
                }
            }

            // Separate method to set unactive objects
            private void SetUnactive(List<GameObject> gameObjects)
            {
                foreach (GameObject currentGameObject in gameObjects)
                {
                    currentGameObject.SetActive(false);
                }
            }

            // If we want to set active objects to edit - boolean = true,
            // otherwise objects to test will be set active
            private void SetActiveForEditing(bool boolean)
            {
                foreach (GameObject currentGameObject in gameObjectsForTestingToSetActive)
                {
                    currentGameObject.SetActive(!boolean);
                }

                foreach (GameObject currentGameObject in gameObjectsForEditingToSetActive)
                {
                    currentGameObject.SetActive(boolean);
                }
            }

            public void ButtonAfterFinishClick(bool isPressedPlayAgain)
            {
                EventSystemDeactivateModule();
                TurnOnEditingMode();
                if (isPressedPlayAgain)
                    TurnOnTestableMode();
            }

            // Deactivating current input module of EventSystem
            // to be sure player will not activate input by spacebar 
            private static void EventSystemDeactivateModule()
            {
                EventSystem currentEventSystem = EventSystem.current;
                if (currentEventSystem.currentInputModule != null)
                    currentEventSystem.currentInputModule.DeactivateModule();
            }

            public void OpenLevelConstructorMenu() => SceneManager.LoadScene("LevelConstructorMenu");
        }
    }
}
