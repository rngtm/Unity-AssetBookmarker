///-----------------------------------------
/// AssetBookmarker
/// @ 2016 RNGTM(https://github.com/rngtm)
///-----------------------------------------

using System.Runtime.CompilerServices;
using AssetBookmarker.Project;

namespace AssetBookmarker
{
    using System.IO;
    using UnityEngine;
    using UnityEditor;

    /// <summary>
    /// ファイルの作成を行うクラス
    /// </summary>
    public static class GenericDataGenerator
    {
        /// <summary>
        /// データ作成(Projectウィンドウで名前入力)
        /// </summary>
        public static T CreateData<T>(string dataName) where T : ScriptableObject
        {
            var directory = ExportConfig.GetDataExportDirectory();
            var path = directory + "/" + dataName + ".asset";
            Debug.Log(path);
            var instance = ScriptableObject.CreateInstance<T>();
            ProjectWindowUtil.CreateAsset(instance, path);

            return instance;
        }

        public static T CreateDataInProject<T>(string dataName) where T : ScriptableObject
        {
            T instance = null;
            string path = EditorUtility.SaveFilePanelInProject("New BookmarkData", dataName + ".asset", "asset", "Create New Bookmark Data");
            if (!string.IsNullOrEmpty(path))
            {
                instance = ScriptableObject.CreateInstance<T>();
                AssetDatabase.CreateAsset(instance, path);
                EditorGUIUtility.PingObject(instance);
            }
            return instance;
        }

        /// <summary>
        /// データ作成(名前入力無しですぐに作成)
        /// </summary>
        public static T CreateDataImmediately<T>(string dataName) where T : ScriptableObject
        {
            // return CreateDataImmediately_Project<T>(rootFolderName, saveFolderRelativePath, dataName);
            var directory = ExportConfig.GetDataExportDirectory();
            var path = Path.Combine(directory, dataName + ".asset");
            path = AssetDatabase.GenerateUniqueAssetPath(path);
            
            var instance = ScriptableObject.CreateInstance<T>();
            Debug.Log("Create: " + path, instance);

            AssetDatabase.CreateAsset(instance, path);
            
            EditorGUIUtility.PingObject(instance);

            return instance;
        }
        
        // /// <summary>
        // /// Projectディレクトリ以下へのデータ作成(名前入力無しですぐに作成)
        // /// </summary>
        // private static T CreateDataImmediately_Project<T>(string rootFolderName, string saveFolderRelativePath, string dataName) where T : ScriptableObject
        // {
        //     var guid = AssetDatabase.FindAssets(rootFolderName)[0];
        //     var rootDirectory = AssetDatabase.GUIDToAssetPath(guid);
        //     var directory = Path.Combine(rootDirectory, saveFolderRelativePath);
        //     if (string.IsNullOrEmpty(rootDirectory))
        //     {
        //         directory = "Assets";
        //     }
        //
        //     var name = dataName + ".asset";
        //     var path = Path.Combine(directory, name);
        //     var instance = ScriptableObject.CreateInstance<T>();
        //     AssetDatabase.CreateAsset(instance, path);
        //     Debug.Log("Create: " + path, instance);
        //
        //     return instance;
        // }
        //
        // /// <summary>
        // /// Packageディレクトリ以下へのデータ作成
        // /// </summary>
        // private static T CreateDataImmediately_Package<T>(string rootFolderName, string saveFolderRelativePath, string dataName) where T : ScriptableObject
        // {
        //     var guid = AssetDatabase.FindAssets(rootFolderName)[0];
        //     var rootDirectory = AssetDatabase.GUIDToAssetPath(guid);
        //     var directory = Path.Combine(rootDirectory, saveFolderRelativePath);
        //     if (string.IsNullOrEmpty(rootDirectory))
        //     {
        //         directory = "Assets";
        //     }
        //
        //     var name = dataName + ".asset";
        //     var path = Path.Combine(directory, name);
        //     var instance = ScriptableObject.CreateInstance<T>();
        //     AssetDatabase.CreateAsset(instance, path);
        //     Debug.Log("Create: " + path, instance);
        //
        //     return instance;
        // }
    }
}