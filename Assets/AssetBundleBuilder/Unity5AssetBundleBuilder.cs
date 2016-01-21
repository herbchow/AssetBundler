using System.Collections.Generic;
using AssetPipeline.DataModels;
using UnityEditor;
using UnityEngine;

namespace Assets.AssetBundleBuilder
{
    public class Unity5AssetBundleBuilder : IAssetBundleBuilder
    {
        protected const string RelativeOutputAssetPath = "Assets/Output/";

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

        public void Build(Object asset, CompressionType compression, string outputFileName)
        {
            var buildList = new List<AssetBundleBuild>();
            Debug.Log("Building asset bundle for " + AssetDatabase.GetAssetPath(asset));
            buildList.Add(new AssetBundleBuild
                {
                    assetBundleName = outputFileName,
                    assetNames =
                        new[]
                            {
                                AssetDatabase.GetAssetPath(asset)
                            },
                });
            BuildPipeline.BuildAssetBundles(RelativeOutputAssetPath,
                                            buildList.ToArray(),
                                            GetBuildOptions(compression));
        }
    }
}
