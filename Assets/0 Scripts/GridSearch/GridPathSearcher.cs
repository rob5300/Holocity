using Infrastructure.Grid;
using System.Collections.Generic;
using UnityEngine;

namespace GridSearch
{
    public class GridPathSearcher
    {
        private Node[,] nodeList;
        Vector2Int StartPoint;
        Vector2Int Target;
        private int GridMax;
        public bool HasPath = false;
        public bool Complete = false;

        public GridPathSearcher(Node[,] nodeList, Vector2Int start, Vector2Int target, int gridMax)
        {
            this.nodeList = nodeList;
            StartPoint = start;
            Target = target;
            GridMax = gridMax;
        }

        public bool Start()
        {
            Node startNode = nodeList[StartPoint.x, StartPoint.y];
            Node targetNode = nodeList[Target.x, Target.y];

            List<Node> openSet = new List<Node>();
            HashSet<Node> closedSet = new HashSet<Node>();
            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                Node node = openSet[0];
                for (int i = 1; i < openSet.Count; i++)
                {
                    if (openSet[i].fCost < node.fCost || openSet[i].fCost == node.fCost)
                    {
                        if (openSet[i].heuristicCost < node.heuristicCost)
                            node = openSet[i];
                    }
                }

                openSet.Remove(node);
                closedSet.Add(node);

                if (node == targetNode)
                {
                    //We have found a route
                    HasPath = true;
                    Complete = true;
                    return HasPath;
                }
                //changed
                foreach (Node neighbour in GetNeighbours(node.Position))
                {

                    if (!neighbour.Populated || closedSet.Contains(neighbour))
                    {
                        continue;
                    }

                    int newCostToNeighbour = node.backCost + GetDistance(node, neighbour);
                    if (newCostToNeighbour < neighbour.backCost || !openSet.Contains(neighbour))
                    {
                        neighbour.backCost = newCostToNeighbour;
                        neighbour.heuristicCost = GetDistance(neighbour, targetNode);
                        neighbour.parent = node;

                        if (!openSet.Contains(neighbour))
                            openSet.Add(neighbour);
                    }
                }
            }

            //Failed to find path!
            HasPath = false;
            Complete = true;
            return HasPath;
        }

        int GetDistance(Node nodeA, Node nodeB)
        {
            int diffX = Mathf.Abs(nodeA.Position.x - nodeB.Position.x);
            int diffY = Mathf.Abs(nodeA.Position.y - nodeB.Position.y);

            return diffX + diffY;
        }

        public List<Node> GetNeighbours(Vector2Int nodePos)
        {
            List<Node> neighbours = new List<Node>();

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (Mathf.Abs(x) == Mathf.Abs(y))
                        continue;

                    int checkX = nodePos.x + x;
                    int checkY = nodePos.y + y;

                    if (checkX >= 0 && checkX <= GridMax && checkY >= 0 && checkY <= GridMax)
                    {
                        neighbours.Add(nodeList[checkX, checkY]);
                    }
                }
            }
            
            return neighbours;
        }
    }
}
