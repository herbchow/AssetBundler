using UnityEngine;
using UnityEditor;

// /////////////////////////////////////////////////////////////////////////////////////////////////////////
//
// Batch Texture import settings modifier.
//
// Modifies all selected textures in the project window and applies the requested modification on the 
// textures. Idea was to have the same choices for multiple files as you would have if you open the 
// import settings of a single texture. Put this into Assets/Editor and once compiled by Unity you find
// the new functionality in HelpTools -> Texture import settings. Enjoy! :-)
// 
// Based on the great work of benblo in this thread: 
// http://forum.unity3d.com/viewtopic.php?t=16079&start=0&postdays=0&postorder=asc&highlight=textureimporter
// 
// Developed by Martin Schultz, Decane in August 2009
// e-mail: ms@decane.net

// Extended by jite in September 2012
// + 5 platforms overrides support (set/clear)
// + mipmap mode, read/write mode, filter mode, aniso level, wrap mode
// + 1 predefined params complex set
// * textures formats for Unity 3.5.5
// * saves selection
// e-mail: jite.gs@gmail.com
//
// /////////////////////////////////////////////////////////////////////////////////////////////////////////
public class ChangeTextureImportSettings : ScriptableObject
{
    static int currentMaxTextureSize;
    static TextureImporterFormat currentTIFormat;
    static string logTitle = "ChangeTextureImportSettings. ";

    //--- Internal Class ------------------------

    class TextureImportParams
    {
        public Platform platform;
        public Actions action;
        public TextureImporterFormat tiFormat;
        public int maxSize;
        public bool mipMap;
        public bool readWriteMode;
        public FilterMode filterMode;
        public int anisoLevel;
        public TextureWrapMode wrapMode;

        public TextureImportParams(Actions oneAction, Platform somePlatform = ChangeTextureImportSettings.Platform.Default)
        {
            platform = somePlatform;
            action = oneAction;
        }
    }

    //--- Enums ---------------------------------

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

    enum Actions
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

    [MenuItem("HelpTools/Texture import settings/Set predefined params (like defaults, see in script)")]
    static void SelectedSetDefaults()
    {
        TextureImportParams tiParams = new TextureImportParams(Actions.SetAll, Platform.Default);
        tiParams.anisoLevel = 1;
        tiParams.filterMode = FilterMode.Bilinear;
        tiParams.maxSize = 4096;
        tiParams.mipMap = false;
        tiParams.readWriteMode = true;
        tiParams.tiFormat = TextureImporterFormat.RGBA32;
        tiParams.wrapMode = TextureWrapMode.Clamp;
        Debug.Log(System.String.Format(
          "{0} Set predefined params @ {1} platform: TextureImporterFormat {2}, MaxTextureSize {3}, MipMap {4}, RWMode {5}, FilterMode {6}, AnisoLevel {7}, WrapMode {8}",
          logTitle, tiParams.platform, tiParams.tiFormat, tiParams.maxSize, tiParams.mipMap, tiParams.readWriteMode,
          tiParams.filterMode, tiParams.anisoLevel, tiParams.wrapMode));
        SelectedChangeAnyPlatformSettings(tiParams);
    }


    //--- ChangeTextureFormat -------------------

    //--- ChangeTextureFormat. Default

    [MenuItem("HelpTools/Texture import settings/Change Texture Format/Default/RGB Compressed DXT1")]
    static void ChangeTextureFormat_Default_DXT1()
    {
        ChangeTextureFormat(TextureImporterFormat.DXT1);
    }

    [MenuItem("HelpTools/Texture import settings/Change Texture Format/Default/RGB Compressed DXT5")]
    static void ChangeTextureFormat_Default_DXT5()
    {
        ChangeTextureFormat(TextureImporterFormat.DXT5);
    }

    [MenuItem("HelpTools/Texture import settings/Change Texture Format/Default/RGB Compressed ETC 4 bit")]
    static void ChangeTextureFormat_Default_ETC_RGB4()
    {
        ChangeTextureFormat(TextureImporterFormat.ETC_RGB4);
    }

    [MenuItem("HelpTools/Texture import settings/Change Texture Format/Default/RGB Compressed PVRTC 2 bit")]
    static void ChangeTextureFormat_Default_PVRTC_RGB2()
    {
        ChangeTextureFormat(TextureImporterFormat.PVRTC_RGB2);
    }

    [MenuItem("HelpTools/Texture import settings/Change Texture Format/Default/RGBA Compressed PVRTC 2 bit")]
    static void ChangeTextureFormat_Default_PVRTC_RGBA2()
    {
        ChangeTextureFormat(TextureImporterFormat.PVRTC_RGBA2);
    }

    [MenuItem("HelpTools/Texture import settings/Change Texture Format/Default/RGB Compressed PVRTC 4 bit")]
    static void ChangeTextureFormat_Default_PVRTC_RGB4()
    {
        ChangeTextureFormat(TextureImporterFormat.PVRTC_RGB4);
    }

    [MenuItem("HelpTools/Texture import settings/Change Texture Format/Default/RGBA Compressed PVRTC 4 bit")]
    static void ChangeTextureFormat_Default_PVRTC_RGBA4()
    {
        ChangeTextureFormat(TextureImporterFormat.PVRTC_RGBA4);
    }

    [MenuItem("HelpTools/Texture import settings/Change Texture Format/Default/RGB Compressed ATC 4 bit")]
    static void ChangeTextureFormat_Default_ATC_RGB4()
    {
        ChangeTextureFormat(TextureImporterFormat.ATC_RGB4);
    }

