using System.Collections.Generic;
using UnityEngine;

namespace FlappyBirdUpdated
{
    public class CustomTileManager : MonoBehaviour
    {
        public static CustomTileManager instance;
        public List<CustomTile> tiles = new List<CustomTile>();

        private void Awake()
        {
            // If instance of this script is already in the scene
            // destroying creating duplicate
            if (instance == null) instance = this;
            else Destroy(this);
        }
    }
}
