using Entities;
using UnityEngine;

namespace Player
{
    public class PlayerStats : MonoBehaviour, IPlayerStats
    {
        public IWallet CoinsWallet { get; private set; }
        public IWallet ShardsWallet { get; private set; }

        private const int _SHARDS_LIMIT = 400;
        
        private void Start()
        {
            CoinsWallet = new Wallet();
            ShardsWallet = new LimitedWallet(_SHARDS_LIMIT);
        }
    }
}