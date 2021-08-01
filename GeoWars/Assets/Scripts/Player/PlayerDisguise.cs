using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDisguise : MonoBehaviour
{
    public GameObject PlayerBody;
    private Mesh PlayerOriginalMesh;
    private Vector3 PlayerOriginalScale;

    void Start()
    {
        // Here we save the player's original body mesh and scale
        PlayerOriginalMesh = PlayerBody.GetComponent<MeshFilter>().mesh;
        PlayerOriginalScale = PlayerBody.transform.localScale;
    }

    private void OnDisguise(InputValue value)
    {
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
        Player.PlayerMovement.disableMovement = true;

        // On PressDisguise, we transform the player's body into the nearest disguise object

        // Get the closest object to disguise to
        GameObject[] enemies = {GameObject.Find("Jerry"), GameObject.Find("Cylinder_01"), GameObject.Find("Sphere")};
        GameObject closet_object = GetClosestObject(enemies);

        // Set the player's mesh to the same as the closest object
        MeshFilter disguise_mesh_filter = closet_object.GetComponent<MeshFilter>();
        PlayerBody.GetComponent<MeshFilter>().sharedMesh = disguise_mesh_filter.mesh;

        // Set the player's scale to the same as the closest object
        PlayerBody.transform.localScale = closet_object.transform.localScale;

        // Uncommenting the below because it doesn't work like you'd expect. Regardless, I'm not sure if it will work at all since the objects might have different origins
        // Set the player's Y-Position to the same as the closest object
        // PlayerBody.transform.position = new Vector3(PlayerBody.transform.position.x, closet_object.transform.position.y, PlayerBody.transform.position.z);
    }

    private void ReleaseDisguise()
    {
        Player.PlayerMovement.disableMovement = false;

        // On ReleaseDisguise, we transform the player's body back into it's original body
        PlayerBody.GetComponent<MeshFilter>().sharedMesh = PlayerOriginalMesh;
        PlayerBody.transform.localScale = PlayerOriginalScale;
    }

    GameObject GetClosestObject(GameObject[] enemies)
    {
        GameObject bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        foreach(GameObject potentialTarget in enemies)
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
