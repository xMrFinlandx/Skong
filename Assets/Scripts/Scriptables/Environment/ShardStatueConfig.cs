using NaughtyAttributes;
using UnityEngine;
using World.Environment;

namespace Scriptables.Environment
{
    [CreateAssetMenu(fileName = "New Shard Statue Config", menuName = "World/Shard Statue", order = 0)]
    public class ShardStatueConfig : ScriptableObject
    {
        [SerializeField, ShowAssetPreview] private Sprite _sprite;
        [Header("Settings")]
        [SerializeField] private int _hitsCount = 4;
        [SerializeField] private int _totalShardsAmount = 15;
        [Header("Shard Settings")]
        [SerializeField] private Shard _shardPrefab;
        [SerializeField] private float _spread;
        [SerializeField] private float _maxTorque;
        [SerializeField] private Vector2 _upwardForceRange;
        [Header("Shake Animation")]
        [SerializeField] private float _shakeTime = .5f;
        [SerializeField] private float _strength = .02f;
        [SerializeField] private int _vibration = 15;
        [Header("Shine Animation")]
        [SerializeField] private AnimationClip _idleClip;
        [SerializeField] private RuntimeAnimatorController _animatorController;

        public int HitsCount => _hitsCount;
        public int TotalShardsAmount => _totalShardsAmount;
        public int Vibration => _vibration;
        public int AnimationHash => Animator.StringToHash(_idleClip.name);
        
        public float Spread => _spread;
        public float MaxTorque => _maxTorque;
        public float ShakeTime => _shakeTime;
        public float Strength => _strength;
        
        public RuntimeAnimatorController AnimatorController => _animatorController;
        public Shard ShardPrefab => _shardPrefab;
        public Vector2 UpwardForceRange => _upwardForceRange;
        public Sprite Sprite => _sprite;
    }
}