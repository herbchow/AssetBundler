@echo StartTime: %time%
"C:\Program Files\Unity\Editor\Unity.exe" -batchmode -nographics -projectPath "C:\GitAlt\ShelfAssetBundler" -executeMethod ExportAssetBundles.BuildAssetBundlePerTexture -quit -Compression Dxt
@echo EndTime: %time%
