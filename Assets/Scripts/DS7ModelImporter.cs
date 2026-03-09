using System.Collections.Generic;
using UnityEngine;

using System.IO;

[UnityEditor.AssetImporters.ScriptedImporter(1, "ds7m")]
public class DS7ModelImporter : UnityEditor.AssetImporters.ScriptedImporter {

    public override void OnImportAsset(UnityEditor.AssetImporters.AssetImportContext ctx)
    {
        using (BinaryReader reader = new BinaryReader(File.Open(ctx.assetPath, FileMode.Open)))
        {
            reader.BaseStream.Seek(0x10, SeekOrigin.Begin);
            int OBJECTS_OFFSET = reader.ReadInt32();
            reader.BaseStream.Seek(0x32, SeekOrigin.Begin);
            int OBJECT_COUNT = reader.ReadInt16();

            for (int i = 0; i < OBJECT_COUNT; i++)
            {
                Mesh mesh = new Mesh();
                List<Vector3> vertices = new List<Vector3>();
                List<Vector3> normals = new List<Vector3>();
                List<Color> colors = new List<Color>();
                List<int> triangles = new List<int>();

                reader.BaseStream.Seek(OBJECTS_OFFSET + i * 4, SeekOrigin.Begin);
                int CUR_OBJ_OFFSET = reader.ReadInt32();
                reader.BaseStream.Seek(OBJECTS_OFFSET + CUR_OBJ_OFFSET + 0x1E, SeekOrigin.Begin);
                int ELEMENT_COUNT = reader.ReadInt16();
                reader.BaseStream.Seek(0x10, SeekOrigin.Current);
                for (int j = 0; j < ELEMENT_COUNT; j++)
                {
                    // int ANIM_INDEX = reader.ReadInt32(); for animations.
                    reader.BaseStream.Seek(0x18, SeekOrigin.Current);
                    while(true)
                    {
                        if (reader.ReadInt16() == 0x0FFF) break;
                        reader.BaseStream.Seek(0x02, SeekOrigin.Current);
                        int VERTICES_COUNT = reader.ReadInt32();
                        reader.BaseStream.Seek(0x08, SeekOrigin.Current);
                        for(int k = 0; k < VERTICES_COUNT; k++)
                        {
                            reader.BaseStream.Seek(0x10, SeekOrigin.Current);
                            Vector3 vertex = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                            vertices.Add(vertex);

                            reader.BaseStream.Seek(0x04, SeekOrigin.Current);
                            Vector3 normal = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                            normals.Add(normal);

                            reader.BaseStream.Seek(0x04, SeekOrigin.Current);
                            Color32 color = new Color32((byte)reader.ReadSingle(), (byte)reader.ReadSingle(), (byte)reader.ReadSingle(), (byte)reader.ReadSingle());
                            //color.a = 80;
                            colors.Add(color);

                            if (k >= 2)
                            {
                                triangles.Add(vertices.Count - 1);
                                triangles.Add(vertices.Count - 2);
                                triangles.Add(vertices.Count - 3);
                                triangles.Add(vertices.Count - 3);
                                triangles.Add(vertices.Count - 2);
                                triangles.Add(vertices.Count - 1);
                            }
                        }
                    }
                    reader.BaseStream.Seek(0x16, SeekOrigin.Current);
                }
                mesh.SetVertices(vertices);
                mesh.SetNormals(normals);
                mesh.SetColors(colors);
                mesh.SetTriangles(triangles, 0);

                mesh.name = Path.GetFileNameWithoutExtension(ctx.assetPath);
                if (i > 0) mesh.name += "_" + i; 
                ctx.AddObjectToAsset("ds7 model" + i, mesh);
                if (i == 0) ctx.SetMainObject(mesh);
            }
        }
    }
}
