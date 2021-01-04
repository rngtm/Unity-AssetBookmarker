using System.Linq;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

namespace AssetBookmarker
{
    public static class PackageConfig
    {
        public const string PackageName = "com.rngtm.asset-bookmarker";
        private static bool isInstallInstallPackage = false;
        private static ListRequest Request;
        private static bool isDebugLog = false;
        
        public static string GetPackagePath(string path) => $"Packages/{PackageName}/{path}";
        public static bool IsInstallPackage => isInstallInstallPackage;

        [MenuItem(MenuConfig.TOOL_MENU_ROOT + "Check Package Installation", priority = 100)]
        static void CheckInstallDebug()
        {
            isDebugLog = true;
            CheckInstall();
        }

        [MenuItem(MenuConfig.TOOL_MENU_ROOT + "Test Path", priority = 100)]
        static void CheckPackagePath()
        {
            UnityEngine.Debug.Log(GetPackagePath("DataPath"));
        }

        [DidReloadScripts]
        public static void CheckInstall()
        {
            Request = Client.List();
            EditorApplication.update += Progress;
        }

        static void Progress()
        {
            if (Request.IsCompleted)
            {
                if (Request.Status == StatusCode.Success)
                {
                    if (Request.Result.Select(q => q.name).Contains(PackageName))
                    {
                        if (isDebugLog) 
                            Debug.Log($"Is Install : {PackageName}");
                        isInstallInstallPackage = true;
                    }
                    else
                    {
                        if (isDebugLog) 
                            Debug.Log($"Is Not Install : {PackageName}");
                        isInstallInstallPackage = false;
                    }
                }
                else if (Request.Status >= StatusCode.Failure)
                {
                    Debug.Log(Request.Error.message);
                }

                EditorApplication.update -= Progress;
                isDebugLog = false;
            }
        }
    }
}