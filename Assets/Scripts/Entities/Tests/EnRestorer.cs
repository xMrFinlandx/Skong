using UnityEngine;

namespace Entities.Tests
{
    public class EnRestorer : MonoBehaviour, IEnergyRestorer
    {
        [SerializeField] private int _maxHealth = 10;
        [SerializeField] private int _energyToRestore = 1;
        [SerializeField] private int _contactDamage = 1;
        
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