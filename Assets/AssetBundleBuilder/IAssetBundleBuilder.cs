using AssetPipeline.DataModels;
using UnityEngine;

namespace Assets.AssetBundleBuilder
{
    public interface IAssetBundleBuilder
    {
        void Build(Object asset, CompressionType compression, string outputFileName);
    }
}
