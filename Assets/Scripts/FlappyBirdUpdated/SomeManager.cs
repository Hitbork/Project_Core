using FlappyBirdUpdated.LevelConstructor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

namespace FlappyBirdUpdated
{
    public class SomeManager : MonoBehaviour
    {
        [SerializeField] GameObject gameObj;
        [SerializeField] GameObject player;
        [SerializeField] LogicScript logic;

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
            // Check if player game object is instanced
            if (player == null)
                player = GameObject.Find("Bird");

            // Defyning unity event
            unityEvent = new UnityEvent();

            // Defyning layer that was collision with
            GameObject layer = collision.gameObject;

            // Defining contact point of collision
            ContactPoint2D contact = collision.contacts[0];

            // Defyning logic by layer
            if (layer.name == "Ground")
            {
                CustomTile currentTile = GetCustomTile(layer, contact.point);

                if (currentTile.id == "Ground")
                    unityEvent.AddListener(logic.GameOver);
            }
        }
    }
}
