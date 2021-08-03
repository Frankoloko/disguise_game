using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 1f;
        [SerializeField] private float turnSpeed = 0.15f;

        private Vector3 _direction;

        //Replace
        private void OnPause()
        {
            Application.Quit();
        }

        private void OnMove(InputValue value)
        {
            _direction = new Vector3(value.Get<Vector2>().x, 0, value.Get<Vector2>().y);
        }

        private void Update()
        {
            if (Player.PlayerDisguise.isDisguised)
            {
                return;
            }

            transform.Translate(_direction * (moveSpeed * Time.deltaTime), Space.World);

            if (_direction.x != 0 || _direction.z != 0)
            {
                transform.rotation =
                    Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_direction), turnSpeed);
            }
        }
    }
}