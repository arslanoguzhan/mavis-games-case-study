using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace MavisCase.Games.MemoryGame
{
    public enum ItemKind
    {
        A = 65,
        B = 66,
        C = 67,
        D = 68,
        E = 69,
        F = 70,
        G = 71,
        H = 72,
        J = 74,
        K = 75,
        L = 76,
        M = 77,
        N = 78,
        P = 80,
        R = 82,
    }
    
    [CreateAssetMenu(fileName = "MemoryGameItemLookup", menuName = "ScriptableObjects/MemoryGame.ItemLookup")]
    public class ItemLookup : ScriptableObject
    {
        [SerializeField] private List<ItemEntry> _itemLookup;

        public GameObject GetPrefab(ItemKind kind)
        {
            return _itemLookup.FirstOrDefault(item => item.Kind == kind).Prefab;
        }
    }

    [Serializable]
    public struct ItemEntry
    {
        [FormerlySerializedAs("kind")] [FormerlySerializedAs("Type")]
        public ItemKind Kind;
        
        public GameObject Prefab;
    }
}