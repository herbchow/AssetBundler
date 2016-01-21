@echo StartTime: %time%
"C:\Program Files\Unity\Editor\Unity.exe" -batchmode -nographics -projectPath "C:\GitAlt\ShelfAssetBundler" -executeMethod ExportAssetBundles.BuildAssetBundleFromTexture -quit -Compression DxtNoBundleCompression -OutputAppFileName some_image.unity3d
@echo EndTime: %time%
