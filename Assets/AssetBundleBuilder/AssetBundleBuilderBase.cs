using UnityEngine;

namespace Assets.AssetBundleBuilder
{
    public abstract class AssetBundleBuilderBase : IAssetBundleBuilder
    {
        protected const string RelativeOutputAssetPath = "Assets/Output/";
        protected const string AssetBundleExtension = ".unity3d";
        public abstract void BuildBundlePerAsset(Object[] assets);

        protected string GenerateBundleFileName(Object asset)
        {
            var bundleFilename = asset.name + AssetBundleExtension;
            return bundleFilename;
        }
    }
}
