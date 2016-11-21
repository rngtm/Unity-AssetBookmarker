///-----------------------------------
/// AssetBookmarker
/// @ 2016 RNGTM(https://github.com/rngtm)
///-----------------------------------
namespace AssetBookmarker
{
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// Bookmarkのデータ
    /// </summary>
    public class BookmarkData : ScriptableObject
    {
        [SerializeField]
        private List<Object> assets;

        /// <summary>
        /// ブックマークとして登録しているアセット
        /// </summary>
        public List<Object> Assets { get { return this.assets; } }

        /// <summary>
        /// アセットをブックマークへ登録
        /// </summary>
        public void Register(Object asset)
        {
            this.assets.Add(asset);
        }
    }
}
