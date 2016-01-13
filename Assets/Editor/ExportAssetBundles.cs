// C# Example
// Builds an asset bundle from the selected objects in the project view.
// Once compiled go to "Menu" -> "Assets" and select one of the choices
// to build the Asset Bundle

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AssetBundlerSupport;
using Assets.AssetBundleBuilder;
using Assets.Editor;
using Assets.Editor.TextureImportSettings;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public class ExportAssetBundles
{
    private const string AssetsToBundlePath = "Assets/ToBundle";
    private static readonly string AbsoluteToBundlePath = Application.dataPath + "/ToBundle";
    private const string ConfigSourcefolderTxt = "Config/SourceFolder.txt";

    private static string CopySourceContent(string sourceContentPath)
    {
        var fileUtils = new FileUtils(new AssetBundlerLogger());
        var path = AbsoluteToBundlePath;
        fileUtils.DirectoryCopy(sourceContentPath, path, true);
        return path;
    }

    [MenuItem("Assets/Build Asset Bundle Per Texture ")]
    private static void BuildAssetBundlePerTexture()
    {
        ImportAssetsInFolder(CopySourceContent(GetSourceFolder()));
        var assets = FindAssets<Texture2D>(AssetsToBundlePath);
        ShelfTextureImportParams.BeginBatch();
        foreach (var asset in assets)
        {
            ShelfTextureImportParams.Begin((Texture2D) asset)
                                    .SetMaxSize(512)
                                    .SetNonPowerOfTwoScale(TextureImporterNPOTScale.ToLarger)
                                    .MipMaps(true)
                                    .SetFilterMode(FilterMode.Trilinear)
                                    .End();
        }
        ShelfTextureImportParams.EndBatch();
        var builder = new Unity5AssetBundleBuilder();
        builder.BuildBundlePerAsset(assets.ToArray());
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
            var t = AssetDatabase.LoadAssetAtPath(assetPath, typeof (T));
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