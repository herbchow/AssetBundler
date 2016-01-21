using AssetPipeline.DataModels;

namespace Assets.CompressionSettings
{
    public class CompressionSettings : ICompressionSettings
    {
        public ImporterSettings Get(CompressionType compression)
        {
            return new ImporterSettings
                {
                    MaxPowerOfTwoSize = 1024,
                };
        }
    }
}
