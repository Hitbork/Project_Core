using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;
using System.IO;
using TMPro;
using LoadSceneData.Level;

public class LevelManager : MonoBehaviour
{
    private LevelData levelData = new LevelData();
    public static LevelManager instance;

    [SerializeField] TMP_Text levelNameField, levelNameText;
    [SerializeField] GameObject SavingLevelUI, loadLevelButton;

    [SerializeField] TMP_Text errorField;
    [SerializeField] GameObject errorUI;

    // Using awake, because it's always called before Start() functions
    // So we may use LevelManager.cs script to set up references between another scripts
    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);

        foreach (Tilemap tilemap in tilemaps)
        {
            foreach (Tilemaps num in System.Enum.GetValues(typeof(Tilemaps)))
            {
                if (tilemap.name == num.ToString())
                {
                    if (!layers.ContainsKey((int)num)) layers.Add((int)num, tilemap);
                }
            }
        }

        // Adding UI if name is not default
        if (!levelData.levelName.IsIncorrect())
            loadLevelButton.SetActive(true);

        // Changing UI about level
        ChangeUILevelData();
    }

    private void ChangeUILevelData()
    {
        levelNameField.text = levelData.levelName.Get();
    }

    public List<CustomTile> tiles = new List<CustomTile>();
    [SerializeField] List<Tilemap> tilemaps = new List<Tilemap>();
    public Dictionary<int, Tilemap> layers = new Dictionary<int, Tilemap>();


    public enum Tilemaps
    {
        Sky = 10,
        Background = 20,
        BounceNESUp = 25,
        BounceNESUpRight = 26,
        BounceNESRight = 27,
        BounceNESDownRight = 28,
        BounceNESDown = 29,
        BounceNESDownLeft = 30,
        BounceNESLeft = 31,
        BounceNESUpLeft = 32,
        BounceRight = 33,
        Ground = 50
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.A)) SaveLevelEvent();
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.D)) LoadLevel(); 
    }

    public void SaveLevelEvent()
    {
        if (levelData.levelName.IsIncorrect())
            SavingLevelUI.SetActive(true);
        else 
            SaveLevel();
    }

    public void AcceptingSavingLevelButtonClick()
    {
        SetLevelName(levelNameText.text);

        if (!levelData.levelName.IsIncorrect())
        {
            SaveLevel();
            SavingLevelUI.SetActive(false);
        } else
        {
            ShowError(levelData.levelName.GetErrorMessage());
        }
    }

    public void CancelingSavingLevelButtonClick()
    {
        SavingLevelUI.SetActive(false);
    }

    public void CancellingErrorButtonClick() 
    {
        errorUI.SetActive(false);
    }

    private void ShowError(string errMessage)
    {
        errorField.text = $"'{errMessage}'"; 
        errorUI.SetActive(true);
    }

    private void SetLevelName(string insertedName)
    {
        levelData.levelName.Set(insertedName);
    }

    void SaveLevel()
    {
        ChangeUILevelData();

        LevelInfo levelInfo = new LevelInfo();

        // Set up the layers in the leveldata
        foreach (var item in layers.Keys)
        {
            levelInfo.layers.Add(new LayerData(item));
        }

        foreach (var layerData in levelInfo.layers)
        {
            if (!layers.TryGetValue(layerData.layer_id, out Tilemap tilemap)) break;
            
            // Get hte bound of the tilemap
            BoundsInt bounds = tilemap.cellBounds;

            // Loop through the bounds of the tilemap
            for (int x = bounds.min.x; x < bounds.max.x; x++)
            {
                for (int y = bounds.min.y; y < bounds.max.y; y++)
                {
                    // Get the tile position
                    TileBase temp = tilemap.GetTile(new Vector3Int(x, y, 0));
                    // Find the temp tile in the custom tiles list
                    CustomTile temptile = tiles.Find(t => t.tile == temp);

                    // If there's a customtile associated /w the tile
                    if (temptile != null)
                    {
                        layerData.tiles.Add(temptile.id);
                        layerData.poses_x.Add(x);
                        layerData.poses_y.Add(y);
                    }
                }
            }

        }

        // Save the data as json
        string json = JsonUtility.ToJson(levelInfo, true);
        File.WriteAllText(Application.dataPath + $"/LevelsOfUsers/{levelData.levelName.Get()}.json", json);

        loadLevelButton.SetActive(true);

        Debug.Log("Level was saved");
    }

    void LoadLevel()
    {
        // Load the json file to a leveldata
        string json = File.ReadAllText(Application.dataPath + $"/LevelsOfUsers/{levelData.levelName.Get()}.json");
        LevelInfo levelInfo = JsonUtility.FromJson<LevelInfo>(json);

        foreach (var data in levelInfo.layers)
        {
            if (!layers.TryGetValue(data.layer_id, out Tilemap tilemap)) break;

            // Clear the tilemap
            tilemap.ClearAllTiles();

            // Place the tiles
            for (int i = 0; i < data.tiles.Count; i++)
            {
                TileBase tile = tiles.Find(t => t.id == data.tiles[i]).tile;
                if (tile) tilemap.SetTile(new Vector3Int(data.poses_x[i], data.poses_y[i], 0), tile);
            }
        }

        // Deabugging
        Debug.Log("Level loaded");
    }
} 

[System.Serializable]
public class LevelInfo
{
    public List<LayerData> layers = new List<LayerData>();
}

[System.Serializable]
public class LayerData
{
    public int layer_id;
    public List<string> tiles = new List<string>();
    public List<int> poses_x = new List<int>();
    public List<int> poses_y = new List<int>();

    public LayerData(int id)
    {
        layer_id = id;
    }
}
