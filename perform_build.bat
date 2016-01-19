@echo StartTime: %time%
"C:\Program Files\Unity\Editor\Unity.exe" -batchmode -nographics -projectPath "C:\GitAlt\ShelfAssetBundler" -executeMethod ExportAssetBundles.BuildAssetBundlePerTexture -quit -Compression DxtNoBundleCompression
@echo EndTime: %time%
