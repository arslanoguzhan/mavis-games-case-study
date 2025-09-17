using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace MavisCase.Games.TicTacToeGame
{
    public enum ItemKind
    {
        Circle = 0,
        Cross = 1
    }
    
    [CreateAssetMenu(fileName = "TicTacToeGameItemLookup", menuName = "ScriptableObjects/TicTacToeGame.ItemLookup")]
    public class ItemLookup : ScriptableObject
    {
        [SerializeField] private List<ItemEntry> _itemLookup;

        public GameObject GetPrefabByType(ItemKind kind)
        {
            return _itemLookup.FirstOrDefault(item => item.Kind == kind).Prefab;
        }
    }

    [Serializable]
    public struct ItemEntry
    {
        [FormerlySerializedAs("kind")]
        [FormerlySerializedAs("Type")]
        public ItemKind Kind;
        
        public GameObject Prefab;
    }
}