using System.IO;
using System.Collections;
using System.Collections.Generic;

public static class VFSExporter
{
    public static void ExtractVFS(string inFile, string outDir)
    {
        using (BinaryReader reader = new BinaryReader(File.Open(inFile, FileMode.Open)))
        {
            const string identifier = "VFS Ea Co,LTD";
            string currentPath = "";

            if (reader.ReadNullTerminatedString() == identifier)
            {
                long pos = 0;
                int breakCount = 0;
                reader.BaseStream.Seek(0x800, SeekOrigin.Begin);

                while (true)
                {
                    string dir = reader.ReadNullTerminatedString();
                    if (string.IsNullOrEmpty(dir))
                    {
                        breakCount++;
                        if (breakCount == 0x80) break;
                    }
                    else if (dir == "/") continue;
                    else if (dir[0] == '/')
                    {
                        currentPath = dir + "/";
                        Directory.CreateDirectory(outDir + currentPath);
                    }
                    else
                    {
                        int size = reader.ReadInt32();
                        int offset = reader.ReadInt32();
                        pos = reader.BaseStream.Position;
                        reader.BaseStream.Seek(offset, SeekOrigin.Begin);
                        string afs = "AFS";
                        
                        if (reader.ReadNullTerminatedString() == afs)
                        {
                            int length = reader.ReadInt32();
                            long pos2 = reader.BaseStream.Position;

                            for (int i = 0; i < length; i++)
                            {
                                reader.BaseStream.Seek(pos2 + i * 8, SeekOrigin.Begin);
                                int offset2 = reader.ReadInt32();
                                int size2 = reader.ReadInt32();
                                reader.BaseStream.Seek(pos2 + offset2 - 8, SeekOrigin.Begin);
                                byte[] buffer = reader.ReadBytes(size2);
                                File.WriteAllBytes(outDir + currentPath + Path.GetFileNameWithoutExtension(dir) 
                                    + "_" + i.ToString("D2") + ".000", buffer);
                            }
                        }
                        else
                        {
                            reader.BaseStream.Seek(offset, SeekOrigin.Begin);
                            byte[] buffer = reader.ReadBytes(size);
                            File.WriteAllBytes(outDir + currentPath + dir, buffer);
                        }

                        reader.BaseStream.Seek(pos, SeekOrigin.Begin);
                    }
                }
            }
        }
    }
}
