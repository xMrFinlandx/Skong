using DG.Tweening;
using Entities;
using Scriptables.Environment;
using UnityEngine;

namespace World.Environment
{
    [RequireComponent(typeof(BoxCollider2D), typeof(Animator), typeof(SpriteRenderer))]
    public class ShardStatue : MonoBehaviour, IDamageable
    {
        [SerializeField] private ShardStatueConfig _shardStatueConfig;
        [Space]
        [SerializeField] private BoxCollider2D _collider;
        [SerializeField] private Animator _animator;
        [SerializeField] private SpriteRenderer _spriteRenderer;

        private int _hitsCount => _shardStatueConfig.HitsCount;
        private int _totalShardsAmount => _shardStatueConfig.TotalShardsAmount;
        private int _vibration => _shardStatueConfig.Vibration;

        private float _spread => _shardStatueConfig.Spread;
        private float _maxTorque => _shardStatueConfig.MaxTorque;
        private float _shakeTime => _shardStatueConfig.ShakeTime;
        private float _strength => _shardStatueConfig.Strength;
        
        private Shard _shardPrefab => _shardStatueConfig.ShardPrefab;
        private Vector2 _upwardForceRange => _shardStatueConfig.UpwardForceRange;
        
        private int _leftShardsAmount;
        private int _currentHealth;
        private int _shardsPerHit;
        
        public void ApplyDamage(int _)
        {
            _currentHealth--;

            SpawnShards();
            Shake();
            
            if (_currentHealth <= 0)
                Kill();
        }

        public void Kill()
        {
            Destroy(gameObject);
        }

        private void OnValidate()
        {
            _animator ??= GetComponent<Animator>();
            _collider ??= GetComponent<BoxCollider2D>();
            _spriteRenderer ??= GetComponent<SpriteRenderer>();

            if (_shardStatueConfig == null)
                return;

            _spriteRenderer.sprite = _shardStatueConfig.Sprite;
            _animator.runtimeAnimatorController = _shardStatueConfig.AnimatorController;
            _animator.Play(_shardStatueConfig.AnimationHash);
            _collider.isTrigger = true;
        }

        private void Start()
        {
            _currentHealth = _hitsCount;
            _shardsPerHit = _totalShardsAmount / _hitsCount;
            _leftShardsAmount = _totalShardsAmount;
        }

        private void SpawnShards()
        {
            if (_leftShardsAmount == 0)
                return;
            
            var count = _currentHealth == 0 ? _leftShardsAmount : _shardsPerHit;
            _leftShardsAmount -= count;
            
            for (int i = 0; i < count ; i++)
            {
                var shard = Instantiate(_shardPrefab, transform.position, Quaternion.identity);

                var range = Random.Range(-_spread, _spread);
                var torque = Random.Range(-_maxTorque, _maxTorque);
                var upwardForce = Random.Range(_upwardForceRange.x, _upwardForceRange.y); 
                var force = new Vector2(range, upwardForce);
                
                shard.Rigidbody.AddTorque(torque);
                shard.Rigidbody.AddForce(force, ForceMode2D.Impulse);
            }
        }
        
        private void Shake()
        {
            transform.DOShakePosition(_shakeTime, _strength, _vibration, 0).SetLink(gameObject).OnComplete( () => {
                
            });
        }
    }
}