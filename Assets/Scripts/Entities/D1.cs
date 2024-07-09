using System;
using UnityEngine;

namespace Entities
{
    public class D1 : MonoBehaviour, IDamageable
    {
        [SerializeField] private int _maxHealth;
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