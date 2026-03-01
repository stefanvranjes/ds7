using System;
using System.Linq;
using UnityEngine;

public class FloatConverter : MonoBehaviour
{
    [TextArea(5, 10)]
    public string hex;
    [TextArea(5, 10)]
    public string floats;

    public string HexToFloatConverter(string hex)
    {
        // 1. Clean the string and convert to byte array
        string cleanHex = hex.Replace(" ", "");
        byte[] bytes = new byte[cleanHex.Length / 2];
        for (int i = 0; i < bytes.Length; i++)
        {
            bytes[i] = Convert.ToByte(cleanHex.Substring(i * 2, 2), 16);
        }

        // 2. Convert bytes to float array (4 bytes per float)
        float[] floats = new float[bytes.Length / 4];
        for (int i = 0; i < floats.Length; i++)
        {
            floats[i] = BitConverter.ToSingle(bytes, i * 4);
        }

        // Output results
        return string.Join(", ", floats.Select(f => $"{f}f"));
    }

    private void Update()
    {
        floats = HexToFloatConverter(hex);
    }
}
