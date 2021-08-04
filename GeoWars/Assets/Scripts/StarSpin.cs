using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StarSpin: MonoBehaviour
{
    public float rotateSpeed = 60.0F;

    void Update()
    {
        // JUST SPIN THE OBJECT AROUND CONSTANTLY
        transform.Rotate(0, 0, Time.deltaTime * rotateSpeed);
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check which object collided with this one
        if (collision.gameObject.name == "Player")
        {
            Destroy(gameObject);            
            GameManager.IncreaseCollectedStars();
        }
    }
}