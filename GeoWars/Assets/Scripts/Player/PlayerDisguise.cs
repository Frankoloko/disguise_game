using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerDisguise : MonoBehaviour
    {
        public GameObject PlayerBody;
        public float maxDistance = 2.5F;
        public static bool isDisguised = false;

        private Mesh PlayerOriginalMesh;
        private Vector3 PlayerOriginalScale;
        private bool showLine = false;
        private GameObject closestObject;

        void Start()
        {
            // Here we save the player's original body mesh and scale
            PlayerOriginalMesh = PlayerBody.GetComponent<MeshFilter>().mesh;
            PlayerOriginalScale = PlayerBody.transform.localScale;
        }

        void Update()
        {
            // Draw a line in debug mode to show which object is gettiing disguised to
            if (isDisguised)
            {
                Debug.DrawLine(PlayerBody.transform.position, closestObject.transform.position, Color.red);
            }
        }

        // ------------------ PRIVATE FUNCTIONS ------------------

        private void OnDisguise(InputValue value)
        {
            // JUST CHECK IF THE DISGUISE BUTTON WAS PRESSED OR RELEASED
            if (value.Get<float>() == 1.0)
            {
                PressDisguise();
            }
            else
            {
                ReleaseDisguise();
            }
        }

        private void PressDisguise()
        {
            // ON PressDisguise WE TRANSFORM THE PLAYER'S BODUY INTO THE NEAREST DISGUISE OBJECT

            // Get the closest object to disguise to
            GameObject[] objects = GameObject.FindGameObjectsWithTag("DisguiseObject");
            closestObject = GetClosestObject(objects);

            // Check of the closest object is within the maxDistance value
            float distance = Vector3.Distance(closestObject.transform.position, transform.position);
            if (distance < maxDistance)
            {
                isDisguised = true;

                // Set the player's mesh to the same as the closest object
                MeshFilter disguise_mesh_filter = closestObject.GetComponent<MeshFilter>();
                PlayerBody.GetComponent<MeshFilter>().sharedMesh = disguise_mesh_filter.mesh;

                // Set the player's scale to the same as the closest object
                PlayerBody.transform.localScale = closestObject.transform.localScale;

                // Uncommenting the below because it doesn't work like you'd expect. Regardless, I'm not sure if it will work at all since the objects might have different origins
                // Set the player's Y-Position to the same as the closest object
                // PlayerBody.transform.position = new Vector3(PlayerBody.transform.position.x, closestObject.transform.position.y, PlayerBody.transform.position.z);
            } else {
                Debug.Log($"Closest object distance ({distance}) is outside maxDistance value ({maxDistance})");
            }

        }

        private void ReleaseDisguise()
        {
            // ON ReleaseDisguise WE TRANSFORM THE PLAYER'S BODY BACK INTO THEIR ORIGINAL BODY

            isDisguised = false;

            PlayerBody.GetComponent<MeshFilter>().sharedMesh = PlayerOriginalMesh;
            PlayerBody.transform.localScale = PlayerOriginalScale;
        }

        private GameObject GetClosestObject(GameObject[] objects)
        {
            // If speed becomes an issue we can have a look at this script:
            // https://www.youtube.com/watch?v=6rDlfYC4HxM

            GameObject bestTarget = null;
            float closestDistanceSqr = Mathf.Infinity;
            Vector3 currentPosition = transform.position;

            foreach(GameObject potentialTarget in objects)
            {
                Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
                float dSqrToTarget = directionToTarget.sqrMagnitude;
                if(dSqrToTarget < closestDistanceSqr)
                {
                    closestDistanceSqr = dSqrToTarget;
                    bestTarget = potentialTarget;
                }
            }
        
            return bestTarget;
        }
    }
}