using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Assets.AssetBundleBuilder
{
    public class Unity5AssetBundleBuilder : AssetBundleBuilderBase
    {
        public override void BuildBundlePerAsset(Object[] assets)
        {
            var buildList = new List<AssetBundleBuild>();
            foreach (var asset in assets)
            {
                Debug.Log("Building asset bundle for " + asset.name);
                var bundleFilename = GenerateBundleFileName(asset);
                buildList.Add(new AssetBundleBuild
                    {
                        assetBundleName = bundleFilename,
                        assetNames =
                            new[]
                                {
                                    "Assets/ToBundle/" +
                                    asset.name + ".png"
                                },
                    });
            }
            var manifest = BuildPipeline.BuildAssetBundles("Assets/Output/",
                                                           buildList.ToArray(),
                                                           BuildAssetBundleOptions.UncompressedAssetBundle | BuildAssetBundleOptions.AppendHashToAssetBundleName);
            Debug.Log("Manifest name" + manifest.name);
        }
    }
}
