using AssetPipeline.DataModels;
using UnityEditor;
using UnityEngine;

namespace Assets.AssetBundleBuilder
{
    public class Unity4AssetBundleBuilder : AssetBundleBuilderBase
    {
        public override void BuildBundlePerAsset(Object[] assets, CompressionType compression)
        {
            foreach (var asset in assets)
            {
                var bundleFilename = GenerateBundleFileName(asset, compression);
                BuildPipeline.BuildAssetBundle(asset,
                                               new[] {asset},
                                               bundleFilename,
                                               BuildAssetBundleOptions.UncompressedAssetBundle);
            }
        }
    }
}
