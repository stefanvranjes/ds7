using System.Collections.Generic;
using System.IO;

public static class BinaryReaderExtensions
{
    public static string ReadNullTerminatedString(this BinaryReader reader)
    {
        var byteList = new List<byte>();
        byte b;

        // Read until the null terminator (0x00) or end of stream
        while (reader.BaseStream.Position < reader.BaseStream.Length && (b = reader.ReadByte()) != 0)
        {
            byteList.Add(b);
        }

        // Convert the collected bytes to a string (usually UTF8 or ASCII)
        return System.Text.Encoding.UTF8.GetString(byteList.ToArray());
    }
}
