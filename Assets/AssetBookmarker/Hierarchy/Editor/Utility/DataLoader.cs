///-----------------------------------------
/// AssetBookmarker
/// @ 2016 RNGTM(https://github.com/rngtm)
///-----------------------------------------
namespace AssetBookmarker.Hierarchy
{
    using System.Linq;
    using UnityEditor;

    /// <summary>
    /// データのロードを行うクラス
    /// </summary>
    public class DataLoader
    {
        /// <summary>
        /// データのロード
        /// </summary>
        public static HierarchyBookmarkData LoadData()
        {
             return AssetDatabase.FindAssets("t:ScriptableObject")
            .Select(guid => AssetDatabase.GUIDToAssetPath(guid))
            .Select(path => AssetDatabase.LoadAssetAtPath(path, typeof(HierarchyBookmarkData)))
            .Where(obj => obj != null)
            .Select(obj => (HierarchyBookmarkData)obj)
            .FirstOrDefault();
        }
    }
}
