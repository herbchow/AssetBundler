using UnityEditor;
using UnityEngine;

namespace Assets.Editor.TexturePostprocess
{
    internal class PremultiplyAlphaTexturePostProcess : ITexturePostProcess
    {
        public void PostProcess(Texture2D texture)
        {
            var srcColors = texture.GetPixels();
            var dstColors = new Color[srcColors.Length];
            for (var i = 0; i < srcColors.Length; i++)
            {
                var srcColor = srcColors[i];
                var a = srcColor.a;
                dstColors[i] = new Color(srcColor.r*a, srcColor.g*a, srcColor.b*a, a);
            }
            texture.SetPixels(dstColors);
            texture.Apply();
        }

        public bool ShouldRun(Texture2D texture, AssetPostprocessor postProcessor)
        {
            var textureImporter = (TextureImporter) postProcessor.assetImporter;
            if (textureImporter.DoesSourceTextureHaveAlpha())
                return true;
            return false;
        }
    }
}
