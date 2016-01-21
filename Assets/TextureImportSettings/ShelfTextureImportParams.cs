using UnityEditor;
using UnityEngine;

namespace Assets.Editor.TextureImportSettings
{
    /// <summary>
    ///     Texture must first be imported, once imported, we can
    ///     Call AssetDatabase.StartAssetEditing()
    ///     then grab the texture importer via asset path
    ///     set some settings
    ///     end
    /// </summary>
    public class ShelfTextureImportParams
    {
        private readonly string _texturePath;
        private Texture2D _texture;
        private readonly TextureImporter _textureImporter;

        private ShelfTextureImportParams(Texture2D texture)
        {
            _texture = texture;
            _texturePath = AssetDatabase.GetAssetPath(texture);
            _textureImporter = AssetImporter.GetAtPath(_texturePath) as TextureImporter;
        }

        public static ShelfTextureImportParams Begin(Texture2D texture)
        {
            return new ShelfTextureImportParams(texture);
        }

        public ShelfTextureImportParams SetNonPowerOfTwoScale(TextureImporterNPOTScale scale)
        {
            _textureImporter.textureType = TextureImporterType.Advanced;
            _textureImporter.npotScale = scale;
            return this;
        }

        public static void BeginBatch()
        {
            AssetDatabase.StartAssetEditing();
        }

        public static void EndBatch()
        {
            AssetDatabase.StopAssetEditing();
        }

        public void End()
        {
            AssetDatabase.ImportAsset(_texturePath, ImportAssetOptions.ForceUpdate);
        }

        public ShelfTextureImportParams SetMaxSize(int maxSize)
        {
            _textureImporter.textureType = TextureImporterType.Advanced;
            _textureImporter.maxTextureSize = maxSize;
            return this;
        }

        public ShelfTextureImportParams MipMaps(bool enabled)
        {
            _textureImporter.mipmapFilter = TextureImporterMipFilter.BoxFilter;
            _textureImporter.mipmapEnabled = enabled;
            return this;
        }

        public ShelfTextureImportParams SetFilterMode(FilterMode mode)
        {
            _textureImporter.filterMode = mode;
            return this;
        }
    }
}
