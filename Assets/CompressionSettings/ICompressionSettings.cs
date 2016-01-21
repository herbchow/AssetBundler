using AssetPipeline.DataModels;

namespace Assets.CompressionSettings
{
    public interface ICompressionSettings
    {
        ImporterSettings Get(CompressionType compression);
    }
}
