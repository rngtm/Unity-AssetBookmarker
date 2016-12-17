///-----------------------------------------
/// AssetBookmarker
/// @ 2016 RNGTM(https://github.com/rngtm)
///-----------------------------------------
namespace AssetBookmarker.Project
{
    using UnityEngine;
    using UnityEditor;
    using UnityEditorInternal;

    /// <summary>
    /// ブックマークの管理を行うウィンドウ
    /// </summary>
    public class ProjectBookmarkWindow : EditorWindow
    {
        /// <summary>
        /// Bookmarkデータ
        /// </summary>
        private ProjectBookmarkData bookmarkData;

        /// <summary>
        /// Bookmark表示用のReorderableList
        /// </summary>
        private ReorderableList bookmarkList;

        /// <summary>
        /// ウィンドウの描画処理
        /// </summary>
        void OnGUI()
        {
            if (this.bookmarkData == null)
            {
                this.bookmarkData = DataLoader.LoadData();
            }
            
            if (this.bookmarkList == null)
            {
                this.RebuildBookmarkList();
            }

            EditorGUILayout.LabelField(Config.GUI_WINDOW_PROJECT_TEXT_OVERVIEW);
            this.bookmarkList.DoLayoutList();

            CustomUI.VersionLabel();
        }

        /// <summary>
        /// ReorderableListを作成する
        /// </summary>
        void RebuildBookmarkList()
        {
            this.bookmarkList = this.CreateBookmarkList();
        }

        /// <summary>
        /// ReorderableListを作成する
        /// </summary>
        private ReorderableList CreateBookmarkList()
        {
            var list = new ReorderableList(bookmarkData.Assets, typeof(Object));

            // ヘッダー描画
            var headerRect = default(Rect);
            list.drawHeaderCallback = (rect) =>
            {
                headerRect = rect;
                EditorGUI.LabelField(rect, Config.GUI_WINDOW_PROJECT_TEXT_LIST_HEADER);
            };

            // フッター描画
            list.drawFooterCallback = (rect) =>
            {
                rect.y = headerRect.y + 3;
                ReorderableList.defaultBehaviours.DrawFooter(rect, list);
            };

            // 要素の描画
            list.drawElementCallback = (rect, index, isActive, isFocused) =>
            {
                rect.y += 1;
                rect.height -= 4;

                var labelRect = new Rect(rect);
                labelRect.width = 13f;

                var buttonRect = new Rect(rect);
                buttonRect.x += labelRect.width + 8f;
                buttonRect.width = 20f;
                buttonRect.y += 1f;
                buttonRect.height -= 2f;

                var objectRect = new Rect(rect);
                objectRect.width -= labelRect.width + buttonRect.width + 18f;
                objectRect.x += labelRect.width + buttonRect.width + 16f;

                EditorGUI.LabelField(labelRect, index.ToString());
                EditorGUI.BeginChangeCheck();
                var asset = (Object)list.list[index];
                asset = EditorGUI.ObjectField(objectRect, asset, typeof(Object), false);
                if (EditorGUI.EndChangeCheck())
                {
                    list.list[index] = asset;
                    EditorUtility.SetDirty(bookmarkData);
                }

                if (GUI.Button(buttonRect, "-"))
                {
                    this.DoRemoveButton(list, index);
                }
            };

            list.onChangedCallback += (index) =>
            {
                EditorUtility.SetDirty(bookmarkData);
            };

            return list;
        }

        /// <summary>
        /// 要素の削除
        /// </summary>
        public void DoRemoveButton(ReorderableList list, int index)
        {
            EditorApplication.delayCall += () =>
            {
                list.list.RemoveAt(index);
                this.Repaint();
                EditorUtility.SetDirty(this.bookmarkData);
            };
        }
        
        /// <summary>
        /// ウィンドウを開く
        /// </summary>
        [MenuItem(Config.GUI_MENU_TEXT_OPEN_BOOKMARK_PROJECT)]
        static void Open()
        {
            var window = GetWindow<ProjectBookmarkWindow>();
            window.titleContent.text = Config.GUI_WINDOW_PROJECT_TEXT_TITLE;
            window.Repaint();
        }

        /// <summary>
        /// エディタ上で選択しているアセットをBookmarkへ登録
        /// </summary>
        [MenuItem(Config.GUI_MENU_TEXT_REGISTER_PROJECT, false, 10001)]
        static void RegisterSelection()
        {
            var data = DataLoader.LoadData();
            data.Assets.AddRange(Selection.objects);
            EditorUtility.SetDirty(data);
            Open();
        }

        /// <summary>
        /// RegisterSelectionのValidateメソッド
        /// </summary>
        [MenuItem(Config.GUI_MENU_TEXT_REGISTER_PROJECT, true)]
        static bool ValidateRegisterSelection()
        {
            return Selection.activeObject != null;
        }
    }
}
