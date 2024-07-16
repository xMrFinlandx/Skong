﻿using DG.Tweening;
using Entities;
using Scriptables.Environment;
using UnityEngine;
using Utilities;

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

        private int _leftShardsAmount;
        private int _currentHealth;
        private int _shardsPerHit;
        
        private int _hitsCount => _shardStatueConfig.HitsCount;
        private int _totalShardsAmount => _shardStatueConfig.TotalShardsAmount;
        private int _vibration => _shardStatueConfig.Vibration;
        
        private float _shakeTime => _shardStatueConfig.ShakeTime;
        private float _strength => _shardStatueConfig.Strength;
        
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
            _animator.PlayWithRandomStart(_shardStatueConfig.AnimationClip);
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
            
            var valueToSpawn = _currentHealth == 0 ? _leftShardsAmount : _shardsPerHit;
            _leftShardsAmount -= valueToSpawn;
            
            print(valueToSpawn);

            CollectablesSpawner.Spawn(transform.position, _shardStatueConfig.CollectableConfig, _shardStatueConfig.BurstConfig, valueToSpawn);
        }
        
        private void Shake()
        {
            transform.DOShakePosition(_shakeTime, _strength, _vibration, 0).SetLink(gameObject).OnComplete( () => {
                
            });
        }
    }
}