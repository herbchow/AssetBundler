using AssetPipeline.DataModels;
using UnityEngine;

namespace Assets.AssetBundleBuilder
{
    public abstract class AssetBundleBuilderBase
    {
        protected CompressionType DesiredCompressionType { get; set; }
        protected const string RelativeOutputAssetPath = "Assets/Output/";
        protected const string AssetBundleExtension = ".unity3d";
        public abstract void BuildBundlePerAsset(Object[] assets, CompressionType compression);

        public string GenerateBundleFileName(Object asset, CompressionType type)
        {
            return asset.name + "_" + AssetBundleExtension;
        }
    }
}
