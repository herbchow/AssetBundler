using System.Collections.Generic;
using AssetPipeline.DataModels;
using Assets.CompressionSettings;
using Assets.Editor.TextureImportSettings;
using UnityEditor;
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
            var settings = _settings.Get(type);
            ShelfTextureImportParams.BeginBatch();
            foreach (var asset in assets)
            {
                ShelfTextureImportParams.Begin((Texture2D) asset)
                                        .SetMaxSize(settings.MaxPowerOfTwoSize)
                                        .SetNonPowerOfTwoScale(TextureImporterNPOTScale.ToLarger)
                                        .MipMaps(true)
                                        .SetFilterMode(FilterMode.Trilinear)
                                        .SetTextureFormat(settings)
                                        .End();
            }
            ShelfTextureImportParams.EndBatch();
        }
    }
}
