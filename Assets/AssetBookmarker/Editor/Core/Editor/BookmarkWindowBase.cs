using UnityEditor;

namespace AssetBookmarker
{
    /// <summary>
    /// ウィンドウの基底クラス
    /// </summary>
    public abstract class BookmarkWindowBase : EditorWindow
    {
        private void OnEnable()
        {
            PackageConfig.CheckInstall();
        }
    }
}