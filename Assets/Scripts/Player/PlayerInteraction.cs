using Entities;
using UnityEngine;
using Utilities;

namespace Player
{
    public class PlayerInteraction : MonoBehaviour
    {
        [SerializeField] private float _overlapRange;
        [SerializeField] private LayerMask _layerMask;

        [SerializeField] private IPlayerStats _playerStats;

        private void OnValidate()
        {
            _playerStats ??= GetComponent<PlayerStats>();
        }

        private void FixedUpdate()
        {
            var collectables = OverlapHelper.GetComponentsInCircle<BaseCollectable>(transform.position, _overlapRange, _layerMask);

            foreach (var collectable in collectables)
            {
                collectable.Collect(_playerStats);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, _overlapRange);
        }
    }
}