    [MenuItem("HelpTools/Texture import settings/Change Texture Format/Default/RGBA Compressed ATC 8 bit")]
    static void ChangeTextureFormat_Default_ATC_RGBA8()
    {
        ChangeTextureFormat(TextureImporterFormat.ATC_RGBA8);
    }

    [MenuItem("HelpTools/Texture import settings/Change Texture Format/Default/Automatic 16 bit")]
    static void ChangeTextureFormat_Default_Automatic16bit()
    {
        ChangeTextureFormat(TextureImporterFormat.Automatic16bit);
    }

    [MenuItem("HelpTools/Texture import settings/Change Texture Format/Default/RGB 16 bit")]
    static void ChangeTextureFormat_Default_RGB16()
    {
        ChangeTextureFormat(TextureImporterFormat.RGB16);
    }

    [MenuItem("HelpTools/Texture import settings/Change Texture Format/Default/RGBA 16 bit")]
    static void ChangeTextureFormat_Default_ARGB16()
    {
        ChangeTextureFormat(TextureImporterFormat.ARGB16);
    }

    [MenuItem("HelpTools/Texture import settings/Change Texture Format/Default/AutomaticTruecolor")]
    static void ChangeTextureFormat_Default_AutomaticTruecolor()
    {
        ChangeTextureFormat(TextureImporterFormat.AutomaticTruecolor);
    }

    [MenuItem("HelpTools/Texture import settings/Change Texture Format/Default/RGB 24 bit")]
    static void ChangeTextureFormat_Default_RGB24()
    {
        ChangeTextureFormat(TextureImporterFormat.RGB24);
    }

    [MenuItem("HelpTools/Texture import settings/Change Texture Format/Default/Alpha 8 bit")]
    static void ChangeTextureFormat_Default_Alpha8()
    {
        ChangeTextureFormat(TextureImporterFormat.Alpha8);
    }

    [MenuItem("HelpTools/Texture import settings/Change Texture Format/Default/ARGB 32 bit")]
    static void ChangeTextureFormat_Default_ARGB32()
    {
        ChangeTextureFormat(TextureImporterFormat.ARGB32);
    }

    [MenuItem("HelpTools/Texture import settings/Change Texture Format/Default/RGBA 32 bit")]
    static void ChangeTextureFormat_Default_RGBA32()
    {
        ChangeTextureFormat(TextureImporterFormat.RGBA32);
    }


    //--- ChangeTextureFormat. Web

    [MenuItem("HelpTools/Texture import settings/Change Texture Format/Web/RGB Compressed DXT1")]
    static void ChangeTextureFormat_Web_DXT1()
    {
        ChangeTextureFormat(TextureImporterFormat.DXT1, Platform.Web);
    }

    [MenuItem("HelpTools/Texture import settings/Change Texture Format/Web/RGB Compressed DXT5")]
    static void ChangeTextureFormat_Web_DXT5()
    {
        ChangeTextureFormat(TextureImporterFormat.DXT5, Platform.Web);
    }

    [MenuItem("HelpTools/Texture import settings/Change Texture Format/Web/RGB 16 bit")]
    static void ChangeTextureFormat_Web_RGB16()
    {
        ChangeTextureFormat(TextureImporterFormat.RGB16, Platform.Web);
    }

    [MenuItem("HelpTools/Texture import settings/Change Texture Format/Web/RGB 24 bit")]
    static void ChangeTextureFormat_Web_RGB24()
    {
        ChangeTextureFormat(TextureImporterFormat.RGB24, Platform.Web);
    }

    [MenuItem("HelpTools/Texture import settings/Change Texture Format/Web/Alpha 8 bit")]
    static void ChangeTextureFormat_Web_Alpha8()
    {
        ChangeTextureFormat(TextureImporterFormat.Alpha8, Platform.Web);
    }

    [MenuItem("HelpTools/Texture import settings/Change Texture Format/Web/RGBA 16 bit")]
    static void ChangeTextureFormat_Web_ARGB16()
    {
        ChangeTextureFormat(TextureImporterFormat.ARGB16, Platform.Web);
    }

    [MenuItem("HelpTools/Texture import settings/Change Texture Format/Web/RGBA 32 bit")]
    static void ChangeTextureFormat_Web_RGBA32()
    {
        ChangeTextureFormat(TextureImporterFormat.RGBA32, Platform.Web);
    }


    //--- ChangeTextureFormat. Standalone

    [MenuItem("HelpTools/Texture import settings/Change Texture Format/Standalone/RGB Compressed DXT1")]
    static void ChangeTextureFormat_Standalone_DXT1()
    {
        ChangeTextureFormat(TextureImporterFormat.DXT1, Platform.Standalone);
    }

    [MenuItem("HelpTools/Texture import settings/Change Texture Format/Standalone/RGB Compressed DXT5")]
    static void ChangeTextureFormat_Standalone_DXT5()
    {
        ChangeTextureFormat(TextureImporterFormat.DXT5, Platform.Standalone);
    }

    [MenuItem("HelpTools/Texture import settings/Change Texture Format/Standalone/RGB 16 bit")]
    static void ChangeTextureFormat_Standalone_RGB16()
    {
        ChangeTextureFormat(TextureImporterFormat.RGB16, Platform.Standalone);
    }

    [MenuItem("HelpTools/Texture import settings/Change Texture Format/Standalone/RGB 24 bit")]
    static void ChangeTextureFormat_Standalone_RGB24()
    {
        ChangeTextureFormat(TextureImporterFormat.RGB24, Platform.Standalone);
    }

