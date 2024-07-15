using Scriptables.Environment;
using UnityEngine;

namespace Utilities
{
    public static class CollectablesSpawner
    {
        public static void Spawn(Vector2 position, CollectableConfig collectableConfig, int count)
        {
            for (int i = 0; i < count; i++)
            {
                var collectable = Object.Instantiate(collectableConfig.Prefab, position, Quaternion.identity);
                
                collectable.Rigidbody.AddTorque(collectableConfig.Torque);
                collectable.Rigidbody.AddForce(collectableConfig.Force, ForceMode2D.Impulse);
            }
        }
    }
}