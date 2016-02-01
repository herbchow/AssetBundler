using System;
using AssetPipeline.DataModels;
using UnityEditor;
using UnityEngine;

namespace Assets.CompressionSettings
{
    public class CompressionSettings : ICompressionSettings
    {
        public ImporterSettings Get(CompressionType compression, Texture2D textureAsset)
        {
            var settings = new ImporterSettings();
            SetMaxSize(compression, settings);
            SetMipmaps(compression, settings);
            SetNpotScale(compression, settings);
            if (AutoDetectFormat(compression))
            {
                SetTiFormatAuto(settings);
            }
            else
            {
                SetTiFormat(compression, settings);
            }
            return settings;
        }

        private void SetNpotScale(CompressionType compression, ImporterSettings settings)
        {
            switch (compression)
            {
                case CompressionType.NoesisLifestyleFeatures:
                case CompressionType.FilterTest:
                    settings.NPotScale = TextureImporterNPOTScale.None;
                    break;
                default:
                    settings.NPotScale = TextureImporterNPOTScale.ToLarger;
                    break;
            }
        }

        private void SetMipmaps(CompressionType compression, ImporterSettings settings)
        {
            settings.MipMapEnabled = false;
        }

        private void SetTiFormatAuto(ImporterSettings settings)
        {
            settings.AutoTransparencyFormat = true;
        }

        private bool AutoDetectFormat(CompressionType compression)
        {
            return compression == CompressionType.Dxt || compression == CompressionType.Dxt_2K ||
                   compression == CompressionType.ShelfHeroshot;
        }

        private void SetTiFormat(CompressionType compression, ImporterSettings settings)
        {
            switch (compression)
            {
                case CompressionType.Dxt1:
                case CompressionType.Dxt1_2K:
                    settings.CompressionFormat = TextureImporterFormat.DXT1;
                    break;
                case CompressionType.Dxt5:
                case CompressionType.Dxt5_2K:
                    settings.CompressionFormat = TextureImporterFormat.DXT5;
                    break;
                case CompressionType.FilterTest:
                case CompressionType.NoesisLifestyleFeatures:
                    settings.CompressionFormat = TextureImporterFormat.DXT1;
                    break;
                default:
                    throw new NotImplementedException("Please handle format");
            }
        }

        private static void SetMaxSize(CompressionType compression, ImporterSettings settings)
        {
            switch (compression)
            {
                case CompressionType.Dxt:
                case CompressionType.Dxt1:
                case CompressionType.Dxt5:
                case CompressionType.ShelfHeroshot:
                    settings.MaxSize = 1024;
                    break;
                case CompressionType.Dxt_2K:
                case CompressionType.Dxt1_2K:
                case CompressionType.Dxt5_2K:
                    settings.MaxSize = 1024*2;
                    break;
                case CompressionType.NoesisLifestyleFeatures:
                case CompressionType.FilterTest:
                    settings.MaxSize = 1024*2;
                    break;
                default:
                    throw new NotImplementedException("Please handle format");
            }
        }
    }
}
