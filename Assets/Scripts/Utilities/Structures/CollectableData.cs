using System;
using Entities;
using UnityEngine;

namespace Utilities.Structures
{
    [Serializable]
    public struct CollectableData
    {
        [SerializeField] private int _key;
        [SerializeField] private BaseCollectable[] _shards;

        public int Key => _key;
        public BaseCollectable Prefab => _shards.GetRandom();
    }
}