    [MenuItem("HelpTools/Texture import settings/Change Texture Format/Standalone/Alpha 8 bit")]
    static void ChangeTextureFormat_Standalone_Alpha8()
    {
        ChangeTextureFormat(TextureImporterFormat.Alpha8, Platform.Standalone);
    }

    [MenuItem("HelpTools/Texture import settings/Change Texture Format/Standalone/RGBA 16 bit")]
    static void ChangeTextureFormat_Standalone_ARGB16()
    {
        ChangeTextureFormat(TextureImporterFormat.ARGB16, Platform.Standalone);
    }

    [MenuItem("HelpTools/Texture import settings/Change Texture Format/Standalone/RGBA 32 bit")]
    static void ChangeTextureFormat_Standalone_RGBA32()
    {
        ChangeTextureFormat(TextureImporterFormat.RGBA32, Platform.Standalone);
    }


    //--- ChangeTextureFormat. Android

    [MenuItem("HelpTools/Texture import settings/Change Texture Format/Android/RGB Compressed DXT1")]
    static void ChangeTextureFormat_Android_DXT1()
    {
        ChangeTextureFormat(TextureImporterFormat.DXT1, Platform.Android);
    }

    [MenuItem("HelpTools/Texture import settings/Change Texture Format/Android/RGB Compressed DXT5")]
    static void ChangeTextureFormat_Android_DXT5()
    {
        ChangeTextureFormat(TextureImporterFormat.DXT5, Platform.Android);
    }

    [MenuItem("HelpTools/Texture import settings/Change Texture Format/Android/RGB Compressed ETC 4 bit")]
    static void ChangeTextureFormat_Android_ETC_RGB4()
    {
        ChangeTextureFormat(TextureImporterFormat.ETC_RGB4, Platform.Android);
    }

    [MenuItem("HelpTools/Texture import settings/Change Texture Format/Android/RGB Compressed PVRTC 2 bit")]
    static void ChangeTextureFormat_Android_PVRTC_RGB2()
    {
        ChangeTextureFormat(TextureImporterFormat.PVRTC_RGB2, Platform.Android);
    }

    [MenuItem("HelpTools/Texture import settings/Change Texture Format/Android/RGBA Compressed PVRTC 2 bit")]
    static void ChangeTextureFormat_Android_PVRTC_RGBA2()
    {
        ChangeTextureFormat(TextureImporterFormat.PVRTC_RGBA2, Platform.Android);
    }

    [MenuItem("HelpTools/Texture import settings/Change Texture Format/Android/RGB Compressed PVRTC 4 bit")]
    static void ChangeTextureFormat_Android_PVRTC_RGB4()
    {
        ChangeTextureFormat(TextureImporterFormat.PVRTC_RGB4, Platform.Android);
    }

    [MenuItem("HelpTools/Texture import settings/Change Texture Format/Android/RGBA Compressed PVRTC 4 bit")]
    static void ChangeTextureFormat_Android_PVRTC_RGBA4()
    {
        ChangeTextureFormat(TextureImporterFormat.PVRTC_RGBA4, Platform.Android);
    }

    [MenuItem("HelpTools/Texture import settings/Change Texture Format/Android/RGB Compressed ATC 4 bit")]
    static void ChangeTextureFormat_Android_ATC_RGB4()
    {
        ChangeTextureFormat(TextureImporterFormat.ATC_RGB4, Platform.Android);
    }

    [MenuItem("HelpTools/Texture import settings/Change Texture Format/Android/RGBA Compressed ATC 8 bit")]
    static void ChangeTextureFormat_Android_ATC_RGBA8()
    {
        ChangeTextureFormat(TextureImporterFormat.ATC_RGBA8, Platform.Android);
    }

    [MenuItem("HelpTools/Texture import settings/Change Texture Format/Android/RGB 16 bit")]
    static void ChangeTextureFormat_Android_RGB16()
    {
        ChangeTextureFormat(TextureImporterFormat.RGB16, Platform.Android);
    }

    [MenuItem("HelpTools/Texture import settings/Change Texture Format/Android/RGB 24 bit")]
    static void ChangeTextureFormat_Android_RGB24()
    {
        ChangeTextureFormat(TextureImporterFormat.RGB24, Platform.Android);
    }

    [MenuItem("HelpTools/Texture import settings/Change Texture Format/Android/Alpha 8 bit")]
    static void ChangeTextureFormat_Android_Alpha8()
    {
        ChangeTextureFormat(TextureImporterFormat.Alpha8, Platform.Android);
    }

    [MenuItem("HelpTools/Texture import settings/Change Texture Format/Android/RGBA 16 bit")]
    static void ChangeTextureFormat_Android_ARGB16()
    {
        ChangeTextureFormat(TextureImporterFormat.ARGB16, Platform.Android);
    }

    [MenuItem("HelpTools/Texture import settings/Change Texture Format/Android/RGBA 32 bit")]
    static void ChangeTextureFormat_Android_RGBA32()
    {
        ChangeTextureFormat(TextureImporterFormat.RGBA32, Platform.Android);
    }

    //--- ChangeTextureFormat. iPhone

    [MenuItem("HelpTools/Texture import settings/Change Texture Format/iPhone/RGB Compressed PVRTC 2 bit")]
    static void ChangeTextureFormat_iPhone_PVRTC_RGB2()
    {
        ChangeTextureFormat(TextureImporterFormat.PVRTC_RGB2, Platform.iPhone);
    }

