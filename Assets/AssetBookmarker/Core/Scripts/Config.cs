///-----------------------------------------
/// AssetBookmarker
/// @ 2016 RNGTM(https://github.com/rngtm)
///-----------------------------------------
namespace AssetBookmarker
{
    using UnityEngine;

    /// <summary>
    /// 設定
    /// </summary>
    public static class Config
    {
        // バージョン情報
        public const string VERSION_TEXT = "Asset Bookmarker v2.2";

        // Menuテキスト
        public const string GUI_MENU_TEXT_OPEN_BOOKMARK_PROJECT = "Tools/Asset Bookmarker/Assets";
        public const string GUI_MENU_TEXT_OPEN_BOOKMARK_HIERARCHY = "Tools/Asset Bookmarker/Hierarchy";
        public const string GUI_MENU_TEXT_REGISTER_PROJECT = "Assets/Add to bookmark list";
        public const string GUI_MENU_TEXT_REGISTER_HIERARCHY = "GameObject/Add to bookmark list...";

        // ウィンドウ内のGUIテキスト (Project Bookmark Window)
        public const string GUI_WINDOW_PROJECT_TEXT_TITLE = "Asset Bookmarker";
        public const string GUI_WINDOW_PROJECT_TEXT_OVERVIEW = "ブックマークしたアセット一覧を表示します";
        public const string GUI_WINDOW_PROJECT_TEXT_LIST_HEADER = "Asset Bookmarks";
        public const string GUI_WINDOW_PROJECT_TEXT_ASSET_REMOVE_BUTTON = "-";

        // ウィンドウ内のGUIテキスト (Hierarchy Bookmark Window)
        public const string GUI_WINDOW_HIERARCHY_TEXT_TITLE = "Asset Bookmarker";
        public const string GUI_WINDOW_HIERARCHY_TEXT_OVERVIEW = "ヒエラルキーの検索文字列一覧を表示します";
        public const string GUI_WINDOW_HIERARCHY_TEXT_LIST_HEADER = "Hierarchy Search Filters";
        public const string GUI_WINDOW_HIERARCHY_TEXT_FILTER_APPLY_BUTTON = "Apply";
        public const string GUI_WINDOW_HIERARCHY_TEXT_FILTER_REMOVE_BUTTON = "-";
    }
}
