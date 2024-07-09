using UnityEngine;

namespace Entities
{
    public class D2 : MonoBehaviour, IEnergyRestorer
    {
        [SerializeField] private int _maxHealth;
        [SerializeField] private float _energyToRestore = 1;

        public float Value => _energyToRestore;
        
        private int _currentHealth;

        private void Start()
        {
            _currentHealth = _maxHealth;
        }

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
    }
}