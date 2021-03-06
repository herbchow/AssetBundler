﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AssetPipeline.DataModels;
using Assets.AssetBundleBuilder;
using Assets.BatchTextureImporter;
using Assets.CompressionSettings;
using Assets.Logger;
using TinyIoC;
using UnityEditor;
using UnityEngine;
using UnityProjectSupport;
using UnityProjectSupport.CommandLine;
using UnityProjectSupport.File;
using Object = UnityEngine.Object;

public class ExportAssetBundles
{
    private const string AssetsToBundlePath = "Assets/ToBundle";
    private static readonly string AbsoluteToBundlePath = Application.dataPath + "/ToBundle";
    private static TinyIoCContainer _container;
    private const string ConfigSourcefolderTxt = "Config/SourceFolder.txt";

    private static string CopySourceContent(string sourceContentPath)
    {
        var path = AbsoluteToBundlePath;
        _container.Resolve<IFileUtils>().DirectoryCopy(sourceContentPath, path, true);
        return path;
    }

    public static void BuildAssetBundleFromTexture()
    {
        _container = Bootstrap();
        var argsParser = _container.Resolve<ICommandLineArgumentsParser>();
        var compressionRequested = argsParser.ParseCompression();
        if (compressionRequested == CompressionType.Invalid)
        {
            throw new ArgumentException("Did you pass command line argument -Compression CompressionType ?");
        }
        var outputFileName = argsParser.ParseOutputAppFileName();
        if (string.IsNullOrEmpty(outputFileName))
        {
            throw new ArgumentException("Please specify an output file name");
        }
        BuildSingleAssetBundleFromTexture(compressionRequested, outputFileName);
    }

    private static TinyIoCContainer Bootstrap()
    {
        var container = TinyIoCContainer.Current;
        container.Register<ICommandLineArgumentsParser, CommandLineArgumentsParser>();
        container.Register<ICommandLineArgumentsProvider, UnityCommandLineArgumentsProvider>();
        container.Register<IFileUtils, FileUtils>();
        container.Register<IAssetBundlerLogger, AssetBundlerLogger>();
        container.Register<IBatchTextureImporter, BatchTextureImporter>();
        container.Register<ICompressionSettings, CompressionSettings>();
        container.Register<IAssetBundleBuilder, Unity5AssetBundleBuilder>();
        return container;
    }

    private static void BuildSingleAssetBundleFromTexture(CompressionType compression, string outputFileName)
    {
        ImportAssetsInFolder(CopySourceContent(GetSourceFolder()));
        var assets = FindAssets<Texture2D>(AssetsToBundlePath);
        _container.Resolve<IBatchTextureImporter>().Import(assets, compression);
        _container.Resolve<IAssetBundleBuilder>().Build(assets.First(), compression, outputFileName);
    }

    private static void BuildAssetBundlePerTextureHelper(CompressionType compression)
    {
        ImportAssetsInFolder(CopySourceContent(GetSourceFolder()));
        var assets = FindAssets<Texture2D>(AssetsToBundlePath);
        _container.Resolve<IBatchTextureImporter>().Import(assets, compression);
        var builder = _container.Resolve<IAssetBundleBuilder>();
        foreach (var asset in assets)
        {
            builder.Build(asset,
                          compression,
                          string.Format("{0}.{1}", asset.name, ImageCompressionHelpers.GetExtension(compression)));
        }
    }

    [MenuItem("Assets/Build Asset Bundle Per Texture Auto Dxt ")]
    private static void BuildAssetBundlePerTextureAutoDxt()
    {
        _container = Bootstrap();
        BuildAssetBundlePerTextureHelper(CompressionType.Dxt);
    }

    [MenuItem("Assets/Build Asset Bundle Per Texture Force Dxt1 ")]
    private static void BuildAssetBundlePerTextureDxt1()
    {
        _container = Bootstrap();
        BuildAssetBundlePerTextureHelper(CompressionType.Dxt1);
    }

    [MenuItem("Assets/Build Asset Bundle Per Texture Force Dxt5 ")]
    private static void BuildAssetBundlePerTextureDxt5()
    {
        _container = Bootstrap();
        BuildAssetBundlePerTextureHelper(CompressionType.Dxt5);
    }

    [MenuItem("Assets/Build Asset Bundle Per Texture Noesis Lifestyle Features")]
    private static void BuildAssetBundlePerTextureNoesisLifestyleFeatures()
    {
        _container = Bootstrap();
        BuildAssetBundlePerTextureHelper(CompressionType.NoesisLifestyleFeatures);
    }

    [MenuItem("Assets/Build Asset Bundle Per Texture Shelf Heroshot")]
    private static void BuildAssetBundlePerTextureShelfHeroshot()
    {
        _container = Bootstrap();
        BuildAssetBundlePerTextureHelper(CompressionType.ShelfHeroshot);
    }

    [MenuItem("Assets/Build Asset Bundle Per Texture Shelf Heroshot Black and White")]
    private static void BuildAssetBundlePerTextureShelfHeroshotBlackAndWhite()
    {
        _container = Bootstrap();
        BuildAssetBundlePerTextureHelper(CompressionType.ShelfHeroshotBlackWhite);
    }

    [MenuItem("Assets/Build Asset Bundle Per Texture Lifestyle Polaroid")]
    private static void BuildAssetBundlePerTextureLifestylePolaroid()
    {
        _container = Bootstrap();
        BuildAssetBundlePerTextureHelper(CompressionType.LifestylePolaroid);
    }

    [MenuItem("Assets/Build Asset Bundle Per Thumbnail 140")]
    private static void BuildAssetBundlePerTextureThumbnail140()
    {
        _container = Bootstrap();
        BuildAssetBundlePerTextureHelper(CompressionType.Thumbnail140);
    }

    [MenuItem("Assets/Build Asset Bundle Per Thumbnail 256")]
    private static void BuildAssetBundlePerTextureThumbnail256()
    {
        _container = Bootstrap();
        BuildAssetBundlePerTextureHelper(CompressionType.Thumbnail256);
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
            Debug.Log("Found asset name: " + t + " path " + assetPath);
        }
        return assetList;
    }
}