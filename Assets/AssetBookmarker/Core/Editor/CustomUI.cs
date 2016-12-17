///-----------------------------------------
/// AssetBookmarker
/// @ 2016 RNGTM(https://github.com/rngtm)
///-----------------------------------------
namespace AssetBookmarker
{
    using UnityEngine;

    public class CustomUI
    {
        private static GUIStyle _versionLabelStyle;

        private static GUIStyle VersionLabelStyle { get { return _versionLabelStyle ?? (_versionLabelStyle = CreateVersionLabelStyle()); } }

        public static void VersionLabel()
        {
            GUI.Label(new Rect(-2, Screen.height - 72, Screen.width, 50), Config.VERSION_TEXT, VersionLabelStyle);
        }

        private static GUIStyle CreateVersionLabelStyle()
        {
            var style = new GUIStyle(GUI.skin.GetStyle("Label"));
            var color = new Color(style.normal.textColor.r, style.normal.textColor.g, style.normal.textColor.b, 0.4f);

            style.alignment = TextAnchor.LowerRight;
            style.normal.textColor = color;

            return style;
        }
    }
}
