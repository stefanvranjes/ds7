using System;
using UnityEngine;

namespace DS7.Grid
{
    /// <summary>
    /// Immutable cube coordinates for a hex grid.
    /// Follows the standard cube-coordinate convention: x + y + z = 0.
    /// </summary>
    [Serializable]
    public struct HexCoordinates : IEquatable<HexCoordinates>
    {
        public readonly int X;
        public readonly int Z;
        public int Y => -X - Z;

        public HexCoordinates(int x, int z)
        {
            X = x;
            Z = z;
        }

        // ── Direction offsets (flat-top hex) ──────────────────────────────────
        private static readonly HexCoordinates[] Directions = {
            new( 1, 0), new( 1,-1), new( 0,-1),
            new(-1, 0), new(-1, 1), new( 0, 1)
        };

        public static HexCoordinates Direction(int index) => Directions[index % 6];

        public HexCoordinates Neighbor(int dirIndex) => this + Direction(dirIndex);

        public HexCoordinates[] AllNeighbors()
        {
            var n = new HexCoordinates[6];
            for (int i = 0; i < 6; i++) n[i] = Neighbor(i);
            return n;
        }

        // ── Distance ──────────────────────────────────────────────────────────
        public static int Distance(HexCoordinates a, HexCoordinates b)
        {
            return (Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y) + Math.Abs(a.Z - b.Z)) / 2;
        }

        public int DistanceTo(HexCoordinates other) => Distance(this, other);

        // ── Offset conversion (even-q flat-top) ──────────────────────────────
        public static HexCoordinates FromOffsetCoords(int col, int row)
        {
            int x = col;
            int z = row - (col - (col & 1)) / 2;
            return new HexCoordinates(x, z);
        }

        public Vector2Int ToOffsetCoords()
        {
            int col = X;
            int row = Z + (X - (X & 1)) / 2;
            return new Vector2Int(col, row);
        }

        // ── World position (flat-top hexes) ──────────────────────────────────
        /// <summary>Converts cube coordinates to world XZ position given hex size.</summary>
        public Vector3 ToWorldPosition(float hexSize)
        {
            float wx = hexSize * 1.5f * X;
            float wz = hexSize * Mathf.Sqrt(3f) * (Z + X * 0.5f);
            return new Vector3(wx, 0f, wz);
        }

        // ── Operators ─────────────────────────────────────────────────────────
        public static HexCoordinates operator +(HexCoordinates a, HexCoordinates b)
            => new(a.X + b.X, a.Z + b.Z);

        public static HexCoordinates operator -(HexCoordinates a, HexCoordinates b)
            => new(a.X - b.X, a.Z - b.Z);

        public bool Equals(HexCoordinates other) => X == other.X && Z == other.Z;
        public override bool Equals(object obj) => obj is HexCoordinates h && Equals(h);
        public override int GetHashCode() => HashCode.Combine(X, Z);
        public static bool operator ==(HexCoordinates a, HexCoordinates b) => a.Equals(b);
        public static bool operator !=(HexCoordinates a, HexCoordinates b) => !a.Equals(b);
        public override string ToString() => $"({X},{Z})";
    }
}
