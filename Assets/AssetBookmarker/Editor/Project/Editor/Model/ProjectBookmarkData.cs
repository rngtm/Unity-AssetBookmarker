///-----------------------------------------
/// AssetBookmarker
/// @ 2016 RNGTM(https://github.com/rngtm)
///-----------------------------------------
namespace AssetBookmarker.Project
{
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// Bookmarkのデータ
    /// </summary>
    public class ProjectBookmarkData : ScriptableObject
    {
        [SerializeField] private List<Object> assets = new List<Object>();

        /// <summary>
        /// ブックマークとして登録しているアセット
        /// </summary>
        public List<Object> Assets { get { return this.assets; } }
    }
}