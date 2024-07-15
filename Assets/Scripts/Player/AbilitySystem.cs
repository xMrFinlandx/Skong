using System.Linq;
using Entities;
using Player.Controls;
using UnityEngine;
using Utilities;
using Zenject;

namespace Player
{
    public class AbilitySystem : MonoBehaviour
    {
        [Header("Combat Attack")]
        [SerializeField] private float _xOffset;
        [SerializeField] private float _yOffset;
        [SerializeField] private float _range;
        [SerializeField] private LayerMask _enemyLayerMask;

        private InputReader _inputReader;

        private Vector2 _attackDirection;
        private Vector2 _cashedInputDirection;

        private IPlayerController _playerController;
        private IPlayerStats _playerStats;

        private Vector2 _attackOverlapCenter => transform.position + (Vector3) (_attackDirection * new Vector3(_xOffset, _yOffset));
        
        [Inject]
        private void Construct(InputReader inputReader)
        {
            _inputReader = inputReader;
        }

        private void Start()
        {
            _inputReader.AttackPerfomedEvent += OnAttack;
            _inputReader.MovePerfomedEvent += OnDirectionInput;
            _inputReader.MoveCancelledEvent += OnDirectionInputCancelled;
            
            _playerController = GetComponent<IPlayerController>();
            _playerStats = GetComponent<IPlayerStats>();
            
            _playerController.IsGrounded.ValueChangedEvent += OnGroundedChanged;
        }

        private void OnGroundedChanged(bool previous, bool isGrounded)
        {
            if (isGrounded)
            {
                _attackDirection = _cashedInputDirection.y > 0
                    ? _cashedInputDirection
                    : new Vector2(_playerController.FacingDirectionModifier, 0);
            }
            else
            {
                _attackDirection = _cashedInputDirection;
            }
        }

        private void OnDirectionInputCancelled()
        {
            _cashedInputDirection = new Vector2(_playerController.FacingDirectionModifier, 0);
            _attackDirection = _cashedInputDirection;
        }

        private void OnDirectionInput(Vector2 direction)
        {
            if (direction == Vector2.zero)
                return;

            _cashedInputDirection = direction.To4DVector();
            _attackDirection = GetAttackDirection(_cashedInputDirection);
        }

        private Vector2 GetAttackDirection(Vector2 direction)
        {
            if (_playerController.IsGrounded.Value && direction.y < 0)
            {
                return new Vector2(_playerController.FacingDirectionModifier, 0);
            }

            return direction;
        }

        private void OnAttack()
        {
            var damageableList = OverlapHelper.GetComponentsInCircle<IDamageable>(_attackOverlapCenter, _range, _enemyLayerMask);

            foreach (var damageable in damageableList)
            {
                damageable.ApplyDamage(1);
            }
            
            var sum = damageableList.OfType<IEnergyRestorer>().Sum(item => item.Value);
            _playerStats.EnergyWallet.Add(sum);
        }

        private void OnDestroy()
        {
            _inputReader.AttackPerfomedEvent -= OnAttack;
            _inputReader.MovePerfomedEvent -= OnDirectionInput;
            _inputReader.MoveCancelledEvent -= OnDirectionInputCancelled;
            
            _playerController.IsGrounded.ValueChangedEvent -= OnGroundedChanged;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(_attackOverlapCenter, _range);
        }
    }
}