using UnityEditor;
using UnityEngine;

public class AssetBundleBuilder
{
    [MenuItem("Tools/Build AssetBundles")]
    public static void BuildAllAssetBundles()
    {
        string outputPath = "AssetBundles/win";

        // 如果資料夾不存在則建立
        if (!System.IO.Directory.Exists(outputPath))
            System.IO.Directory.CreateDirectory(outputPath);
        BuildPipeline.BuildAssetBundles(outputPath,
            BuildAssetBundleOptions.None,
            BuildTarget.StandaloneWindows); // 替換成你的平台

        outputPath = "AssetBundles/ios";
        // 如果資料夾不存在則建立
        if (!System.IO.Directory.Exists(outputPath))
            System.IO.Directory.CreateDirectory(outputPath);
        BuildPipeline.BuildAssetBundles(outputPath,
            BuildAssetBundleOptions.None,
            BuildTarget.iOS); // 替換成你的平台

        outputPath = "AssetBundles/android";
        // 如果資料夾不存在則建立
        if (!System.IO.Directory.Exists(outputPath))
            System.IO.Directory.CreateDirectory(outputPath);
        BuildPipeline.BuildAssetBundles(outputPath,
            BuildAssetBundleOptions.None,
            BuildTarget.Android); // 替換成你的平台
    }
}
