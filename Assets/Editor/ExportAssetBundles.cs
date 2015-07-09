// C# Example
// Builds an asset bundle from the selected objects in the project view.
// Once compiled go to "Menu" -> "Assets" and select one of the choices
// to build the Asset Bundle

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public class ExportAssetBundles
{
    private const string AssetsToBundlePath = "Assets/ToBundle";
    private const string SaveBundlePath = "Assets/Output/ShelfAssetBundle.unity3d";
    private static readonly string _shelfContentPath = "C:/Users/herbert.chow/AppData/LocalLow/iQmetrix/XQ_Shelf/Assets";
    private static readonly string _toBundleFolder = Application.dataPath + "/ToBundle";

    [MenuItem("Assets/Build AssetBundle From Selection - Track dependencies")]
    private static void ExportResource()
    {
        // Bring up save panel
        var path = EditorUtility.SaveFilePanel("Save Resource", "", "New Resource", "unity3d");
        if (path.Length != 0)
        {
            // Build the resource file from the active selection.
            var selection = Selection.GetFiltered(typeof (Object), SelectionMode.DeepAssets);
            // TODO: figure out whether these are paths?
            // What happens when we drop the files into the project using a script?
            foreach (var obj in selection)
            {
                Debug.Log("Selected " + obj);
            }
            BuildPipeline.BuildAssetBundle(Selection.activeObject, selection, path, 0);
            Selection.objects = selection;
        }
    }

    private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
    {
        // Get the subdirectories for the specified directory.
        var dir = new DirectoryInfo(sourceDirName);
        var dirs = dir.GetDirectories();
        if (!dir.Exists)
        {
            throw new DirectoryNotFoundException(
                "Source directory does not exist or could not be found: "
                + sourceDirName);
        }
        // If the destination directory doesn't exist, create it. 
        if (!Directory.Exists(destDirName))
        {
            Directory.CreateDirectory(destDirName);
        }
        // Get the files in the directory and copy them to the new location.
        var files = dir.GetFiles();
        foreach (var file in files)
        {
            var temppath = Path.Combine(destDirName, file.Name);
            file.CopyTo(temppath, false);
        }
        // If copying subdirectories, copy them and their contents to new location. 
        if (copySubDirs)
        {
            foreach (var subdir in dirs)
            {
                var temppath = Path.Combine(destDirName, subdir.Name);
                DirectoryCopy(subdir.FullName, temppath, copySubDirs);
            }
        }
    }

    private static string CopyShelfContent()
    {
        var path = _toBundleFolder;
        DirectoryCopy(_shelfContentPath, path, false);
        return path;
    }

    private static void BuildBundleFromAssets(Object[] assets)
    {
        BuildPipeline.BuildAssetBundle(assets.First(), assets, SaveBundlePath, 0);
    }

    [MenuItem("Assets/Build Shelf Texture AssetBundle")]
    private static void BuildShelfTextureAssetBundle()
    {
        ImportAssetsInFolder(CopyShelfContent());
        var assets = FindAssets<Texture2D>(AssetsToBundlePath);
        BuildBundleFromAssets(assets.ToArray());
    }

    private static void ImportAssetsInFolder(string folder)
    {
        var info = new DirectoryInfo(folder);
        var fileInfo = info.GetFiles();
        foreach (var file in fileInfo)
        {
            var importAssetPath = "Assets/ToBundle/" + file.Name;
            Debug.Log("Importing asset at : " + importAssetPath);
            AssetDatabase.ImportAsset(importAssetPath);
        }
    }

    private static IList<Object> FindAssets<T>(string path)
    {
        var textureAssetGuids = AssetDatabase.FindAssets("t:" + typeof (T).Name.ToLower(), new[] {path});
        var assetList = new List<Object>();
        foreach (var guid in textureAssetGuids)
        {
            var assetPath = AssetDatabase.GUIDToAssetPath(guid);
            Debug.Log(assetPath);
            var t = Resources.LoadAssetAtPath(assetPath, typeof (T));
            if (t == null)
            {
                throw new InvalidOperationException(assetPath + " is not of type " + typeof (T));
            }
            else
            {
                assetList.Add(t);
            }
            Debug.Log("Printing asset name: " + t);
        }
        return assetList;
    }

    [MenuItem("Assets/Build AssetBundle From Selection - No dependency tracking")]
    private static void ExportResourceNoTrack()
    {
        // Bring up save panel
        var path = EditorUtility.SaveFilePanel("Save Resource", "", "New Resource", "unity3d");
        if (path.Length != 0)
        {
            // Build the resource file from the active selection.
            BuildPipeline.BuildAssetBundle(Selection.activeObject, Selection.objects, path);
        }
    }
}