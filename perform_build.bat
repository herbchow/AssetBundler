@echo StartTime: %time%
"C:\Program Files\Unity\Editor\Unity.exe" -batchmode -nographics -projectPath "C:\GitAlt\ShelfAssetBundler" -executeMethod ExportAssetBundles.BuildAssetBundleFromTexture -quit -Compression Dxt_2K -OutputAppFileName some_image.dxt_2k -logFile c:/AzureStorage/editor2.log
@echo EndTime: %time%
