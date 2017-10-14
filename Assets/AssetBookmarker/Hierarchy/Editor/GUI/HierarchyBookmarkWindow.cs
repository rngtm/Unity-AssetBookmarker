///-----------------------------------------
/// AssetBookmarker
/// @ 2016 RNGTM(https://github.com/rngtm)
///-----------------------------------------
namespace AssetBookmarker.Hierarchy
{
    using System.Linq;
    using UnityEngine;
    using UnityEditor;
    using UnityEditorInternal;

    /// <summary>
    /// メインのウィンドウ
    /// </summary>
    public class HierarchyBookmarkWindow : EditorWindow
    {
        private HierarchyBookmarkData bookmarkData;

        /// <summary>
        /// 検索用テキスト表示用のReorderableList
        /// </summary>
        private ReorderableList searchInfoList;

        private Vector2 scrollPosition = Vector2.zero;

        /// <summary>
        /// ウィンドウ描画処理
        /// </summary>
        void OnGUI()
        {
            if (this.bookmarkData == null)
            {
                this.bookmarkData = DataLoader.LoadData();
            }

            this.scrollPosition = EditorGUILayout.BeginScrollView(this.scrollPosition);

            if (this.searchInfoList == null)
            {
                this.RebuildSearchInfoList();
            }

            EditorGUILayout.LabelField(Config.GUI_WINDOW_HIERARCHY_TEXT_OVERVIEW);

            this.searchInfoList.DoLayoutList();
            EditorGUILayout.EndScrollView();

            CustomUI.VersionLabel();
        }

        /// <summary>
        /// ReorderableList作成
        /// </summary>
        private void RebuildSearchInfoList()
        {
            this.searchInfoList = this.CreateSearchInfoList();
        }

        /// <summary>
        /// ReorderableList作成
        /// </summary>
        private ReorderableList CreateSearchInfoList()
        {
            var reorderableList = new ReorderableList(this.bookmarkData.SearchInfos, typeof(SearchInfo));

            // ヘッダー描画
            var headerRect = default(Rect);
            reorderableList.drawHeaderCallback = (rect) =>
            {
                headerRect = rect;
                EditorGUI.LabelField(rect, Config.GUI_WINDOW_HIERARCHY_TEXT_LIST_HEADER);
            };

            // フッター描画
            reorderableList.drawFooterCallback = (rect) =>
            {
                rect.y = headerRect.y + 3;
                ReorderableList.defaultBehaviours.DrawFooter(rect, reorderableList);
            };

            // 要素の描画
            reorderableList.drawElementCallback = (rect, index, isActive, isFocused) =>
            {
                rect.height -= 6f;
                rect.y += 3f;

                const float space = 4f;
                const float removeButtonWidth = 19f;

                var selectButtonRect = new Rect(rect);
                selectButtonRect.width = 42f;

                var textRect = new Rect(rect);
                textRect.width -= selectButtonRect.width + removeButtonWidth + space;
                textRect.x = selectButtonRect.x + selectButtonRect.width + space;

                var removeButtonRect = new Rect(rect);
                removeButtonRect.width = removeButtonWidth;
                removeButtonRect.height -= 1f;
                removeButtonRect.x = textRect.x + textRect.width + 2;

                if (GUI.Button(selectButtonRect, Config.GUI_WINDOW_HIERARCHY_TEXT_FILTER_APPLY_BUTTON, EditorStyles.miniButton))
                {
                    var data = (SearchInfo)reorderableList.list[index];
                    SceneHierarchyAccessor.SetSearchFilter(data.Text);

                    if (!string.IsNullOrEmpty(data.Text))
                    {
                        SceneHierarchyAccessor.SelectTop();
                    }
                }

                if (GUI.Button(removeButtonRect, Config.GUI_WINDOW_HIERARCHY_TEXT_FILTER_REMOVE_BUTTON))
                {
                    this.DoRemoveButton(index);
                }

                EditorGUI.BeginChangeCheck();
                var text = EditorGUI.TextField(textRect, this.bookmarkData.SearchInfos[index].Text);
                if (EditorGUI.EndChangeCheck())
                {
                    this.bookmarkData.SearchInfos[index].Text = text;
                    EditorUtility.SetDirty(this.bookmarkData);
                }
            };

            reorderableList.drawElementBackgroundCallback = (rect, index, isActive, isFocused) => { };

            return reorderableList;
        }

        /// <summary>
        /// リストの要素を削除
        /// </summary>
        private void DoRemoveButton(int index)
        {
            EditorApplication.delayCall += () =>
            {
                this.bookmarkData.SearchInfos.RemoveAt(index);
                this.Repaint();
                EditorUtility.SetDirty(this.bookmarkData);
            };
        }

        /// <summary>
        /// ウィンドウを開く
        /// </summary>
        [MenuItem(Config.GUI_MENU_TEXT_OPEN_BOOKMARK_HIERARCHY)]
        static void Open()
        {
            var window = GetWindow<HierarchyBookmarkWindow>();
            window.titleContent.text = Config.GUI_WINDOW_HIERARCHY_TEXT_TITLE;
            window.Repaint();
        }

        /// <summary>
        /// 選択オブジェクトを登録
        /// </summary>
        [MenuItem(Config.GUI_MENU_TEXT_REGISTER_HIERARCHY, false, 12)]
        static void RegisterSelectionToPalette()
        {
            var data = DataLoader.LoadData();
            data.SearchInfos.AddRange(
                Selection.gameObjects.Select(go => new SearchInfo { Text = go.name })
            );
            EditorUtility.SetDirty(data);
            Open();
        }

        /// <summary>
        /// RegisterSelectionToPaletteのValidateメソッド
        /// </summary>
        [MenuItem(Config.GUI_MENU_TEXT_REGISTER_HIERARCHY, true)]
        static bool ValidateAddSelectionToPalette()
        {
            return Selection.activeObject != null && !AssetDatabase.IsMainAsset(Selection.activeObject);
        }

    }
}
