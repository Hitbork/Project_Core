using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using FlappyBirdUpdated.LevelConstructor;
using FlappyBirdUpdated;

namespace FlappyBirdUpdated
{
    [CreateAssetMenu]
    public class CustomTile : ScriptableObject
    {
        public TileBase tile;
        public Sprite sprite;
        public string id;
        public LevelManager.Tilemaps tilemap;
        public LogicScript.Actions action;
        public bool isBouncable = false;
        public int xBounceableDirection = 0;
        public int yBounceableDirection = 0;
    }
}
