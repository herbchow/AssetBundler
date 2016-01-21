using UnityEditor;
using UnityEngine;

namespace Assets.Editor.TexturePostprocess
{
    public interface ITexturePostProcess
    {
        void PostProcess(Texture2D texture);
        bool ShouldRun(Texture2D texture,AssetPostprocessor postProcessor);
    }
}
