using System.Collections.Generic;
using AssetPipeline.DataModels;
using Assets.CompressionSettings;
using Assets.Editor.TextureImportSettings;
using UnityEngine;

namespace Assets.BatchTextureImporter
{
    public class BatchTextureImporter : IBatchTextureImporter
    {
        private readonly ICompressionSettings _settings;

        public BatchTextureImporter(ICompressionSettings settings)
        {
            _settings = settings;
        }

        public void Import(IList<Object> assets, CompressionType type)
        {

            ShelfTextureImportParams.BeginBatch();
            foreach (var asset in assets)
            {
                var settings = _settings.Get(type, (Texture2D) asset);
                ShelfTextureImportParams.Begin((Texture2D) asset)
                                        .SetMaxSize(settings.MaxSize)
                                        .SetNonPowerOfTwoScale(settings.NPotScale)
                                        .MipMaps(settings.MipMapEnabled)
                                        .SetFilterMode(FilterMode.Trilinear)
                                        .SetTextureFormat(settings)
                                        .End();
            }
            ShelfTextureImportParams.EndBatch();
        }
    }
}
