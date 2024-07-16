using NaughtyAttributes;
using UnityEngine;

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
        [SerializeField, Expandable] private CollectableConfig _collectableConfig;
        [SerializeField, Expandable] private BurstConfig _burstConfig;
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
        
        public float ShakeTime => _shakeTime;
        public float Strength => _strength;
        
        public RuntimeAnimatorController AnimatorController => _animatorController;
        public Sprite Sprite => _sprite;
        public CollectableConfig CollectableConfig => _collectableConfig;
        public BurstConfig BurstConfig => _burstConfig;
        public AnimationClip AnimationClip => _idleClip;
    }
}