    [MenuItem("HelpTools/Texture import settings/Change Texture Format/iPhone/RGBA Compressed PVRTC 2 bit")]
    static void ChangeTextureFormat_iPhone_PVRTC_RGBA2()
    {
        ChangeTextureFormat(TextureImporterFormat.PVRTC_RGBA2, Platform.iPhone);
    }

    [MenuItem("HelpTools/Texture import settings/Change Texture Format/iPhone/RGB Compressed PVRTC 4 bit")]
    static void ChangeTextureFormat_iPhone_PVRTC_RGB4()
    {
        ChangeTextureFormat(TextureImporterFormat.PVRTC_RGB4, Platform.iPhone);
    }

    [MenuItem("HelpTools/Texture import settings/Change Texture Format/iPhone/RGBA Compressed PVRTC 4 bit")]
    static void ChangeTextureFormat_iPhone_PVRTC_RGBA4()
    {
        ChangeTextureFormat(TextureImporterFormat.PVRTC_RGBA4, Platform.iPhone);
    }

    [MenuItem("HelpTools/Texture import settings/Change Texture Format/iPhone/RGB 16 bit")]
    static void ChangeTextureFormat_iPhone_RGB16()
    {
        ChangeTextureFormat(TextureImporterFormat.RGB16, Platform.iPhone);
    }

    [MenuItem("HelpTools/Texture import settings/Change Texture Format/iPhone/RGB 24 bit")]
    static void ChangeTextureFormat_iPhone_RGB24()
    {
        ChangeTextureFormat(TextureImporterFormat.RGB24, Platform.iPhone);
    }

    [MenuItem("HelpTools/Texture import settings/Change Texture Format/iPhone/Alpha 8 bit")]
    static void ChangeTextureFormat_iPhone_Alpha8()
    {
        ChangeTextureFormat(TextureImporterFormat.Alpha8, Platform.iPhone);
    }

    [MenuItem("HelpTools/Texture import settings/Change Texture Format/iPhone/RGBA 16 bit")]
    static void ChangeTextureFormat_iPhone_ARGB16()
    {
        ChangeTextureFormat(TextureImporterFormat.ARGB16, Platform.iPhone);
    }

    [MenuItem("HelpTools/Texture import settings/Change Texture Format/iPhone/RGBA 32 bit")]
    static void ChangeTextureFormat_iPhone_RGBA32()
    {
        ChangeTextureFormat(TextureImporterFormat.RGBA32, Platform.iPhone);
    }

    //--- ChangeTextureFormat. FlashPlayer

    //[MenuItem("HelpTools/Texture import settings/Change Texture Format/FlashPlayer/RGB JPG Compressed")]
    //static void ChangeTextureFormat_FlashPlayer_ATF_RGB_JPG()
    //{
    //    ChangeTextureFormat(TextureImporterFormat.ATF_RGB_JPG, Platform.FlashPlayer);
    //}

    //[MenuItem("HelpTools/Texture import settings/Change Texture Format/FlashPlayer/RGBA JPG Compressed")]
    //static void ChangeTextureFormat_FlashPlayer_ATF_RGBA_JPG()
    //{
    //    ChangeTextureFormat(TextureImporterFormat.ATF_RGBA_JPG, Platform.FlashPlayer);
    //}

    [MenuItem("HelpTools/Texture import settings/Change Texture Format/FlashPlayer/RGB 24 bit")]
    static void ChangeTextureFormat_FlashPlayer_RGB24()
    {
        ChangeTextureFormat(TextureImporterFormat.RGB24, Platform.FlashPlayer);
    }

    [MenuItem("HelpTools/Texture import settings/Change Texture Format/FlashPlayer/RGBA 32 bit")]
    static void ChangeTextureFormat_FlashPlayer_RGBA32()
    {
        ChangeTextureFormat(TextureImporterFormat.RGBA32, Platform.FlashPlayer);
    }

    //--- ChangeMaxTextureSize ------------------

    //--- ChangeMaxTextureSize. Default

    [MenuItem("HelpTools/Texture import settings/Change Max Texture Size/Default/32")]
    static void ChangeTextureSize_32()
    {
        ChangeMaxTextureSize(32);
    }

    [MenuItem("HelpTools/Texture import settings/Change Max Texture Size/Default/64")]
    static void ChangeTextureSize_64()
    {
        ChangeMaxTextureSize(64);
    }

    [MenuItem("HelpTools/Texture import settings/Change Max Texture Size/Default/128")]
    static void ChangeTextureSize_128()
    {
        ChangeMaxTextureSize(128);
    }

    [MenuItem("HelpTools/Texture import settings/Change Max Texture Size/Default/256")]
    static void ChangeTextureSize_256()
    {
        ChangeMaxTextureSize(256);
    }

    [MenuItem("HelpTools/Texture import settings/Change Max Texture Size/Default/512")]
    static void ChangeTextureSize_512()
    {
        ChangeMaxTextureSize(512);
    }

    [MenuItem("HelpTools/Texture import settings/Change Max Texture Size/Default/1024")]
    static void ChangeTextureSize_1024()
    {
        ChangeMaxTextureSize(1024);
    }

    [MenuItem("HelpTools/Texture import settings/Change Max Texture Size/Default/2048")]
    static void ChangeTextureSize_2048()
    {
        ChangeMaxTextureSize(2048);
    }

