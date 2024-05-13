using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelEditor : MonoBehaviour
{
    [SerializeField] Tilemap currentTilemap;
    
    // Using TileBase instead of Tile in case we may
    // use TileRule types of tiles in the future
    [SerializeField] TileBase currentTile;

    [SerializeField] Camera cam;

    private void Update()
    {
        Vector3Int pos = currentTilemap.WorldToCell(cam.ScreenToWorldPoint(Input.mousePosition));

        if (Input.GetMouseButton(0))
        {
            PlaceTile(pos);
        }

        if (Input.GetMouseButton(1))
        {
            DeleteTile(pos);
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
