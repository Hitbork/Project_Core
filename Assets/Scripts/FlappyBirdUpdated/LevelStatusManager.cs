using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LevelStatusManager : MonoBehaviour
{
    private LevelEditor levelEditor;

    [SerializeField]
    List<GameObject> gameObjectsForEditingToSetActive,
        gameObjectsForTestingToSetActive,
        gameObjectsForEditingToInstantiate,
        gameObjectsForTestingToInstantiate,
        gameObjectsForEditingToSetUnactive;

    public bool isEditingMode { get; private set; } = true; 

    private List<string> tagsOfObjectsToDestroy = new List<string>();

    private void Awake()
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
            Destroy(GameObject.FindGameObjectWithTag(name));
        }
        
        tagsOfObjectsToDestroy.Clear();
    }

    public void InstatiatingObjects(List<GameObject> gameObjects)
    {
        foreach (GameObject currentGameObject in gameObjects)
        {
            if (!currentGameObject.CompareTag("MainCamera"))
                Instantiate(currentGameObject, new Vector3(0, 0, 0), transform.rotation);
            else
                Instantiate(currentGameObject, new Vector3(0, 0, -10), transform.rotation);
            tagsOfObjectsToDestroy.Add(currentGameObject.tag);
        }
    }

    private void SetUnactive(List<GameObject> gameObjects)
    {
        foreach (GameObject currentGameObject in gameObjects)
        {
            currentGameObject.SetActive(false);
        }
    }

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

    private static void EventSystemDeactivateModule()
    {
        EventSystem currentEventSystem = EventSystem.current;
        if (currentEventSystem.currentInputModule != null)
            currentEventSystem.currentInputModule.DeactivateModule();
    }
}
