using System;
using AssetPipeline.DataModels;
using UnityEditor;

namespace Assets.CompressionSettings
{
    public class CompressionSettings : ICompressionSettings
    {
        public ImporterSettings Get(CompressionType compression)
        {
            var settings = new ImporterSettings();
            SetPowerOfTwoSize(compression, settings);
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

        private void SetTiFormatAuto(ImporterSettings settings)
        {
            settings.AutoTransparencyFormat = true;
        }

        private bool AutoDetectFormat(CompressionType compression)
        {
            return compression == CompressionType.Dxt || compression == CompressionType.Dxt_2K;
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
                default:
                    throw new NotImplementedException("Please handle format");
            }
        }

        private static void SetPowerOfTwoSize(CompressionType compression, ImporterSettings settings)
        {
            switch (compression)
            {
                case CompressionType.Dxt:
                case CompressionType.Dxt1:
                case CompressionType.Dxt5:
                    settings.MaxPowerOfTwoSize = 1024;
                    break;
                case CompressionType.Dxt_2K:
                case CompressionType.Dxt1_2K:
                case CompressionType.Dxt5_2K:
                    settings.MaxPowerOfTwoSize = 1024*2;
                    break;
                default:
                    throw new NotImplementedException("Please handle format");
            }
        }
    }
}
