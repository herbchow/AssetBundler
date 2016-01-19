using AssetPipeline.DataModels;
using UnityEditorInternal;
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
            if (!InternalEditorUtility.inBatchMode) // Editor debug mode
            {
                return string.Format("{0}_{1}{2}", asset.name, type, AssetBundleExtension);
            }
            else
            {
                return string.Format("{0}{1}", asset.name, AssetBundleExtension);
            }
        }
    }
}
