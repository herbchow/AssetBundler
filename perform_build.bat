@echo StartTime: %time%
"C:\Program Files (x86)\Unity_466\Editor\Unity.exe" -batchmode -nographics -projectPath "C:\GitAlt\ShelfAssetBundler" -executeMethod ExportAssetBundles.BuildAssetBundlePerTexture -quit
@echo EndTime: %time%
