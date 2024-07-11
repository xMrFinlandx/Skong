using UnityEngine;

namespace World.Environment
{
    public class Shard : MonoBehaviour
    {
        [SerializeField] private int _value;
        [Space]
        [SerializeField] private Rigidbody2D _rigidbody;

        public Rigidbody2D Rigidbody => _rigidbody;
    }
}