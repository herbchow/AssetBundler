using Assets.Editor.TexturePostprocess;
using UnityEditor;
using UnityEngine;

public class ShelfTexturePostprocessor : AssetPostprocessor
{
    private readonly ITexturePostProcess[] _postProcesses = new ITexturePostProcess[]
        {
            new PremultiplyAlphaTexturePostProcess(),
            new InvertColorTexturePostProcess(),
        };

    private void OnPostprocessTexture(Texture2D texture)
    {
        foreach (var process in _postProcesses)
        {
            if (process.ShouldRun(texture, this))
            {
                Debug.Log(string.Format("running {0} on texture {1}",
                                        process.GetType(),
                                        texture.name));
                process.PostProcess(texture);
            }
        }
    }
}