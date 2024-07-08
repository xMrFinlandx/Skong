using UnityEngine;

namespace Player
{
    public interface IPlayerController
    {
        public Transform Transform { get; }
        public Rigidbody2D Rigidbody { get; }
        public Vector2 OverlapSize { get; }

        public void SetFacingDirection(bool isFacingRight);

        public bool CanClimb();
    }
}