///-----------------------------------
/// AssetBookmarker
/// @ 2016 RNGTM(https://github.com/rngtm)
///-----------------------------------
namespace AssetBookmarker
{
    using UnityEngine;
    using UnityEditor;
    using UnityEditorInternal;

    /// <summary>
    /// ブックマークの管理を行うウィンドウ
    /// </summary>
    public class BookmarkWindow : EditorWindow
    {
        /// <summary>
        /// MenuItemのPriority
        /// </summary>
        private const int Priority = 10001;

        /// <summary>
        /// Bookmarkデータ
        /// </summary>
        private static BookmarkData _data;

        /// <summary>
        /// Bookmark表示用のReorderableList
        /// </summary>
        private ReorderableList list;

        /// <summary>
        /// ウィンドウを開く
        /// </summary>
        [MenuItem("Tools/AssetBookmarker")]
        static void Open()
        {
            GetWindow<BookmarkWindow>();
        }

        /// <summary>
        /// ウィンドウの描画処理
        /// </summary>
        void OnGUI()
        {
            if (_data == null)
            {
                _data = BookmarkUtility.LoadData();
            }

            if (this.list == null)
            {
                this.CreateList();
            }

            EditorGUILayout.LabelField("ブックマークしたアセット一覧を表示します");

            this.list.DoLayoutList();
        }

        /// <summary>
        /// エディタ上で選択しているアセットをBookmarkへ登録
        /// </summary>
        [MenuItem("Assets/Add to bookmark list", false, Priority)]
        static void RegisterSelection()
        {
            if (_data == null)
            {
                _data = BookmarkUtility.LoadData();
            }

            _data.Assets.AddRange(Selection.objects);
            EditorUtility.SetDirty(_data);

            var window = Resources.FindObjectsOfTypeAll<BookmarkWindow>();
            if (window == null || window.Length == 0)
            {
                Open();
            }
            else
            {
                window[0].Repaint();
            }
        }

        /// <summary>
        /// RegisterSelectionのValidateメソッド
        /// </summary>
        [MenuItem("Assets/Add to bookmark list", true, Priority)]
        static bool ValidateRegisterSelection()
        {
            return Selection.activeObject != null;
        }

        /// <summary>
        /// ReorderableListを作成する
        /// </summary>
        void CreateList()
        {
            this.list = new ReorderableList(_data.Assets, typeof(Object));

            this.list.drawHeaderCallback = (rect) =>
            {
                EditorGUI.LabelField(rect, "Bookmarks");
            };

            // 要素の描画
            this.list.drawElementCallback = (rect, index, isActive, isFocused) =>
            {
                rect.y += 1;
                rect.height -= 4;

                var labelRect = new Rect(rect);
                labelRect.width = 17f;

                var objectRect = new Rect(rect);
                objectRect.width -= labelRect.width;
                objectRect.x += labelRect.width;

                EditorGUI.LabelField(labelRect, index.ToString());

                EditorGUI.BeginChangeCheck();
                var asset = (Object)list.list[index];
                asset = EditorGUI.ObjectField(objectRect, asset, typeof(Object), false);
                if (EditorGUI.EndChangeCheck())
                {
                    Debug.Log("Changed");
                    list.list[index] = asset;
                    EditorUtility.SetDirty(_data);
                }
            };

            // フッター描画
            this.list.drawFooterCallback = (rect) =>
            {
                if (this.list.count == 0)
                {
                    rect.position -= new Vector2(0f, this.list.elementHeight + this.list.headerHeight + 3f);
                }
                else
                {
                    rect.position -= new Vector2(0f, this.list.elementHeight * this.list.count + this.list.headerHeight + 3f);
                }
                ReorderableList.defaultBehaviours.DrawFooter(rect, list);
            };

            this.list.onChangedCallback += (index) =>
            {
                EditorUtility.SetDirty(_data);
            };
        }
    }
}
