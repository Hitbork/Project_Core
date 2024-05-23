using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;
using System.IO;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

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
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.A)) SaveLevel();
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.D)) LoadLevel(); 
    }

    void SaveLevel()
    {
        LevelData levelData = new LevelData();

        // Set up the layers in the leveldata
        foreach (var item in layers.Keys)
        {
            levelData.layers.Add(new LayerData(item));
        }

        foreach (var layerData in levelData.layers)
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
        string json = JsonUtility.ToJson(levelData, true);
        File.WriteAllText(Application.dataPath + "/testLevel.json", json);

        Debug.Log("Level was saved");
    }

    void LoadLevel()
    {
        // Load the json file to a leveldata
        string json = File.ReadAllText(Application.dataPath + "/testLevel.json");
        LevelData levelData = JsonUtility.FromJson<LevelData>(json);

        foreach (var data in levelData.layers)
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
public class LevelData
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
