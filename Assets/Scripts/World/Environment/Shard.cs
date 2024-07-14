using Entities;
using UnityEngine;

namespace World.Environment
{
    public class Shard : MonoBehaviour, ICollectable
    {
        [SerializeField] private int _value;
        [SerializeField] private float _enableTime = .5f;
        [Space]
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private Collider2D _collectCollider;

        private float _enableTimeCounter;
        
        public Rigidbody2D Rigidbody => _rigidbody;
        
        public void Collect(IPlayerStats playerStats)
        {
            playerStats.ShardsWallet.Add(_value);
            Destroy(gameObject);
        }

        private void Start()
        {
            _collectCollider.enabled = false;
            _enableTimeCounter = _enableTime;
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