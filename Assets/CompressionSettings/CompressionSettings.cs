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
                case CompressionType.LifestyleGaussianBlur:
                case CompressionType.LifestylePolaroid:
                case CompressionType.FilterTest:
                case CompressionType.Thumbnail140:
                case CompressionType.Thumbnail256:
                case CompressionType.ShelfHeroshot:
                case CompressionType.ShelfHeroshotBlackWhite:
                    settings.NPotScale = TextureImporterNPOTScale.None;
                    break;
                default:
                    settings.NPotScale = TextureImporterNPOTScale.None;
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
                   compression == CompressionType.ShelfHeroshot || compression == CompressionType.Thumbnail140 ||
                   compression == CompressionType.Thumbnail256 || compression == CompressionType.ShelfHeroshotBlackWhite;
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
                case CompressionType.LifestyleGaussianBlur:
                case CompressionType.LifestylePolaroid:
                    settings.CompressionFormat = TextureImporterFormat.DXT1;
                    break;
                default:
                    settings.CompressionFormat = TextureImporterFormat.DXT1;
                    break;
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
                case CompressionType.ShelfHeroshotBlackWhite:
                    settings.MaxSize = 1024;
                    break;
                case CompressionType.Dxt_2K:
                case CompressionType.Dxt1_2K:
                case CompressionType.Dxt5_2K:
                case CompressionType.NoesisLifestyleFeatures:
                case CompressionType.FilterTest:
                    settings.MaxSize = 1024*2;
                    break;
                case CompressionType.Thumbnail140:
                case CompressionType.Thumbnail256:
                    settings.MaxSize = 256;
                    break;
                default:
                    settings.MaxSize = 1024*2;
                    break;
            }
        }
    }
}
