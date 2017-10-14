///-----------------------------------------
/// AssetBookmarker
/// @ 2016 RNGTM(https://github.com/rngtm)
///-----------------------------------------
namespace AssetBookmarker.Project
{
    using System.Linq;
    using UnityEngine;
    using UnityEditor;
    using UnityEditor.Callbacks;
    using UnityEditorInternal;

    /// <summary>
    /// ブックマークの管理を行うウィンドウ
    /// </summary>
    public class ProjectBookmarkWindow : EditorWindow
    {
        /// <summary>
        /// Label領域の大きさ
        /// </summary>
        private const float LabelWidth = 68f;
        
        /// <summary>
        /// ボタンの大きさ
        /// </summary>
        private const float ButtonWidth = 46f;

        /// <summary>
        /// ポップアップ表示用の文字列
        /// </summary>
        private string[] popupDisplayedOptions;

        /// <summary>
        /// Bookmark表示用のReorderableList
        /// </summary>
        private ReorderableList bookmarkList;

        /// <summary>
        /// 現在選択しているブックマーク
        /// </summary>
        [SerializeField] private int currentBookmarkIndex = 0;

        /// <summary>
        /// 現在選択しているブックマークデータ名
        /// </summary>
        [SerializeField] private string currentBookmarkName;
        
        /// <summary>
        /// ブックマーク情報
        /// </summary>
        private ProjectBookmarkData[] bookmarkDatas;

        private static EditorWindow window;
        private static bool needReloadData = false;
        private static Object[] willRegisterAssets = null;

        /// <summary>
        /// アセットのロード時に呼ばれる
        /// </summary> 
        [DidReloadScripts]
        [InitializeOnLoadMethodAttribute]
        public static void OnLoadAssets()
        {
            needReloadData = true;
        }

        /// <summary>
        /// ウィンドウの描画処理
        /// </summary>
        private void OnGUI()
        {
            if (window == null)
            {
                window = this;
            }

            if (needReloadData)
            {
                needReloadData = false;
                this.ReloadDatas();
            }

            if (bookmarkDatas == null)
            {
                this.ReloadDatas();
            }

            if (this.bookmarkList == null)
            {
                this.RebuildBookmarkList();
            }
            
            if (willRegisterAssets != null)
            {
                var data = this.bookmarkDatas[this.currentBookmarkIndex]; 
                data.Assets.AddRange(willRegisterAssets);
                EditorUtility.SetDirty(data);
                willRegisterAssets = null;
            }

            EditorGUILayout.LabelField(Config.GUI_WINDOW_PROJECT_TEXT_OVERVIEW);
            EditorGUILayout.BeginHorizontal();
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.LabelField("Bookmark", GUILayout.Width(LabelWidth));
            int index = EditorGUILayout.Popup(this.currentBookmarkIndex, this.popupDisplayedOptions);
            if (EditorGUI.EndChangeCheck())
            {
                if (index < bookmarkDatas.Length)
                {
                    this.currentBookmarkIndex = index;
                    this.currentBookmarkName = this.bookmarkDatas[this.currentBookmarkIndex].name;
                    this.RebuildBookmarkList();
                }
                else
                {
                    DataGenerator.CreateBookmarkData();
                    this.ReloadDatas();
                    this.RebuildBookmarkList();
                }
            }
            if (GUILayout.Button("Select", EditorStyles.miniButton, GUILayout.Width(ButtonWidth)))
            {
                EditorGUIUtility.PingObject(bookmarkDatas[this.currentBookmarkIndex]);
            }
            EditorGUILayout.EndHorizontal();

            this.bookmarkList.DoLayoutList();

            CustomUI.VersionLabel();
        }

        /// <summary>
        /// ReorderableListを作成する
        /// </summary>
        private void RebuildBookmarkList()
        {
            this.bookmarkList = CreateBookmarkList(bookmarkDatas[this.currentBookmarkIndex]);
        }

        /// <summary>
        /// データのリロード
        /// </summary>
        private void ReloadDatas()
        {
            this.bookmarkDatas = DataLoader.LoadData();
            if (this.bookmarkDatas.Length == 0)
            {
                Debug.LogWarning("bookmark not found");
                DataGenerator.CreateBookmarkDataImmediately();
                this.bookmarkDatas = DataLoader.LoadData();
                this.RebuildBookmarkList();
            }

            this.popupDisplayedOptions = this.bookmarkDatas
            .Select(b => b.name)
            .Concat(new[] { "", "New..." }).ToArray();

            var selection = this.bookmarkDatas
            .Select((d,i) => new { Data = d, Index = i })
            .FirstOrDefault(item => item.Data.name == this.currentBookmarkName);

            if (selection != null)
            {
                this.currentBookmarkIndex = selection.Index;
            }
            else
            {
                // 直前に選択していたブックマークが見つからなかった場合は選択リセット
                this.currentBookmarkIndex = 0;
            }

            this.currentBookmarkName = this.bookmarkDatas[this.currentBookmarkIndex].name;
        }

        /// <summary>
        /// ReorderableListを作成する
        /// </summary>
        static private ReorderableList CreateBookmarkList(ProjectBookmarkData bookmarkData)
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
                    DoRemoveButton(list, index);
                    EditorUtility.SetDirty(bookmarkData);
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
        static private void DoRemoveButton(ReorderableList list, int index)
        {
            EditorApplication.delayCall += () =>
            {
                list.list.RemoveAt(index);
                window.Repaint();
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
            willRegisterAssets = Selection.objects;
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
