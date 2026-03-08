using UnityEngine;

using System.IO;

[UnityEditor.AssetImporters.ScriptedImporter(1, "tnkd")]
public class DS7SpriteImporter : UnityEditor.AssetImporters.ScriptedImporter {

    public override void OnImportAsset(UnityEditor.AssetImporters.AssetImportContext ctx)
    {
        using (BinaryReader reader = new BinaryReader(File.Open(ctx.assetPath, FileMode.Open)))
        {
            int START_TNKD = reader.ReadInt32();
            reader.BaseStream.Seek(START_TNKD + 0x08, SeekOrigin.Begin);
            int TEX_COUNT = reader.ReadInt32();
            for (int i = 0; i < TEX_COUNT; i++)
            {
                reader.BaseStream.Seek(i * 4 + 4, SeekOrigin.Begin);
                int TEX_OFFSET = reader.ReadInt32();
                reader.BaseStream.Seek(TEX_OFFSET+0x10, SeekOrigin.Begin);
                int pixelSectionOffset = reader.ReadInt32() + TEX_OFFSET + 0x10;
                int colorSectionOffset = TEX_OFFSET+0x30;
                reader.BaseStream.Seek(TEX_OFFSET+0x20, SeekOrigin.Begin);
                int palleteType = reader.ReadByte();

                reader.BaseStream.Seek(pixelSectionOffset, SeekOrigin.Begin);
                reader.BaseStream.Seek(0x08, SeekOrigin.Current);
                int width = (palleteType == 0x40) ? reader.ReadInt16() * 2 : reader.ReadInt16() * 4;
                int height = reader.ReadInt16();
                Texture2D texture = new Texture2D(width, height);

                reader.BaseStream.Seek(pixelSectionOffset + 0x20, SeekOrigin.Begin);
                bool hiDone = false;
                bool liDone = false;
                for (int y = texture.height; y > 0; y--)
                {
                    for (int x = 0; x < texture.width; x++)
                    {
                        int colorPaletteIndex = reader.ReadByte();
                        int currentPixelPosition = (int)reader.BaseStream.Position;
                        if (palleteType == 0x04)
                        {
                            if (!hiDone)
                            {
                                colorPaletteIndex = colorPaletteIndex & 0x0F;
                                currentPixelPosition = (int)reader.BaseStream.Position - 0x01;
                                hiDone = true;
                                liDone = false;
                            }
                            else if (!liDone)
                            {
                                colorPaletteIndex = colorPaletteIndex >> 0x04;
                                liDone = true;
                                hiDone = false;
                            }
                        }
                        reader.BaseStream.Seek(colorSectionOffset + colorPaletteIndex * 0x04, SeekOrigin.Begin);
                        byte red = reader.ReadByte();
                        byte green = reader.ReadByte();
                        byte blue = reader.ReadByte();
                        byte alpha = GetAlpha(reader.ReadByte());
                        Color color = new Color32(red, green, blue, alpha);
                        texture.SetPixel(x, y-1, color);
                        reader.BaseStream.Seek(currentPixelPosition, SeekOrigin.Begin);
                    }
                }
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
                sprite.name = Path.GetFileNameWithoutExtension(ctx.assetPath);
                texture.name = Path.GetFileNameWithoutExtension(ctx.assetPath);
                if (TEX_COUNT > 1)
                {
                    sprite.name += "_" + i + "_sprite";
                    texture.name += "_" + i + "_tex";
                }

                texture.filterMode = FilterMode.Point;
                texture.wrapMode = TextureWrapMode.Repeat;
                texture.anisoLevel = 0;
                texture.Apply();

                byte[] pngBytes = texture.EncodeToPNG();
                string pngFileName = TEX_COUNT > 1 ? $"{texture.name}.png" : $"{Path.GetFileNameWithoutExtension(ctx.assetPath)}.png";
                string pngPath = Path.Combine(Path.GetDirectoryName(ctx.assetPath), pngFileName);
                File.WriteAllBytes(pngPath, pngBytes);

                ctx.AddObjectToAsset("ds7 texture" + i, texture);
                ctx.AddObjectToAsset("ds7 sprite" + i, sprite);
                if (i == 0) ctx.SetMainObject(sprite);
            }
        }
    }

    private byte GetAlpha(short alpha)
    {
        alpha *= 0x02;
        if (alpha >= 0x0100) alpha = 0x00FF;
        return (byte)alpha;
    }
}
