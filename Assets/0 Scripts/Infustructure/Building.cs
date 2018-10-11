using UnityEngine;

namespace Infustructure
{
    public class Building : MonoBehaviour
    {
        public int Length;
        public int Width;
        public int Height;

        public City ParentCity { get; internal set; }

        public bool QueryPlace(Grid.GridTile tile)
        {
            return true;
        }
    }
}
