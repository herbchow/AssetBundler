@echo StartTime: %time%
"C:\AzureStorage\TestUnityInstall\Unity.exe" -batchmode -nographics -projectPath "C:\GitAlt\ShelfAssetBundler" -executeMethod ExportAssetBundles.BuildAssetBundlePerTexture -quit
@echo EndTime: %time%
