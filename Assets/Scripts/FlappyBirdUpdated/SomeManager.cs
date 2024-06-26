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

        private CustomTile GetCustomTile(GameObject tilemapGameObject, Vector3 vector)
        {
            Tilemap currentTilemap = tilemapGameObject.GetComponent<Tilemap>();
            TileBase tileBase = currentTilemap.GetTile(currentTilemap.WorldToCell(vector));
            CustomTile currentCustomTile = CustomTileManager.instance.tiles[1];

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

            foreach (ContactPoint2D contact in collision.contacts)
            {
                Vector3 contactVector = Vector3.zero;

                contactVector.x = contact.point.x - 0.01f * contact.normal.x;
                contactVector.y = contact.point.y - 0.01f * contact.normal.y;

                // Defyning logic of tile
                string collisionedTileLogic = GetCustomTile(collision.gameObject, contactVector).logic;

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

                if (collisionedTileLogic == "UD")
                {
                    unityEvent.AddListener(logic.Contact);
                    return;
                }

                if (collisionedTileLogic == "F")
                {
                    unityEvent.AddListener(logic.FinishGame);
                    return;
                }
            }
        }
    }
}
