using System.Collections.Generic;
using System.Linq;
using Entities;
using UnityEngine;
using Utilities.Structures;

namespace Scriptables.Environment
{
    [CreateAssetMenu(fileName = "New Collectable Config", menuName = "World/Collectable Config", order = 0)]
    public class CollectableConfig : ScriptableObject 
    {
        [SerializeField] private CollectableData[] _shards;
        
        public IList<int> PossibleValues => _shards.Select(item => item.Key).OrderByDescending(key => key).ToArray();

        public BaseCollectable GetPrefab(int value) => _shards.FirstOrDefault(item => item.Key == value).Prefab;
    }
}