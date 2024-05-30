using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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
    [SerializeField] Image currentTileImage;
    [SerializeField] GameObject savingLevelUI;
    
    int _selectedTileIndex;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);

        ChangeCurrentUITileImage();
    }
    private void Update()
    {
        Vector3Int pos = currentTilemap.WorldToCell(cam.ScreenToWorldPoint(Input.mousePosition));

        if (!savingLevelUI.activeSelf)
        {
            // Moving camera options
            if (Input.GetKeyDown(KeyCode.D)) MovingCamera("right");
            if (Input.GetKeyDown(KeyCode.A)) MovingCamera("left");
            if (Input.GetKeyDown(KeyCode.W)) MovingCamera("up");
            if (Input.GetKeyDown(KeyCode.S)) MovingCamera("down");

            // Scaling camera options
            if (Input.GetKeyDown(KeyCode.F)) ScalingCamera(-2);
            if (Input.GetKeyDown(KeyCode.G)) ScalingCamera(2);
        
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                // Placing tile /w LMB
                if (Input.GetMouseButton(0)) PlaceTile(pos);
                // Deleting tile /w RMB
                if (Input.GetMouseButton(1)) DeleteTile(pos);
            }

            // Selecting next tile in the list by pressing plus btn
            if (Input.GetKeyDown(KeyCode.Plus) || Input.GetKeyDown(KeyCode.KeypadPlus)) SelectNextTile();
        
            // Selecting previous tile in the list by pressing plus btn
            if (Input.GetKeyDown(KeyCode.Minus) || Input.GetKeyDown(KeyCode.KeypadMinus)) SelectPreviousTile();
        }
    }

    public void SelectNextTile()
    {
        _selectedTileIndex++;
        if (_selectedTileIndex >= LevelManager.instance.tiles.Count) _selectedTileIndex = 0;
        ChangeCurrentUITileImage();
        Debug.Log($"Selected tile: {LevelManager.instance.tiles[_selectedTileIndex].name}");
    }
    public void SelectPreviousTile()
    {
        _selectedTileIndex--;
        if (_selectedTileIndex < 0) _selectedTileIndex = LevelManager.instance.tiles.Count - 1;
        ChangeCurrentUITileImage();
        Debug.Log($"Selected tile: {LevelManager.instance.tiles[_selectedTileIndex].name}");
    }

    public void ChangeCurrentUITileImage()
    {
        currentTileImage.sprite = LevelManager.instance.tiles[_selectedTileIndex].sprite;
    }

    public void ScalingCamera(int scalingNumber)
    {
        var cameraSize = cam.GetComponent<Camera>().orthographicSize;

        // if not (( camera size is minimal and trying to scale it down) or
        //  ( if camera size is maximum and trying to scale it up))
        if (!((cameraSize == 4 && scalingNumber < 0) ||
               (cameraSize == 40 && scalingNumber > 0)))
            cam.GetComponent<Camera>().orthographicSize += scalingNumber;
    }

    public void MovingCamera(string direction)
    {
        var cameraPosition = cam.transform.position;

        switch (direction)
        {
            case "right":
                cameraPosition.x += 5;
                break;
            case "left":
                cameraPosition.x -= 5;
                break;
            case "up":
                cameraPosition.y += 5;
                break;
            case "down":
                cameraPosition.y -= 5;
                break;
            default:
                Debug.Log("fuck");
                break;
        }

        cam.transform.position = cameraPosition;
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
