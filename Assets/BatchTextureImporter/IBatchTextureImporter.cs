using System.Collections.Generic;
using AssetPipeline.DataModels;
using UnityEngine;

namespace Assets.BatchTextureImporter
{
    public interface IBatchTextureImporter
    {
        void Import(IList<Object> assets, CompressionType type);
    }
}
