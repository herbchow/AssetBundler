using AssetPipeline.DataModels;

namespace Assets.CommandLine
{
    public interface IAssetBundlerCommandLineArguments
    {
        CompressionType ParseCompression();
    }
}
