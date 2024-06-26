using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

namespace FlappyBirdUpdated
{
    public class SomeManager : MonoBehaviour
    {
        public LogicScript logic;

        private void Start()
        {
            logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>();
        }

        public CustomTile GetCustomTile(GameObject tilemapGameObject, Vector3 vector)
        {
            TileBase tileBase = tilemapGameObject.GetComponent<Tilemap>().GetTile(Vector3Int.FloorToInt(vector));
            CustomTile currentCustomTile = CustomTileManager.instance.tiles[0];

            foreach (CustomTile customTile in CustomTileManager.instance.tiles)
            {
                if (customTile.tile == tileBase)
                {
                    currentCustomTile = customTile;
                }
            }

            return currentCustomTile;
        }

        public void GetEvent(Collision2D collision, out UnityEvent unityEvent)
        {
            // Defyning unity event
            unityEvent = new UnityEvent();

            // Defining contact point of collision
            ContactPoint2D contact = collision.contacts[0];

            // Defyning logic of tile
            string collisionedTileLogic = GetCustomTile(collision.gameObject, contact.point).logic;

            if (collisionedTileLogic == "D")
            {
                unityEvent.AddListener(logic.GameOver);
                return;
            } 
            
            if (collisionedTileLogic.StartsWith("BNES"))
            {
                logic.SetBounceDirection(collisionedTileLogic.Substring(4));
                unityEvent.AddListener(logic.BouncePlayer);
                return;
            }
        }
    }
}
