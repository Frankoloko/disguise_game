using UnityEngine;

namespace Weapons
{
    public class WeaponColliderHandler : MonoBehaviour
    {
        [SerializeField]
        private Collider weaponCollider;

        public void ToggleCollider()
        {
            weaponCollider.enabled = !weaponCollider.enabled;
        }
    }
}
