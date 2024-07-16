using Scriptables.Environment;
using UnityEngine;
using Utilities;

namespace Entities.Tests
{
    public class EnRestorer : MonoBehaviour, IEnergyRestorer
    {
        [SerializeField] private int _maxHealth = 10;
        [SerializeField] private int _energyToRestore = 1;
        [SerializeField] private int _contactDamage = 1;
        [Space] 
        [SerializeField] private int _rosaryAmount = 5;
        [SerializeField] private int _shardsAmount = 2;
        [SerializeField] private CollectableConfig _rosaryConfig;
        [SerializeField] private CollectableConfig _shardsConfig;
        [SerializeField] private BurstConfig _burstConfig;
        
        public int Value => _energyToRestore;

        private int _currentHealth;
        
        public void ApplyDamage(int damage)
        {
            _currentHealth -= damage;

            if (_currentHealth <= 0)
                Kill();
        }

        public void Kill()
        {
            CollectablesSpawner.Spawn(transform.position, _rosaryConfig, _burstConfig, _rosaryAmount);
            CollectablesSpawner.Spawn(transform.position, _shardsConfig, _burstConfig, _shardsAmount);
            Destroy(gameObject);
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<IDamageable>(out var target))
            {
                target.ApplyDamage(_contactDamage);
            }
        }

        private void Start()
        {
            _currentHealth = _maxHealth;
        }
    }
}