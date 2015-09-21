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
    private const string Output = "Assets/Output/";
    private const string AssetBundleExtension = ".unity3d";
    private static readonly string AbsoluteToBundlePath = Application.dataPath + "/ToBundle";
    private const string ConfigSourcefolderTxt = "Config/SourceFolder.txt";

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
            Debug.Log("Copying file to " + temppath);
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

    // TODO: move this out to its own preparation step in worker role
    private static string CopySourceContent(string sourceContentPath)
    {
        var path = AbsoluteToBundlePath;
        DirectoryCopy(sourceContentPath, path, false);
        return path;
    }

    private static void BuildBundlePerAsset(Object[] assets)
    {
        foreach (var asset in assets)
        {
            var bundleFilename = Output + asset.name + AssetBundleExtension;
            BuildPipeline.BuildAssetBundle(asset, new[] {asset}, bundleFilename, 0);
        }
    }

    [MenuItem("Assets/Build Asset Bundle Per Texture ")]
    private static void BuildAssetBundlePerTexture()
    {
        ImportAssetsInFolder(CopySourceContent(GetSourceFolder()));
        var assets = FindAssets<Texture2D>(AssetsToBundlePath);
        BuildBundlePerAsset(assets.ToArray());
    }

    private static string GetSourceFolder()
    {
        var text = File.ReadAllText(Path.Combine(Application.dataPath, ConfigSourcefolderTxt));
        // Read the source folder from a config file
        Debug.Log("SourceFolder.txt contents: " + text);
        return text;
    }

    private static void ImportAssetsInFolder(string folder)
    {
        var info = new DirectoryInfo(folder);
        var fileInfo = info.GetFiles();
        if (fileInfo.Length <= 0)
        {
            Debug.Log("No files to convert from " + folder + " exiting");
        }
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
            Debug.Log("Found asset name: " + t);
        }
        return assetList;
    }
}