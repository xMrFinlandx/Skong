using Entities;
using UnityEngine;

namespace World.Environment
{
    public class Shard : MonoBehaviour, ICollectable
    {
        [SerializeField] private int _value;
        [Space]
        [SerializeField] private Rigidbody2D _rigidbody;

        public Rigidbody2D Rigidbody => _rigidbody;
        
        public void Collect(IPlayerStats playerStats)
        {
            playerStats.ShardsWallet.Add(_value);
            Destroy(gameObject);
        }
    }
}