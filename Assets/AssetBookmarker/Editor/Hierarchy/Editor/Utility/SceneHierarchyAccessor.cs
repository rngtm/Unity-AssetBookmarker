///-----------------------------------------
/// AssetBookmarker
/// @ 2016 RNGTM(https://github.com/rngtm)
///-----------------------------------------
namespace AssetBookmarker.Hierarchy
{
    using System.Collections.Generic;
    using System.Reflection;
    using UnityEngine;
    using UnityEditor;
    using UnityEditor.IMGUI.Controls;

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

        /// <summary>
        /// Hierarchyで一番上にある項目を選択
        /// </summary>
        public static void SelectTop()
        {
            var rows = GetRows();
            if (rows.Count < 2) { return; }

            var gameObject = (GameObject)EditorUtility.InstanceIDToObject(rows[1].id);
            Selection.activeGameObject = gameObject;
        }

        /// <summary>
        /// ヒエラルキーウィンドウ上の項目の一覧取得
        /// </summary>
        public static IList<TreeViewItem> GetRows()
        {
            // Hierarchyウィンドウ取得
            var asm = Assembly.Load("UnityEditor.dll");
            var typeWindow = asm.GetType("UnityEditor.SceneHierarchyWindow");
            var window = EditorWindow.GetWindow(typeWindow);

            // GetRows実行
            var m_TreeView = typeWindow.GetField("m_TreeView", BindingFlags.Instance | BindingFlags.NonPublic)
            .GetValue(window);
            var data = m_TreeView.GetType().GetProperty("data", BindingFlags.Instance | BindingFlags.Public)
            .GetValue(m_TreeView, null);
            return (IList<TreeViewItem>)data.GetType().GetMethod("GetRows", BindingFlags.Instance | BindingFlags.Public)
            .Invoke(data, null);
        }
    }
}
