///-----------------------------------------
/// AssetBookmarker
/// @ 2016 RNGTM(https://github.com/rngtm)
///-----------------------------------------
namespace AssetBookmarker.Hierarchy
{
    using System.Reflection;
    using UnityEditor;

    /// <summary>
    /// SceneHierarchyWindowへのアクセスを行うクラス
    /// </summary>
    public class SceneHierarchyAccessor
    {
        /// <summary>
        /// Hierarchyの検索ボックスの文字列を設定
        /// </summary>
        /// <param name="filter">検索文字列</param>
        public static void SetSearchFilter(string filter)
        {
            // Hierarchyウィンドウ取得
            var asm = Assembly.Load("UnityEditor.dll");
            var typeWindow = asm.GetType("UnityEditor.SceneHierarchyWindow");
            var window = EditorWindow.GetWindow(typeWindow);

            // 検索ボックスの文字列を設定
            var mode = (SearchableEditorWindow.SearchMode)typeWindow.GetField("m_SearchMode", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(window);
            var setAll = false;
            typeWindow.GetMethod("SetSearchFilter", BindingFlags.Instance | BindingFlags.NonPublic)
            .Invoke(window, new object[] { filter, mode, setAll });
        }
    }
}
