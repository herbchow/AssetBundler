using UnityEditor;

namespace Assets.CompressionSettings
{
    public class ImporterSettings
    {
        public int MaxPowerOfTwoSize { get; set; }
        public TextureImporterFormat CompressionFormat { get; set; }
        public bool AutoTransparencyFormat { get; set; }
    }
}
