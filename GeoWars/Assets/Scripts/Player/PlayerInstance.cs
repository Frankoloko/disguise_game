using UnityEngine;

namespace Player
{
    public class PlayerInstance : MonoBehaviour
    {
        public static PlayerInstance Instance;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
