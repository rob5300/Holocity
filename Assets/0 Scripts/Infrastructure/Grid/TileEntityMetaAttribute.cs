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
        private static bool[][,] TileLayout = {
            //Have the 2D array be 3,3 in length and consider index 1,1 as the center.
            //Follow this format:
            //{ true, true, false },
            //{ true, CENTER, false },
            //{ false, false, false }

            //2x2
            new bool[,] {
                { true, true, false },
                { true, true, false },
                { false, false, false }
            }
            //Any others below.
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
