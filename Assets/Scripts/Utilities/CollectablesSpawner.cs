using Scriptables.Environment;
using UnityEngine;

namespace Utilities
{
    public static class CollectablesSpawner
    {
        public static void Spawn(Vector2 position, CollectableConfig collectableConfig, BurstConfig burstConfig, int value)
        {
            var coins = CoinChanger.GetCoins(value, collectableConfig.PossibleValues);

            foreach (var coin in coins)
            {
                var collectable = Object.Instantiate(collectableConfig.GetPrefab(coin), position, Quaternion.identity);
                
                collectable.Rigidbody.AddTorque(burstConfig.Torque);
                collectable.Rigidbody.AddForce(burstConfig.Force, ForceMode2D.Impulse);
                collectable.SpriteRenderer.sortingOrder = Random.Range(0, 100);
            }
        }
    }
}