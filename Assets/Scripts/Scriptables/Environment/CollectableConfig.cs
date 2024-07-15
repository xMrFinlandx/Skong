using Entities;
using UnityEngine;
using Utilities;
using Random = UnityEngine.Random;

namespace Scriptables.Environment
{
    [CreateAssetMenu(fileName = "New Collectable Config", menuName = "World/Collectable Config", order = 0)]
    public class CollectableConfig : ScriptableObject 
    {
        [SerializeField] private BaseCollectable[] _shards;
        [SerializeField] private float _maxTorque;
        [SerializeField] private float _spread;
        [SerializeField] private Vector2 _upwardForceRange;

        public BaseCollectable Prefab => _shards.GetRandom();
        public float Torque => Random.Range(-_maxTorque, _maxTorque);
        public Vector2 Force => new(Random.Range(-_spread, _spread), Random.Range(_upwardForceRange.x, _upwardForceRange.y));
    }
}