///-----------------------------------------
/// AssetBookmarker
/// @ 2016 RNGTM(https://github.com/rngtm)
///-----------------------------------------
namespace AssetBookmarker.Project
{
    /// <summary>
    /// Export設定
    /// </summary>
    public static class ExportConfig
    {
        // /// <summary>
        // /// AssetBookmarkerのルートフォルダ名
        // /// </summary>
        // private const string RootFolderName = "AssetBookmarker";

        // private const string DataRealtivePath = "Data/Project";
        private const string DataRealtivePath = "Data/Project";

        /// <summary>
        /// Bookmarkデータの保存先の相対パス(Assetsフォルダ以下)
        /// </summary>
        private const string ProjectSaveFolderPath = "Assets/AssetBookmarker/" + DataRealtivePath;
        
        /// <summary>
        /// Bookmarkデータの保存先の相対パス(Packageフォルダ以下)
        /// </summary>
        private static readonly string PackageSaveFolderPath = PackageConfig.GetPackagePath(DataRealtivePath);

        /// <summary>
        /// 新規Bookmarkデータのデフォルトの名前
        /// </summary>
        public const string DefaultAssetName = "NewBookmarkData";
        
        public static string GetDataExportDirectory()
        {
            return PackageConfig.IsInstallPackage ? PackageSaveFolderPath : ProjectSaveFolderPath;
        }
        
    }
}
