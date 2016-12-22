///-----------------------------------------
/// AssetBookmarker
/// @ 2016 RNGTM(https://github.com/rngtm)
///-----------------------------------------
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
        public static T CreateData<T>(string rootFolderName, string saveFolderRelativePath, string dataName) where T : ScriptableObject
        {
            var guid = AssetDatabase.FindAssets(rootFolderName)[0];
            var rootDirectory = AssetDatabase.GUIDToAssetPath(guid);
            var directory = Path.Combine(rootDirectory, saveFolderRelativePath);
            if (string.IsNullOrEmpty(rootDirectory))
            {
                directory = "Assets";
            }

            var name = dataName + ".asset";
            var path = Path.Combine(directory, name);
            var instance = ScriptableObject.CreateInstance<T>();
            ProjectWindowUtil.CreateAsset(instance, path);

            return instance;
        }

        public static T CreateDataImmediately<T>(string rootFolderName, string saveFolderRelativePath, string dataName) where T : ScriptableObject
        {
            var guid = AssetDatabase.FindAssets(rootFolderName)[0];
            var rootDirectory = AssetDatabase.GUIDToAssetPath(guid);
            var directory = Path.Combine(rootDirectory, saveFolderRelativePath);
            if (string.IsNullOrEmpty(rootDirectory))
            {
                directory = "Assets";
            }

            var name = dataName + ".asset";
            var path = Path.Combine(directory, name);
            var instance = ScriptableObject.CreateInstance<T>();
            AssetDatabase.CreateAsset(instance, path);
            Debug.Log("Create: " + path, instance);

            return instance;
        }
    }
}