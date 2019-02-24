using Infrastructure.Grid;
using Infrastructure.Grid.Entities;
using System.Collections.Generic;
using UnityEngine;

namespace GridSearch
{
    public class Node
    {
        public bool Populated;
        public Vector2Int Position;

        public int backCost;
        public int heuristicCost;
        public int fCost
        {
            get
            {
                return backCost + heuristicCost;
            }
        }

        public Node parent;

        public Node(bool populated, Vector2Int pos)
        {
            Populated = populated;
            Position = pos;
        }

        public Node(GridTile tile)
        {
            Position = tile.Position;
            Populated = (tile.Entity != null && tile.Entity is ResourceConductEntity);
        }
    }
}
