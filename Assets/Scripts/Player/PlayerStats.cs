using Entities;
using Scriptables.Player;
using UnityEngine;

namespace Player
{
    public class PlayerStats : MonoBehaviour, IPlayerStats, IDamageable
    {
        [SerializeField] private int _maxHealth = 5;
        [Header("Crest")] 
        [SerializeField] private CrestConfig _crestConfig;
        
        public IWallet CoinsWallet { get; private set; }
        public IWallet ShardsWallet { get; private set; }
        public IWallet EnergyWallet { get; private set; }

        private const int _SHARDS_LIMIT = 400;

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
        
        private void Start()
        {
            CoinsWallet = new Wallet();
            ShardsWallet = new LimitedWallet(_SHARDS_LIMIT);
            EnergyWallet = new LimitedWallet(_crestConfig.MaxEnergyAmount);

            EnergyWallet.BalanceChangedAction += OnEnergyBalanceChanged;

            _currentHealth = _maxHealth;
        }

        private void OnEnergyBalanceChanged(int balance, int added)
        {
            Debug.Log($"Balance: {balance}, Added {added}");
        }

        private void OnDisable()
        {
            EnergyWallet.BalanceChangedAction -= OnEnergyBalanceChanged;
        }
    }
}