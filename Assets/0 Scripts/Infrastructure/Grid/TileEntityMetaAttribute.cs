using System;

namespace Infrastructure.Grid.Entities
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class TileEntityMetaAttribute : Attribute
    {
        /// <summary>
        /// Binary table of tile requirements for multi tile tileentities.
        /// [1,1] is considered the center.
        /// </summary>
        private static readonly bool[][,] TileLayout = {
            //Have the 2D array be 3,3 in length and consider index 1,1 as the center.
            //Note that the y index is reversed visually vs how we display grid tiles in game.
            //Arrays are indexed and accessed via the MultiTileSize enum int values.
    
            //2x2:
            //X X _
            //X C _
            //_ _ _
            new bool[,] {
                { false, false, false },
                { true, true, false },
                { true, true, false }
            }
        };

        public bool MultiTile { get; private set; }
        public MultiTileSize Size { get; private set; }
        public bool[,] RequiredTiles {
            get {
                return GetTileLayout(Size);
            }
        }
        public string ModelPath { get; private set; }

        public TileEntityMetaAttribute(string modelPath)
        {
            ModelPath = modelPath;
        }

        public TileEntityMetaAttribute(string modelPath, MultiTileSize size) : this(modelPath)
        {
            Size = size;
            MultiTile = true;
        }

        public static bool[,] GetTileLayout(MultiTileSize size)
        {
            return TileLayout[(int)size];
        }
    }

    public enum MultiTileSize { x2 };
}
