using DS7.Data;

namespace DS7.Grid
{
    /// <summary>
    /// Helper to compute 6-bit connectivity masks for hex tiles.
    /// Follows HexCoordinates.Directions order: 0:E, 1:SE, 2:SW, 3:W, 4:NW, 5:NE.
    /// </summary>
    public static class ConnectivityHelper
    {
        public static int AddConnection(int mask, int direction)
        {
            return mask | (1 << direction);
        }

        public static int RemoveConnection(int mask, int direction)
        {
            return mask & ~(1 << direction);
        }

        public static int GetDirection(HexCoordinates from, HexCoordinates to)
        {
            for (int i = 0; i < 6; i++)
            {
                if (from.Neighbor(i) == to) return i;
            }
            return -1;
        }

        public static int GetOppositeDirection(int direction)
        {
            return (direction + 3) % 6;
        }
    }
}
