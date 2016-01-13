using System.Collections.Generic;
using AssetPipeline.DataModels;
using UnityEditor;
using UnityEngine;

namespace Assets.AssetBundleBuilder
{
    public class Unity5AssetBundleBuilder : AssetBundleBuilderBase
    {
        public override void BuildBundlePerAsset(Object[] assets, CompressionType type)
        {
            var buildList = new List<AssetBundleBuild>();
            foreach (var asset in assets)
            {
                Debug.Log("Building asset bundle for " + AssetDatabase.GetAssetPath(asset));
                buildList.Add(new AssetBundleBuild
                    {
                        assetBundleName = GenerateBundleFileName(asset, type),
                        assetNames =
                            new[]
                                {
                                    AssetDatabase.GetAssetPath(asset)
                                },
                    });
            }
            BuildPipeline.BuildAssetBundles("Assets/Output/",
                                            buildList.ToArray(),
                                            GetBuildOptions(type));
        }

        private BuildAssetBundleOptions GetBuildOptions(CompressionType type)
        {
            var options = BuildAssetBundleOptions.None;
            switch (type)
            {
                case CompressionType.Dxt:
                    break;
                case CompressionType.DxtNoBundleCompression:
                    options = options | BuildAssetBundleOptions.UncompressedAssetBundle;
                    break;
            }
            return options;
        }
    }
}
