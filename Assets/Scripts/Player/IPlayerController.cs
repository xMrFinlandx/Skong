using UnityEngine;
using Utilities.Classes;

namespace Player
{
    public interface IPlayerController
    {
        public ReactiveProperty<bool> IsGrounded { get; }
        public ReactiveProperty<float> HorizontalCashedDirection { get; }
        
        public bool IsFacingRight { get; }
        public int FacingDirectionModifier { get; }

        public Transform Transform { get; }
        public Rigidbody2D Rigidbody { get; }
        public Vector2 OverlapSize { get; }

        public void SetFacingDirection(bool isFacingRight);

        public bool CanClimb();
    }
}