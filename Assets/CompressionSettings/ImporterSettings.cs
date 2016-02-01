using UnityEditor;

namespace Assets.CompressionSettings
{
    public class ImporterSettings
    {
        public int MaxSize { get; set; }
        public TextureImporterFormat CompressionFormat { get; set; }
        public bool AutoTransparencyFormat { get; set; }
        public bool AllowNpot { get; set; }
        public bool MipMapEnabled { get; set; }
        public TextureImporterNPOTScale NPotScale { get; set; }
    }
}
