///-----------------------------------------
/// AssetBookmarker
/// @ 2016 RNGTM(https://github.com/rngtm)
///-----------------------------------------
namespace AssetBookmarker.Hierarchy
{
    using UnityEngine;
    using System.Collections.Generic;

    /// <summary>
    /// ブックマーク情報
    /// </summary>
    public class HierarchyBookmarkData : ScriptableObject
    {
        [SerializeField] private List<SearchInfo> searchInfos = new List<SearchInfo>();

        /// <summary>
        /// 検索情報
        /// </summary>
        public List<SearchInfo> SearchInfos { get { return this.searchInfos; } }
    }
}
