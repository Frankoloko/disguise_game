using UnityEngine;

namespace Weapons
{
    public class Weapon : MonoBehaviour
    {
        [Tooltip("Mark this as true if this weapon is held by the player, or false if it will be used by an enemy")]
        [SerializeField] private bool playerWeapon = true;
        [SerializeField] private float force = 10f;

        private float _weaponDamage = 10f;
        private string _colliderTag;

        private void Awake()
        {
            _colliderTag = playerWeapon ? "Enemy" : "Player";
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(_colliderTag))
            {
                try
                {
                    Transform transformOfObject = transform;
                    Transform transformOfCollidedObject = other.transform;

                    transformOfCollidedObject.parent.GetComponent<Health>().TakeDamage(_weaponDamage);

                    Vector3 direction = transformOfCollidedObject.position - transform.position;
                    direction.y = 0;
                    direction /= direction.magnitude;

                    other.transform.parent.GetComponent<Rigidbody>()
                        .AddForceAtPosition(direction * force, transformOfObject.position);
                }
                catch (System.Exception e)
                {
                    Debug.LogError(e);
                }
            }
        }
    }
}