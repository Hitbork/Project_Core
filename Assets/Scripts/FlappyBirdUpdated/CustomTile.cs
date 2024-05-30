using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class CustomTile : ScriptableObject
{
    public TileBase tile;
    public Sprite sprite;
    public string id;
    public LevelManager.Tilemaps tilemap;
}