    [MenuItem("HelpTools/Texture import settings/Change Max Texture Size/Default/4096")]
    static void ChangeTextureSize_4096()
    {
        ChangeMaxTextureSize(4096);
    }

    //--- ChangeMaxTextureSize. Web

    [MenuItem("HelpTools/Texture import settings/Change Max Texture Size/Web/32")]
    static void ChangeTextureSizeWeb_32()
    {
        ChangeMaxTextureSize(32, Platform.Web);
    }

    [MenuItem("HelpTools/Texture import settings/Change Max Texture Size/Web/64")]
    static void ChangeTextureSizeWeb_64()
    {
        ChangeMaxTextureSize(64, Platform.Web);
    }

    [MenuItem("HelpTools/Texture import settings/Change Max Texture Size/Web/128")]
    static void ChangeTextureSizeWeb_128()
    {
        ChangeMaxTextureSize(128, Platform.Web);
    }

    [MenuItem("HelpTools/Texture import settings/Change Max Texture Size/Web/256")]
    static void ChangeTextureSizeWeb_256()
    {
        ChangeMaxTextureSize(256, Platform.Web);
    }

    [MenuItem("HelpTools/Texture import settings/Change Max Texture Size/Web/512")]
    static void ChangeTextureSizeWeb_512()
    {
        ChangeMaxTextureSize(512, Platform.Web);
    }

    [MenuItem("HelpTools/Texture import settings/Change Max Texture Size/Web/1024")]
    static void ChangeTextureSizeWeb_1024()
    {
        ChangeMaxTextureSize(1024, Platform.Web);
    }

    [MenuItem("HelpTools/Texture import settings/Change Max Texture Size/Web/2048")]
    static void ChangeTextureSizeWeb_2048()
    {
        ChangeMaxTextureSize(2048, Platform.Web);
    }

    [MenuItem("HelpTools/Texture import settings/Change Max Texture Size/Web/4096")]
    static void ChangeTextureSizeWeb_4096()
    {
        ChangeMaxTextureSize(4096, Platform.Web);
    }

    //--- ChangeMaxTextureSize. Standalone

    [MenuItem("HelpTools/Texture import settings/Change Max Texture Size/Standalone/32")]
    static void ChangeTextureSizeStandalone_32()
    {
        ChangeMaxTextureSize(32, Platform.Standalone);
    }

    [MenuItem("HelpTools/Texture import settings/Change Max Texture Size/Standalone/64")]
    static void ChangeTextureSizeStandalone_64()
    {
        ChangeMaxTextureSize(64, Platform.Standalone);
    }

    [MenuItem("HelpTools/Texture import settings/Change Max Texture Size/Standalone/128")]
    static void ChangeTextureSizeStandalone_128()
    {
        ChangeMaxTextureSize(128, Platform.Standalone);
    }

    [MenuItem("HelpTools/Texture import settings/Change Max Texture Size/Standalone/256")]
    static void ChangeTextureSizeStandalone_256()
    {
        ChangeMaxTextureSize(256, Platform.Standalone);
    }

    [MenuItem("HelpTools/Texture import settings/Change Max Texture Size/Standalone/512")]
    static void ChangeTextureSizeStandalone_512()
    {
        ChangeMaxTextureSize(512, Platform.Standalone);
    }

    [MenuItem("HelpTools/Texture import settings/Change Max Texture Size/Standalone/1024")]
    static void ChangeTextureSizeStandalone_1024()
    {
        ChangeMaxTextureSize(1024, Platform.Standalone);
    }

    [MenuItem("HelpTools/Texture import settings/Change Max Texture Size/Standalone/2048")]
    static void ChangeTextureSizeStandalone_2048()
    {
        ChangeMaxTextureSize(2048, Platform.Standalone);
    }

    [MenuItem("HelpTools/Texture import settings/Change Max Texture Size/Standalone/4096")]
    static void ChangeTextureSizeStandalone_4096()
    {
        ChangeMaxTextureSize(4096, Platform.Standalone);
    }

    //--- ChangeMaxTextureSize. Android

    [MenuItem("HelpTools/Texture import settings/Change Max Texture Size/Android/32")]
    static void ChangeTextureSizeAndroid_32()
    {
        ChangeMaxTextureSize(32, Platform.Android);
    }

    [MenuItem("HelpTools/Texture import settings/Change Max Texture Size/Android/64")]
    static void ChangeTextureSizeAndroid_64()
    {
        ChangeMaxTextureSize(64, Platform.Android);
    }

    [MenuItem("HelpTools/Texture import settings/Change Max Texture Size/Android/128")]
    static void ChangeTextureSizeAndroid_128()
    {
        ChangeMaxTextureSize(128, Platform.Android);
    }

    [MenuItem("HelpTools/Texture import settings/Change Max Texture Size/Android/256")]
    static void ChangeTextureSizeAndroid_256()
    {
        ChangeMaxTextureSize(256, Platform.Android);
    }

    [MenuItem("HelpTools/Texture import settings/Change Max Texture Size/Android/512")]
    static void ChangeTextureSizeAndroid_512()
    {
        ChangeMaxTextureSize(512, Platform.Android);
    }

    [MenuItem("HelpTools/Texture import settings/Change Max Texture Size/Android/1024")]
    static void ChangeTextureSizeAndroid_1024()
    {
        ChangeMaxTextureSize(1024, Platform.Android);
    }

    [MenuItem("HelpTools/Texture import settings/Change Max Texture Size/Android/2048")]
    static void ChangeTextureSizeAndroid_2048()
    {
        ChangeMaxTextureSize(2048, Platform.Android);
    }

