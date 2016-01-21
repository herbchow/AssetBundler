using UnityEditor;
using UnityEngine;

namespace Assets.Editor.TextureImportSettings
{
    public enum Platform
    {
        Default,
        Web,
        Standalone,
        iPhone,
        Android,
        FlashPlayer,
        All
    }

    public enum SettingActions
    {
        SetAll,
        SetTextureFormat,
        SetMaxTextureSize,
        SetMipMap,
        SetReadWrite,
        SetFilterMode,
        SetAniso,
        SetWrapMode,
        ClearOverrides
    }

    public class TextureImportParams
    {
        public Platform platform;
        public SettingActions SettingAction;
        public TextureImporterFormat tiFormat;
        public int maxSize;
        public bool mipMap;
        public bool readWriteMode;
        public FilterMode filterMode;
        public int anisoLevel;
        public TextureWrapMode wrapMode;
        public TextureImporterNPOTScale npotScale;

        public TextureImportParams(SettingActions oneSettingAction, Platform somePlatform = Platform.Default)
        {
            platform = somePlatform;
            SettingAction = oneSettingAction;
        }

        public void ApplyToImporter(TextureImportParams importParams, TextureImporter importer)
        {
            //Selection.objects = new Object[0]; //Clear selection (for correct data representation on GUI)

            //AssetDatabase.StartAssetEditing();
            //{
            //    switch (importParams.SettingAction)
            //    {
            //        //platform independent 
            //        case SettingActions.SetMipMap:
            //            importer.mipmapEnabled = importParams.mipMap;
            //            break;
            //        case SettingActions.SetReadWrite:
            //            importer.isReadable = importParams.readWriteMode;
            //            break;
            //        case SettingActions.SetWrapMode:
            //            importer.wrapMode = importParams.wrapMode;
            //            break;
            //        case SettingActions.SetFilterMode:
            //            importer.filterMode = importParams.filterMode;
            //            break;
            //        case SettingActions.SetAniso:
            //            importer.anisoLevel = importParams.anisoLevel;
            //            break;
            //        case SettingActions.SetAll: //Set all platform independent settings
            //            importer.textureType = TextureImporterType.Advanced;
            //            importer.mipmapEnabled = importParams.mipMap;
            //            importer.isReadable = importParams.readWriteMode;
            //            importer.wrapMode = importParams.wrapMode;
            //            importer.filterMode = importParams.filterMode;
            //            importer.anisoLevel = importParams.anisoLevel;
            //            importer.maxTextureSize = importParams.maxSize;
            //            importer.textureFormat = importParams.tiFormat;
            //            break;
            //        //platform specific props
            //        default:
            //            if (importParams.platform == Platform.Default) //default platform mode
            //            {
            //                switch (importParams.SettingAction)
            //                {
            //                    case SettingActions.SetMaxTextureSize:
            //                        importer.maxTextureSize = importParams.maxSize;
            //                        break;
            //                    case SettingActions.SetTextureFormat:
            //                        importer.textureFormat = importParams.tiFormat;
            //                        break;
            //                    default:
            //                        Debug.Log("Unhandled action on Platform.Default: " + importParams.SettingAction); //foolproof
            //                        return;
            //                }
            //            }
            //            else //override mode
            //            {
            //                if (importParams.platform != Platform.All)
            //                    importer.GetPlatformTextureSettings(importParams.platform.ToString(), out currentMaxTextureSize, out currentTIFormat);

            //                switch (importParams.SettingAction)
            //                {
            //                    case SettingActions.SetMaxTextureSize:
            //                        importer.SetPlatformTextureSettings(importParams.platform.ToString(), importParams.maxSize, currentTIFormat);
            //                        break;
            //                    case SettingActions.SetTextureFormat:
            //                        importer.SetPlatformTextureSettings(importParams.platform.ToString(), currentMaxTextureSize, importParams.tiFormat);
            //                        break;
            //                    case SettingActions.ClearOverrides:
            //                        if (importParams.platform == Platform.All)
            //                        {
            //                            ClearPlatformOverrides(Platform.Android.ToString(), importer);
            //                            ClearPlatformOverrides(Platform.Standalone.ToString(), importer);
            //                            ClearPlatformOverrides(Platform.iPhone.ToString(), importer);
            //                            ClearPlatformOverrides(Platform.Web.ToString(), importer);
            //                            ClearPlatformOverrides(Platform.FlashPlayer.ToString(), importer);
            //                        }
            //                        else
            //                            ClearPlatformOverrides(importParams.platform.ToString(), importer);
            //                        break;
            //                    default:
            //                        Debug.Log("Unhandled action on Platform." + importParams.platform.ToString() + ": " + importParams.SettingAction); //foolproof
            //                        return;
            //                }
            //            }
            //            break;
            //    }
            //    AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
            //}
            //AssetDatabase.StopAssetEditing();
            //Selection.objects = originalSelection; //Restore selection
            //Debug.Log("Textures processed: " + processingTexturesNumber);
        }

    }
}