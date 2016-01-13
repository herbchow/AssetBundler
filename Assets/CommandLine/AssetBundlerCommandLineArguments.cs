using System;
using AssetPipeline.DataModels;

namespace Assets.CommandLine
{
    public class AssetBundlerCommandLineArguments : IAssetBundlerCommandLineArguments
    {
        public CompressionType ParseCompression()
        {
            var commandLineArguments = Environment.GetCommandLineArgs();
            var compressionTagIndex = Array.IndexOf(commandLineArguments, "-Compression");
            if (compressionTagIndex < 0)
            {
                return CompressionType.Invalid;
            }
            var compressionStr = commandLineArguments[compressionTagIndex + 1];
            var parsedEnum = (CompressionType) Enum.Parse(typeof (CompressionType), compressionStr);
            return parsedEnum;
        }
    }
}