    [MenuItem("HelpTools/Texture import settings/Change Max Texture Size/Android/4096")]
    static void ChangeTextureSizeAndroid_4096()
    {
        ChangeMaxTextureSize(4096, Platform.Android);
    }

    //--- ChangeMaxTextureSize. iPhone

    [MenuItem("HelpTools/Texture import settings/Change Max Texture Size/iPhone/32")]
    static void ChangeTextureSizeIPhone_32()
    {
        ChangeMaxTextureSize(32, Platform.iPhone);
    }

    [MenuItem("HelpTools/Texture import settings/Change Max Texture Size/iPhone/64")]
    static void ChangeTextureSizeIPhone_64()
    {
        ChangeMaxTextureSize(64, Platform.iPhone);
    }

    [MenuItem("HelpTools/Texture import settings/Change Max Texture Size/iPhone/128")]
    static void ChangeTextureSizeIPhone_128()
    {
        ChangeMaxTextureSize(128, Platform.iPhone);
    }

    [MenuItem("HelpTools/Texture import settings/Change Max Texture Size/iPhone/256")]
    static void ChangeTextureSizeIPhone_256()
    {
        ChangeMaxTextureSize(256, Platform.iPhone);
    }

    [MenuItem("HelpTools/Texture import settings/Change Max Texture Size/iPhone/512")]
    static void ChangeTextureSizeIPhone_512()
    {
        ChangeMaxTextureSize(512, Platform.iPhone);
    }

    [MenuItem("HelpTools/Texture import settings/Change Max Texture Size/iPhone/1024")]
    static void ChangeTextureSizeIPhone_1024()
    {
        ChangeMaxTextureSize(1024, Platform.iPhone);
    }

    [MenuItem("HelpTools/Texture import settings/Change Max Texture Size/iPhone/2048")]
    static void ChangeTextureSizeIPhone_2048()
    {
        ChangeMaxTextureSize(2048, Platform.iPhone);
    }

    [MenuItem("HelpTools/Texture import settings/Change Max Texture Size/iPhone/4096")]
    static void ChangeTextureSizeIPhone_4096()
    {
        ChangeMaxTextureSize(4096, Platform.iPhone);
    }

    //--- ChangeMaxTextureSize. FlashPlayer

    [MenuItem("HelpTools/Texture import settings/Change Max Texture Size/FlashPlayer/32")]
    static void ChangeTextureSizeFlashPlayer_32()
    {
        ChangeMaxTextureSize(32, Platform.FlashPlayer);
    }

    [MenuItem("HelpTools/Texture import settings/Change Max Texture Size/FlashPlayer/64")]
    static void ChangeTextureSizeFlashPlayer_64()
    {
        ChangeMaxTextureSize(64, Platform.FlashPlayer);
    }

    [MenuItem("HelpTools/Texture import settings/Change Max Texture Size/FlashPlayer/128")]
    static void ChangeTextureSizeFlashPlayer_128()
    {
        ChangeMaxTextureSize(128, Platform.FlashPlayer);
    }

    [MenuItem("HelpTools/Texture import settings/Change Max Texture Size/FlashPlayer/256")]
    static void ChangeTextureSizeFlashPlayer_256()
    {
        ChangeMaxTextureSize(256, Platform.FlashPlayer);
    }

    [MenuItem("HelpTools/Texture import settings/Change Max Texture Size/FlashPlayer/512")]
    static void ChangeTextureSizeFlashPlayer_512()
    {
        ChangeMaxTextureSize(512, Platform.FlashPlayer);
    }

    [MenuItem("HelpTools/Texture import settings/Change Max Texture Size/FlashPlayer/1024")]
    static void ChangeTextureSizeFlashPlayer_1024()
    {
        ChangeMaxTextureSize(1024, Platform.FlashPlayer);
    }

    [MenuItem("HelpTools/Texture import settings/Change Max Texture Size/FlashPlayer/2048")]
    static void ChangeTextureSizeFlashPlayer_2048()
    {
        ChangeMaxTextureSize(2048, Platform.FlashPlayer);
    }

    [MenuItem("HelpTools/Texture import settings/Change Max Texture Size/FlashPlayer/4096")]
    static void ChangeTextureSizeFlashPlayer_4096()
    {
        ChangeMaxTextureSize(4096, Platform.FlashPlayer);
    }

    //--- ChangeMipMap --------------------------

    [MenuItem("HelpTools/Texture import settings/Change MipMap/Enable MipMap")]
    static void ChangeMipMap_On()
    {
        ChangeMipMap(true);
    }

    [MenuItem("HelpTools/Texture import settings/Change MipMap/Disable MipMap")]
    static void ChangeMipMap_Off()
    {
        ChangeMipMap(false);
    }

    //--- Change ReadWrite ----------------------

    [MenuItem("HelpTools/Texture import settings/Change ReadWrite/Enable")]
    static void ChangeRW_On()
    {
        ChangeRW(true);
    }

    [MenuItem("HelpTools/Texture import settings/Change ReadWrite/Disable")]
    static void ChangeRW_Off()
    {
        ChangeRW(false);
    }

    //--- Change WrapMode -----------------------

    [MenuItem("HelpTools/Texture import settings/Change WrapMode/Clamp")]
    static void ChangeWrapMode_On()
    {
        ChangeWrapMode(TextureWrapMode.Clamp);
    }

    [MenuItem("HelpTools/Texture import settings/Change WrapMode/Repeat")]
    static void ChangeWrapMode_Off()
    {
        ChangeWrapMode(TextureWrapMode.Repeat);
    }

