using FlappyBirdUpdated.LevelConstructor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace FlappyBirdUpdated
{
    public class SomeManager : MonoBehaviour
    {
        [SerializeField] GameObject gameObj;

        public void Explosion(Vector3 vector, Quaternion quaternion) =>
            Instantiate(gameObj, vector, quaternion);

        public CustomTile GetCustomTile(GameObject tilemapGameObject, Vector3 vector)
        {
            TileBase tileBase = tilemapGameObject.GetComponent<Tilemap>().GetTile(Vector3Int.FloorToInt(vector));
            CustomTile currentCustomTile = LevelManager.instance.tiles[0];

            foreach (CustomTile customTile in LevelManager.instance.tiles)
            {
                if (customTile.tile == tileBase)
                {
                    currentCustomTile = customTile;
                }
            }

            return currentCustomTile;
        }

        public bool IsDead(GameObject tilemapGameObject, Vector3 vector)
        {
            return GetCustomTile(tilemapGameObject, vector).id == "Ground";
        }
    }
}
