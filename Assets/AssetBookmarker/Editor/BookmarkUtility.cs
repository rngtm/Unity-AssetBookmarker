///-----------------------------------
/// AssetBookmarker
/// @ 2016 RNGTM(https://github.com/rngtm)
///-----------------------------------
namespace AssetBookmarker
{
    using System.Linq;
    using System.IO;
    using UnityEngine;
    using UnityEditor;

    public class BookmarkUtility : MonoBehaviour
    {
        /// <summary>
        /// Bookmarkデータファイル名
        /// </summary>
        private static readonly string DataName = "BookmarkData.asset";

        /// <summary>
        /// Bookmarkデータのロード
        /// </summary>
        public static BookmarkData LoadData()
        {
            var monoScript = Resources.FindObjectsOfTypeAll<MonoScript>().FirstOrDefault(m => m.GetClass() == typeof(BookmarkUtility));
            var path = AssetDatabase.GetAssetPath(monoScript);
            var dirPath = Path.GetDirectoryName(path);
            var dataPath = dirPath + Path.DirectorySeparatorChar + DataName;

            return (BookmarkData)AssetDatabase.LoadAssetAtPath(dataPath, typeof(BookmarkData));
        }
    }
}
