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

        // ── Direction offsets (pointy-top hex) ────────────────────────────────
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

        // ── Line Drawing ──────────────────────────────────────────────────────
        public static float Lerp(float a, float b, float t) => a + (b - a) * t;

        public static HexCoordinates Round(float x, float y, float z)
        {
            int rx = Mathf.RoundToInt(x);
            int ry = Mathf.RoundToInt(y);
            int rz = Mathf.RoundToInt(z);

            float xDiff = Mathf.Abs(rx - x);
            float yDiff = Mathf.Abs(ry - y);
            float zDiff = Mathf.Abs(rz - z);

            if (xDiff > yDiff && xDiff > zDiff)
                rx = -ry - rz;
            else if (yDiff > zDiff)
                ry = -rx - rz;
            else
                rz = -rx - ry;

            return new HexCoordinates(rx, rz);
        }

        public static HexCoordinates Lerp(HexCoordinates a, HexCoordinates b, float t)
        {
            return Round(
                Lerp(a.X + 1e-6f, b.X + 1e-6f, t), 
                Lerp(a.Y + 1e-6f, b.Y + 1e-6f, t), 
                Lerp(a.Z - 2e-6f, b.Z - 2e-6f, t)
            );
        }

        public static System.Collections.Generic.List<HexCoordinates> GetLine(HexCoordinates a, HexCoordinates b)
        {
            int N = Distance(a, b);
            var results = new System.Collections.Generic.List<HexCoordinates>();
            if (N == 0)
            {
                results.Add(a);
                return results;
            }

            for (int i = 0; i <= N; i++)
            {
                results.Add(Lerp(a, b, 1.0f / N * i));
            }
            return results;
        }

        // ── Offset conversion (even-r pointy-top) ──────────────────────────────
        public static HexCoordinates FromOffsetCoords(int col, int row)
        {
            int x = col - (row + (row & 1)) / 2;
            int z = row;
            return new HexCoordinates(x, z);
        }

        public Vector2Int ToOffsetCoords()
        {
            int row = Z;
            int col = X + (row + (row & 1)) / 2;
            return new Vector2Int(col, row);
        }

        // ── World position (pointy-top hexes) ────────────────────────────────
        /// <summary>Converts cube coordinates to world XZ position given hex size.</summary>
        public Vector3 ToWorldPosition(float hexSize)
        {
            float wx = hexSize * Mathf.Sqrt(3f) * (X + Z * 0.5f);
            float wz = hexSize * 1.5f * Z;
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
