using Stats;
using UnityEngine;

namespace Attacks
{
    public class BounceAttack : MonoBehaviour
    {
        private Rigidbody _rigidBody;

        [SerializeField] private BoxCollider playerCollider;
        [SerializeField] private float bounceVelocity = 5f;

        private GameObject _hitObject;

        private void Awake()
        {
            _rigidBody = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            if (IsEnemy() && _hitObject.GetComponent<IDamageable>() != null)
            {
                _rigidBody.velocity += Vector3.up * bounceVelocity;
                Invoke(nameof(DeliverDamage), 0.5f);
            }
        }

        private void DeliverDamage()
        {
            _hitObject.GetComponent<IDamageable>()?.TakeFullDamage();
        }

        private bool IsEnemy()
        {
            int layermask = 1 << 7;
            float distance = .2f;

            RaycastHit hitInfo;

            if (Physics.BoxCast(playerCollider.bounds.center, playerCollider.bounds.extents / 1.5f, Vector3.down,
                out hitInfo, transform.rotation, distance, layermask))
            {
                _hitObject = hitInfo.transform.gameObject;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}