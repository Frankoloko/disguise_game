using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerAttack : MonoBehaviour
    {
        [SerializeField] private Animator animatorController;

        private bool _isWeaponDrawn;
        private static readonly int DrawSword = Animator.StringToHash("DrawSword");
        private static readonly int SheatheSword = Animator.StringToHash("SheatheSword");
        private static readonly int MeleeAttack = Animator.StringToHash("MeleeAttack");

        private void OnDrawWeapon()
        {
            if (!_isWeaponDrawn)
            {
                animatorController.SetTrigger(DrawSword);
            }
            else
            {
                animatorController.SetTrigger(SheatheSword);
            }

            _isWeaponDrawn = !_isWeaponDrawn;
        }

        private void OnMeleeAttack(InputValue value)
        {
            if (!_isWeaponDrawn)
            {
                OnDrawWeapon();
            }
            else
            {
                animatorController.SetTrigger(MeleeAttack);
            }
        }
    }
}