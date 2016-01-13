using UnityEditor;
using UnityEngine;

namespace Assets.AssetBundleBuilder
{
    public class Unity4AssetBundleBuilder : AssetBundleBuilderBase
    {
        public override void BuildBundlePerAsset(Object[] assets)
        {
            foreach (var asset in assets)
            {
                var bundleFilename = GenerateBundleFileName(asset);
                BuildPipeline.BuildAssetBundle(asset,
                                               new[] {asset},
                                               bundleFilename,
                                               BuildAssetBundleOptions.UncompressedAssetBundle);
            }
        }
    }
}
