using UnityEngine;

namespace Scriptables.Environment
{
    [CreateAssetMenu(fileName = "New Burst Config", menuName = "World/Burst Config", order = 0)]
    public class BurstConfig : ScriptableObject
    {
        [SerializeField] private float _maxTorque;
        [SerializeField] private float _spread;
        [SerializeField] private Vector2 _upwardForceRange;
        
        public float Torque => Random.Range(-_maxTorque, _maxTorque);
        public Vector2 Force => new(Random.Range(-_spread, _spread), Random.Range(_upwardForceRange.x, _upwardForceRange.y));
    }
}