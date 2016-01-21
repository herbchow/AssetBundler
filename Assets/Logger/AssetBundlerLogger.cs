using UnityEngine;
using UnityProjectSupport;

namespace Assets.Logger
{
    public class AssetBundlerLogger : IAssetBundlerLogger
    {
        public void Log(string s)
        {
            Debug.Log(s);
        }
    }
}