    //--- Change FilterMode ---------------------

    [MenuItem("HelpTools/Texture import settings/Change FilterMode/Point")]
    static void ChangeFilterMode_Point()
    {
        ChangeFilterMode(FilterMode.Point);
    }

    [MenuItem("HelpTools/Texture import settings/Change FilterMode/Bilinear")]
    static void ChangeFilterMode_Bilinear()
    {
        ChangeFilterMode(FilterMode.Bilinear);
    }

    [MenuItem("HelpTools/Texture import settings/Change FilterMode/Trilinear")]
    static void ChangeFilterMode_Trilinear()
    {
        ChangeFilterMode(FilterMode.Trilinear);
    }

    //--- Change Aniso level ---------------------

    [MenuItem("HelpTools/Texture import settings/Change Aniso level/0")]
    static void ChangeAniso_0()
    {
        ChangeAniso(0);
    }

    [MenuItem("HelpTools/Texture import settings/Change Aniso level/1")]
    static void ChangeAniso_1()
    {
        ChangeAniso(1);
    }

    [MenuItem("HelpTools/Texture import settings/Change Aniso level/2")]
    static void ChangeAniso_2()
    {
        ChangeAniso(2);
    }

    [MenuItem("HelpTools/Texture import settings/Change Aniso level/3")]
    static void ChangeAniso_3()
    {
        ChangeAniso(3);
    }

    [MenuItem("HelpTools/Texture import settings/Change Aniso level/4")]
    static void ChangeAniso_4()
    {
        ChangeAniso(4);
    }

    //--- Clear platform overrides --------------

    [MenuItem("HelpTools/Texture import settings/Clear platform overrides/All")]
    static void SelectedClearOverrides_All()
    {
        ClearOverrides();
    }

    [MenuItem("HelpTools/Texture import settings/Clear platform overrides/Standalone")]
    static void SelectedClearOverrides_Standalone()
    {
        ClearOverrides(Platform.Standalone);
    }

    [MenuItem("HelpTools/Texture import settings/Clear platform overrides/Android")]
    static void SelectedClearOverrides_Android()
    {
        ClearOverrides(Platform.Android);
    }

    [MenuItem("HelpTools/Texture import settings/Clear platform overrides/iPhone")]
    static void SelectedClearOverrides_iPhone()
    {
        ClearOverrides(Platform.iPhone);
    }

    [MenuItem("HelpTools/Texture import settings/Clear platform overrides/Web")]
    static void SelectedClearOverrides_Web()
    {
        ClearOverrides(Platform.Web);
    }

    [MenuItem("HelpTools/Texture import settings/Clear platform overrides/FlashPlayer")]
    static void SelectedClearOverrides_FlashPlayer()
    {
        ClearOverrides(Platform.FlashPlayer);
    }

    //--- Work ----------------------------------

    static void ChangeRW(bool flag, Platform somePlatform = Platform.Default)
    {
        Debug.Log(System.String.Format("{0} Set ReadWriteMode '{2}' @ {1} platform", logTitle, somePlatform, flag));
        TextureImportParams tiParams = new TextureImportParams(Actions.SetReadWrite, somePlatform);
        tiParams.readWriteMode = flag;
        SelectedChangeAnyPlatformSettings(tiParams);
    }

    static void ChangeWrapMode(TextureWrapMode newMode, Platform somePlatform = Platform.Default)
    {
        Debug.Log(System.String.Format("{0} Set TextureWrapMode '{2}' @ {1} platform", logTitle, somePlatform, newMode));
        TextureImportParams tiParams = new TextureImportParams(Actions.SetWrapMode, somePlatform);
        tiParams.wrapMode = newMode;
        SelectedChangeAnyPlatformSettings(tiParams);
    }

    static void ChangeFilterMode(FilterMode mode, Platform somePlatform = Platform.Default)
    {
        Debug.Log(System.String.Format("{0} Set FilterMode '{2}' @ {1} platform", logTitle, somePlatform, mode));
        TextureImportParams tiParams = new TextureImportParams(Actions.SetFilterMode, somePlatform);
        tiParams.filterMode = mode;
        SelectedChangeAnyPlatformSettings(tiParams);
    }

    static void ChangeAniso(int newLevel, Platform somePlatform = Platform.Default)
    {
        Debug.Log(System.String.Format("{0} Set AnisoLevel '{2}' @ {1} platform", logTitle, somePlatform, newLevel));
        TextureImportParams tiParams = new TextureImportParams(Actions.SetAniso, somePlatform);
        tiParams.anisoLevel = newLevel;
        SelectedChangeAnyPlatformSettings(tiParams);
    }

    static void ChangeMipMap(bool flag, Platform somePlatform = Platform.Default)
    {
        Debug.Log(System.String.Format("{0} Set MipMap '{2}' @ {1} platform", logTitle, somePlatform, flag));
        TextureImportParams tiParams = new TextureImportParams(Actions.SetMipMap, somePlatform);
        tiParams.mipMap = flag;
        SelectedChangeAnyPlatformSettings(tiParams);
    }

    static void ChangeMaxTextureSize(int newSize, Platform somePlatform = Platform.Default)
    {
        Debug.Log(System.String.Format("{0} Set MaxTextureSize '{2}' @ {1} platform", logTitle, somePlatform, newSize));
        TextureImportParams tiParams = new TextureImportParams(Actions.SetMaxTextureSize, somePlatform);
        tiParams.maxSize = newSize;
        SelectedChangeAnyPlatformSettings(tiParams);
    }

