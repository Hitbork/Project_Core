using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStatusManager : MonoBehaviour
{
    private LevelEditor levelEditor;

    [SerializeField]
    List<GameObject> gameObjectsForEditingToSetActive, gameObjectsForTestingToSetActive, gameObjectsForEditingToInstantiate, gameObjectsForTestingToInstantiate;

    private List<string> tagsOfObjectsToDestroy = new List<string>();

    private void Awake()
    {
        levelEditor = GameObject.Find("Grid").GetComponent<LevelEditor>();
        TurnOnEditingMode();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.M)) TurnOnTestableMode();
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.L)) TurnOnEditingMode();
    }
    public void TurnOnTestableMode()
    {
        DestroyingObjects();
        levelEditor.enabled = false;
        SetActiveForEditing(false);
        InstatiatingObjects(gameObjectsForTestingToInstantiate);
    }

    public void TurnOnEditingMode()
    {
        DestroyingObjects();
        InstatiatingObjects(gameObjectsForEditingToInstantiate);
        levelEditor.enabled = true;
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
                Instantiate(currentGameObject, new Vector3(0, 0, -1), transform.rotation);
            tagsOfObjectsToDestroy.Add(currentGameObject.tag);
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
}
