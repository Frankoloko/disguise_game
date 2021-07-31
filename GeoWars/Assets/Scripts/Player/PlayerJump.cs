using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerJump : MonoBehaviour
    {
        private Rigidbody _rigidBody;

        private bool _isJumping;

        [SerializeField] private BoxCollider playerCollider;
        [SerializeField] private float jumpVelocity = 5f;
        [SerializeField] private float fallMultiplier = 2.5f;
        [SerializeField] private float jumpMultiplier = 2.5f;

        private void Awake()
        {
            _rigidBody = GetComponent<Rigidbody>();
        }

        private void OnJump(InputValue value)
        {
            _isJumping = Convert.ToBoolean(value.Get<float>());

            if (_isJumping && IsGrounded())
            {
                _rigidBody.velocity += Vector3.up * jumpVelocity;
            }
        }

        private void FixedUpdate()
        {
            if (_rigidBody.velocity.y < 0)
            {
                _rigidBody.velocity += Vector3.up * (Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime);
            }
            else if (_rigidBody.velocity.y > 0 && !_isJumping)
            {
                _rigidBody.velocity += Vector3.up * (Physics.gravity.y * (jumpMultiplier - 1) * Time.deltaTime);
            }
        }

        private bool IsGrounded()
        {
            const int layermask = 1 << 6;
            const float distance = .2f;

            var bounds = playerCollider.bounds;
            return Physics.BoxCast(bounds.center, bounds.extents / 1.5f, Vector3.down, transform.rotation, distance,
                layermask);
        }
    }
}