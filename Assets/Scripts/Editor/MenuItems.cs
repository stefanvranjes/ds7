using System.IO;
using UnityEngine;
using UnityEditor;

public class MenuItems
{
    [MenuItem("Tools/Export VFS")]
    private static void ExportVFS()
    {
        string file = EditorUtility.OpenFilePanel("Open .vfs file to extract asset", "", "vfs");
        string save = EditorUtility.SaveFolderPanel("Save location", "", "");

        VFSExporter.ExtractVFS(file, save);
    }
}