    static void ChangeTextureFormat(TextureImporterFormat newFormat, Platform somePlatform = Platform.Default)
    {
        Debug.Log(System.String.Format("{0} Set TextureImporterFormat '{2}' @ {1} platform", logTitle, somePlatform, newFormat));
        TextureImportParams tiParams = new TextureImportParams(Actions.SetTextureFormat, somePlatform);
        tiParams.tiFormat = newFormat;
        SelectedChangeAnyPlatformSettings(tiParams);
    }

    static void ClearOverrides(Platform somePlatform = Platform.All)
    {
        Debug.Log(System.String.Format("{0} Clear overrides @ {1} platform", logTitle, somePlatform));
        TextureImportParams tiParams = new TextureImportParams(Actions.ClearOverrides, somePlatform);
        SelectedChangeAnyPlatformSettings(tiParams);
    }

    /// <summary>
    /// Main work method
    /// </summary>
    static void SelectedChangeAnyPlatformSettings(TextureImportParams tip)
    {
        int processingTexturesNumber;
        Object[] originalSelection = Selection.objects;
        Object[] textures = GetSelectedTextures();
        Selection.objects = new Object[0]; //Clear selection (for correct data representation on GUI)
        processingTexturesNumber = textures.Length;
        if (processingTexturesNumber == 0)
        {
            Debug.LogWarning(logTitle + "Nothing to do. Please select objects/folders with 2d textures in Project tab");
            return;
        }
        AssetDatabase.StartAssetEditing();
        foreach (Texture2D texture in textures)
        {
            string path = AssetDatabase.GetAssetPath(texture);
            TextureImporter textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;
            switch (tip.action)
            {
                //platform independent 
                case Actions.SetMipMap:
                    textureImporter.mipmapEnabled = tip.mipMap;
                    break;
                case Actions.SetReadWrite:
                    textureImporter.isReadable = tip.readWriteMode;
                    break;
                case Actions.SetWrapMode:
                    textureImporter.wrapMode = tip.wrapMode;
                    break;
                case Actions.SetFilterMode:
                    textureImporter.filterMode = tip.filterMode;
                    break;
                case Actions.SetAniso:
                    textureImporter.anisoLevel = tip.anisoLevel;
                    break;
                case Actions.SetAll: //Set all platform independent settings
                    textureImporter.textureType = TextureImporterType.Advanced;
                    textureImporter.mipmapEnabled = tip.mipMap;
                    textureImporter.isReadable = tip.readWriteMode;
                    textureImporter.wrapMode = tip.wrapMode;
                    textureImporter.filterMode = tip.filterMode;
                    textureImporter.anisoLevel = tip.anisoLevel;
                    textureImporter.maxTextureSize = tip.maxSize;
                    textureImporter.textureFormat = tip.tiFormat;
                    break;
                //platform specific props
                default:
                    if (tip.platform == Platform.Default) //default platform mode
                    {
                        switch (tip.action)
                        {
                            case Actions.SetMaxTextureSize:
                                textureImporter.maxTextureSize = tip.maxSize;
                                break;
                            case Actions.SetTextureFormat:
                                textureImporter.textureFormat = tip.tiFormat;
                                break;
                            default:
                                Debug.Log("Unhandled action on Platform.Default: " + tip.action); //foolproof
                                return;
                        }
                    }
                    else //override mode
                    {
                        if (tip.platform != Platform.All)
                            textureImporter.GetPlatformTextureSettings(tip.platform.ToString(), out currentMaxTextureSize, out currentTIFormat);

                        switch (tip.action)
                        {
                            case Actions.SetMaxTextureSize:
                                textureImporter.SetPlatformTextureSettings(tip.platform.ToString(), tip.maxSize, currentTIFormat);
                                break;
                            case Actions.SetTextureFormat:
                                textureImporter.SetPlatformTextureSettings(tip.platform.ToString(), currentMaxTextureSize, tip.tiFormat);
                                break;
                            case Actions.ClearOverrides:
                                if (tip.platform == Platform.All)
                                {
                                    ClearPlatformOverrides(Platform.Android.ToString(), textureImporter);
                                    ClearPlatformOverrides(Platform.Standalone.ToString(), textureImporter);
                                    ClearPlatformOverrides(Platform.iPhone.ToString(), textureImporter);
                                    ClearPlatformOverrides(Platform.Web.ToString(), textureImporter);
                                    ClearPlatformOverrides(Platform.FlashPlayer.ToString(), textureImporter);
                                }
                                else
                                    ClearPlatformOverrides(tip.platform.ToString(), textureImporter);
                                break;
                            default:
                                Debug.Log("Unhandled action on Platform." + tip.platform.ToString() + ": " + tip.action); //foolproof
                                return;
                        }
                    }
                    break;
            }
            AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
        }
        AssetDatabase.StopAssetEditing();
        Selection.objects = originalSelection; //Restore selection
        Debug.Log("Textures processed: " + processingTexturesNumber);
    }

    static Object[] GetSelectedTextures()
    {
        return Selection.GetFiltered(typeof(Texture2D), SelectionMode.DeepAssets);
    }

    static void ClearPlatformOverrides(string platformName, TextureImporter importer)
    {
        //Workaround: without this AssetDatabase.ImportAsset() not working
        importer.SetPlatformTextureSettings(platformName, 0, 0);

        importer.ClearPlatformTextureSettings(platformName);
    }

}