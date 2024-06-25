using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using System.Linq;
using System.IO;
using TMPro;
using LoadSceneData.Level;
using System;

namespace FlappyBirdUpdated
{
    namespace LevelConstructor
    {
        public class LevelManager : MonoBehaviour
        {
            private LevelData levelData = new LevelData();
            public static LevelManager instance;
            [SerializeField] bool isInConstructor;
            [SerializeField] GameObject grid;

            [SerializeField] TMP_Text levelNameField;
            [SerializeField] TMP_InputField levelNameTextInputField;
            [SerializeField] GameObject SavingLevelUI, loadLevelButton;

            [SerializeField] TMP_Text errorField;
            [SerializeField] GameObject errorUI;

            [SerializeField] Image selectedTileImage; 

            public List<CustomTile> tiles = new List<CustomTile>();
            [SerializeField] List<Tilemap> tilemaps = new List<Tilemap>();
            public Dictionary<int, Tilemap> layers = new Dictionary<int, Tilemap>();

            // Enum used to define order of tilemaps
            public enum Tilemaps
            {
                Sky = 10,
                Background = 20,
                FinishLine = 21,
                BounceNESUp = 25,
                BounceNESUpRight = 26,
                BounceNESRight = 27,
                BounceNESDownRight = 28,
                BounceNESDown = 29,
                BounceNESDownLeft = 30,
                BounceNESLeft = 31,
                BounceNESUpLeft = 32,
                BounceRight = 33,
                TouchableGround = 49,
                Ground = 50
            }

            // Using awake, because it's always called before Start() functions
            // So we may use LevelManager.cs script to set up references between another scripts
            private void Awake()
            {
                // If instance of this script is already in the scene
                // destroying creating duplicate
                if (instance == null) instance = this;
                else Destroy(this);

                if (isInConstructor)
                {
                    tiles = CustomTileManager.instance.tiles;

                    // For each seriliazable tilemap
                    foreach (Tilemap tilemap in tilemaps)
                    {
                        // Searching in pairs of enum tilemaps
                        foreach (Tilemaps num in System.Enum.GetValues(typeof(Tilemaps)))
                        {
                            // If pair has been found
                            if (tilemap.name == num.ToString())
                            {
                                // Adding the layer to dictionary with its order number (defined in enum Tilemaps)
                                if (!layers.ContainsKey((int)num)) layers.Add((int)num, tilemap);
                            }
                        }
                    }

                    // Loading leveldata from file
                    levelData.Load();

                    // Adding UI if name is not default
                    if (!levelData.levelName.isIncorrect)
                    {
                        LoadLevel();
                        loadLevelButton.SetActive(true);
                    }

                    // Changing UI about level
                    ChangeUILevelData();
                } else
                {
                    Tilemap[] tempTilemaps = grid.GetComponentsInChildren<Tilemap>();

                    Debug.Log($"Amount of tilemaps founded: {tempTilemaps.Length}");

                    foreach (Tilemap tilemap in grid.GetComponentsInChildren<Tilemap>())
                    {
                        tilemaps.Add(tilemap);
                    }

                    // For each seriliazable tilemap
                    foreach (Tilemap tilemap in tilemaps)
                    {
                        // Searching in pairs of enum tilemaps
                        foreach (Tilemaps num in Enum.GetValues(typeof(Tilemaps)))
                        {
                            // If pair has been found
                            if (tilemap.name == num.ToString())
                            {
                                // Adding the layer to dictionary with its order number (defined in enum Tilemaps)
                                if (!layers.ContainsKey((int)num)) layers.Add((int)num, tilemap);
                            }
                        }
                    }
                }
            }

            private void Update()
            {
                if (isInConstructor)
                {
                    if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.A)) SaveLevelEvent();
                    if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.D)) LoadLevel(); 
                }
            }
            public void AcceptingSavingLevelButtonClick()
            {
                // Setting levelname.Value string as player typed in the window
                levelData.levelName.Value = levelNameTextInputField.text;


                if (!levelData.levelName.isIncorrect)
                {
                    SaveLevel();
                    SavingLevelUI.SetActive(false);
                } else
                {
                    ShowError(levelData.levelName.errorMessage);
                }
            }

            private void ChangeUILevelData()
            {
                levelNameField.text = levelData.levelName.Value;
            }

            public void CancelingSavingLevelButtonClick()
            {
                SavingLevelUI.SetActive(false);
            }

            public void CancellingErrorButtonClick() 
            {
                errorUI.SetActive(false);
            }

            public void SaveLevelEvent()
            {
                if (levelData.levelName.isIncorrect)
                    SavingLevelUI.SetActive(true);
                else 
                    SaveLevel();
            }

            private void ShowError(string errMessage)
            {
                errorField.text = $"'{errMessage}'"; 
                errorUI.SetActive(true);
            }

            void LoadLevel()
            {
                LevelInfo levelInfo = new LevelInfo();

                try
                {
                    // Load the json file to a leveldata
                    string pathOfSavingDirectory = Application.persistentDataPath + "/LevelsOfUser";
                    string json = File.ReadAllText(pathOfSavingDirectory + $"/{levelData.levelName.Value}.json");
                    levelInfo = JsonUtility.FromJson<LevelInfo>(json);
                    Debug.Log("File found succesfully");
                }
                catch (Exception ex)
                {
                    Debug.Log("File hasn't been found");
                    Debug.Log(ex.Message);
                    return;
                }

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

                Debug.Log("Level loaded");
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

                string pathOfSavingDirectory = Application.persistentDataPath + "/LevelsOfUser";
                if (!Directory.Exists(pathOfSavingDirectory))
                    Directory.CreateDirectory(pathOfSavingDirectory);

                // Save the data as json
                string json = JsonUtility.ToJson(levelInfo, true);
                File.WriteAllText(pathOfSavingDirectory + $"/{levelData.levelName.Value}.json", json);

                loadLevelButton.SetActive(true);
                levelData.Save();

                Debug.Log("Level was saved");
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
    }
}