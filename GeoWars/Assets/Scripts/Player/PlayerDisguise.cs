using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDisguise : MonoBehaviour
{

    public GameObject PlayerBody;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDisguise(InputValue value)
    {
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

    private void OnReleaseDisguise(InputValue value)
    {
        Debug.Log("Released!");
    }
}
