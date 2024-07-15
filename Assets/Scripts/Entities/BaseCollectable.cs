using System;
using UnityEngine;
using Utilities;
using Random = UnityEngine.Random;

namespace Entities
{
    [RequireComponent(typeof(Animator))]
    public abstract class BaseCollectable : MonoBehaviour
    {
        [SerializeField] private int _value;
        [SerializeField] private float _enableTime = .3f;
        [SerializeField] private AnimationClip _animationClip;
        [Space]
        [SerializeField] private Collider2D _collectCollider;
        [SerializeField] private Animator _animator;
        [SerializeField] private Rigidbody2D _rigidbody;
        
        private float _enableTimeCounter;
        
        public Rigidbody2D Rigidbody => _rigidbody;
        protected int Value => _value;

        public abstract void Collect(IPlayerStats playerStats);
        
        private void Start()
        {
            _collectCollider.enabled = false;
            _enableTimeCounter = _enableTime;
            
            _animator.PlayWithRandomStart(_animationClip);
        }
        
        private void Update()
        {
            _enableTimeCounter -= Time.deltaTime;

            if (_enableTimeCounter > 0)
                return;

            _collectCollider.enabled = true;
        }
    }
}