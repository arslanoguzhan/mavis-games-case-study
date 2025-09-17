using System;
using System.Collections.Generic;
using MavisCase.Common.Assets;
using MavisCase.Common.GridSystem;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MavisCase.Common.Pooling
{
    public class PoolService
    {
        private readonly Dictionary<Type, Queue<IPoolItem>> _pooledItems = new();
        private readonly HashSet<IPoolItem> _activeItems = new();

        private readonly IGamePrefix _gamePrefix;
        private readonly AssetManager _assetManager;

        public PoolService(IGamePrefix gamePrefix, AssetManager assetManager)
        {
            _gamePrefix = gamePrefix;
            _assetManager = assetManager;
        }

        public T GetPrefabAsset<T>(GameObject prefab) where T : Object, IPoolItem
        {
            var type = typeof(T);

            if (!_pooledItems.ContainsKey(type))
            {
                _pooledItems[type] = new Queue<IPoolItem>();
            }
            
            if (!_pooledItems[type].TryDequeue(out IPoolItem item))
            {
                item = GameObject.Instantiate(prefab).GetComponent<T>();
            }
            
            Activated(item);
            
            return item as T;
        }

        public T GetGameAsset<T>() where T : Object, IPoolItem
        {
            return GetAsset<T>(_gamePrefix.Prefix);
        }

        public T GetCommonAsset<T>() where T : Object, IPoolItem
        {
            return GetAsset<T>("Common");
        }

        private T GetAsset<T>(string directory) where T : Object, IPoolItem
        {
            var type = typeof(T);
            
            if (!_pooledItems.ContainsKey(type))
            {
                _pooledItems[type] = new Queue<IPoolItem>();
            }

            if (!_pooledItems[type].TryDequeue(out IPoolItem item))
            {
                GameObject prefab = _assetManager.GetAssetPrefab(directory, type.Name);
                item = GameObject.Instantiate(prefab).GetComponent<T>();
            }
            
            Activated(item);
            
            return item as T;
        }

        private void Activated(IPoolItem item)
        {
            _activeItems.Add(item);
        }
    
        public void Return<T>(T instance) where T : Object, IPoolItem
        {
            var type = typeof(T);
            instance.Recycle();
            _pooledItems[type].Enqueue(instance);
            _activeItems.Remove(instance);
        }

        public void ReturnAll()
        {
            foreach (var activeItem in _activeItems)
            {
                var type = activeItem.GetType();
        
                activeItem.Recycle();
                _pooledItems[type].Enqueue(activeItem);
            }
            
            _activeItems.Clear();
        }
    }
}