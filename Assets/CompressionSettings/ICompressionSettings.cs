using AssetPipeline.DataModels;
using UnityEngine;

namespace Assets.CompressionSettings
{
    public interface ICompressionSettings
    {
        ImporterSettings Get(CompressionType compression, Texture2D textureAsset);
    }
}
