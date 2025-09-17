using System.Collections.Generic;
using UnityEngine;

namespace MavisCase.Common.Assets
{
    public class AssetManager
    {
        private Dictionary<string, Object> _assetCache = new Dictionary<string, Object>();
        private Dictionary<string, GameObject> _assetDict = new ();
    
        public T GetAsset<T>(string directory) where T : Object
        {
            var assetPath = $"{directory}/{typeof(T).Name}";
            if (_assetCache.ContainsKey(assetPath))
            {
                return (T)_assetCache[assetPath];
            }
        
            var asset = Resources.Load<T>(assetPath);
            _assetCache.Add(assetPath, asset);
            return asset;
        }
        
        public GameObject GetAssetPrefab(string directory, string name)
        {
            var assetPath = $"{directory}/{name}";
            if (_assetDict.ContainsKey(assetPath))
            {
                return _assetDict[assetPath];
            }
        
            var asset = Resources.Load<GameObject>(assetPath);
            _assetDict.Add(assetPath, asset);
            return asset;
        }
    }

}