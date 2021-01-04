///-----------------------------------------
/// AssetBookmarker
/// @ 2016 RNGTM(https://github.com/rngtm)
///-----------------------------------------
namespace AssetBookmarker.Project
{
    /// <summary>
    /// ファイルの作成を行うクラス
    /// </summary>
    public class DataGenerator
    {
        /// <summary>
        /// データを作成
        /// </summary>
        public static ProjectBookmarkData CreateBookmarkData()
        {
            return GenericDataGenerator.CreateData<ProjectBookmarkData>(
                ExportConfig.DefaultAssetName
            );
        }
        
        /// <summary>
        /// データを強制的に作成
        /// </summary>
        public static ProjectBookmarkData CreateBookmarkDataImmediately()
        {
            return GenericDataGenerator.CreateDataImmediately<ProjectBookmarkData>(
                ExportConfig.DefaultAssetName
            );
        }
    }
}