using AssetBundlerSupport;
using UnityEngine;

namespace Assets.Editor
{
    internal class AssetBundlerLogger : IAssetBundlerLogging
    {
        public void Log(string s)
        {
            Debug.Log(s);
        }
    }
}
