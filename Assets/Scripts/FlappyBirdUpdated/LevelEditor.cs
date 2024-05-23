using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelEditor : MonoBehaviour
{
    public static LevelEditor instance;

    [SerializeField] Tilemap defaultTilemap;
    Tilemap currentTilemap
    {
        get
        {
            if (LevelManager.instance.layers.TryGetValue((int)LevelManager.instance.tiles[_selectedTileIndex].tilemap, out Tilemap tilemap))
            {
                return tilemap;
            } 
            else
            {
                return defaultTilemap;
            }
        }
    }
    
    // Using TileBase instead of Tile in case we may
    // use TileRule types of tiles in the future
    TileBase currentTile 
    {
        get 
        {
            return LevelManager.instance.tiles[_selectedTileIndex].tile;
        }
    }

    [SerializeField] Camera cam;
    
    int _selectedTileIndex;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
    }
    private void Update()
    {
        Vector3Int pos = currentTilemap.WorldToCell(cam.ScreenToWorldPoint(Input.mousePosition));
        
        // Placing tile /w LMB
        if (Input.GetMouseButton(0)) PlaceTile(pos);
        // Deleting tile /w RMB
        if (Input.GetMouseButton(1)) DeleteTile(pos);
        
        // Selecting next tile in the list by pressing plus btn
        if (Input.GetKeyDown(KeyCode.Plus) || Input.GetKeyDown(KeyCode.KeypadPlus)) {
            _selectedTileIndex++;
            if (_selectedTileIndex >= LevelManager.instance.tiles.Count) _selectedTileIndex = 0;
            Debug.Log($"Selected tile: {LevelManager.instance.tiles[_selectedTileIndex].name}");
        }
        
        // Selecting previous tile in the list by pressing plus btn
        if (Input.GetKeyDown(KeyCode.Minus) || Input.GetKeyDown(KeyCode.KeypadMinus)) {
            _selectedTileIndex--;
            if (_selectedTileIndex < 0) _selectedTileIndex = LevelManager.instance.tiles.Count - 1;
            Debug.Log($"Selected tile: {LevelManager.instance.tiles[_selectedTileIndex].name}");
        }
    }

    void PlaceTile(Vector3Int pos)
    {
        currentTilemap.SetTile(pos, currentTile);
    }

    void DeleteTile(Vector3Int pos)
    {
        currentTilemap.SetTile(pos, null);
    }
}
