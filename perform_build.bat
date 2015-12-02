@echo StartTime: %time%
"C:\AzureStorage\TestUnityInstall\Editor\Unity.exe" -batchmode -nographics -projectPath "C:\GitAlt\ShelfAssetBundler" -executeMethod ExportAssetBundles.BuildAssetBundlePerTexture -quit
@echo EndTime: %time%
