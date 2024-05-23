using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStatusManager : MonoBehaviour
{
    public GameObject player;
    private LevelEditor levelEditor;

    [SerializeField]
    List<GameObject> gameObjectsForEditingToSetActive, gameObjectsForTestingToSetActive, gameObjectsForEditingToInstantiate, gameObjectsForTestingToInstantiate;

    private List<string> namesOfObjectsToDestroy;

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
        InstatiatingObjects(gameObjectsForTestingToInstantiate);
        levelEditor.enabled = false;
        SetActiveForEditing(false);
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
        foreach (string name in namesOfObjectsToDestroy)
        {
            Destroy(GameObject.Find(name));
        }

        namesOfObjectsToDestroy.Clear();
    }

    public void InstatiatingObjects(List<GameObject> gameObjects)
    {
        foreach (GameObject gamaObject in gameObjects)
        {
            Instantiate(gamaObject, new Vector3(0, 0, 0), transform.rotation);
            namesOfObjectsToDestroy.Add(gameObject.name);
        }
    }

    private void SetActiveForEditing(bool boolean)
    {
        foreach (GameObject gameObject in gameObjectsForTestingToSetActive)
        {
            gameObject.SetActive(!boolean);
        }

        foreach (GameObject gameObject in gameObjectsForEditingToSetActive)
        {
            gameObject.SetActive(boolean);
        }
    }
}
