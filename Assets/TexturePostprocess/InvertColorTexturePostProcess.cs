using Assets.Editor.TexturePostprocess;
using UnityEditor;
using UnityEngine;

namespace Assets.TexturePostprocess
{
    public class InvertColorTexturePostProcess : ITexturePostProcess
    {
        public void PostProcess(Texture2D texture)
        {
            for (var m = 0; m < texture.mipmapCount; m++)
            {
                var c = texture.GetPixels(m);
                for (var i = 0; i < c.Length; i++)
                {
                    c[i].r = 1 - c[i].r;
                    c[i].g = 1 - c[i].g;
                    c[i].b = 1 - c[i].b;
                }
                texture.SetPixels(c, m);
            }
        }

        public bool ShouldRun(Texture2D texture, AssetPostprocessor postProcessor)
        {
            return false;
        }
    }
}
