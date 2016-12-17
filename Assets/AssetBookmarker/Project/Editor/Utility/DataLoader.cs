///-----------------------------------------
/// AssetBookmarker
/// @ 2016 RNGTM(https://github.com/rngtm)
///-----------------------------------------
namespace AssetBookmarker.Project
{
    using System.Linq;
    using UnityEditor;

    /// <summary>
    /// データのロードを行うクラス
    /// </summary>
    public class DataLoader
    {
        /// <summary>
        /// Bookmarkデータのロード
        /// </summary>
        public static ProjectBookmarkData LoadData()
        {
             return AssetDatabase.FindAssets("t:ScriptableObject")
            .Select(guid => AssetDatabase.GUIDToAssetPath(guid))
            .Select(path => AssetDatabase.LoadAssetAtPath(path, typeof(ProjectBookmarkData)))
            .Where(obj => obj != null)
            .Select(obj => (ProjectBookmarkData)obj)
            .FirstOrDefault();
        }
    }
}
