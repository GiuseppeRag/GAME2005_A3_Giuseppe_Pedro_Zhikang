using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomPhysicsSystem : MonoBehaviour
{
    public List<CustomPhysicsObject> objectsList = new List<CustomPhysicsObject>();
    public float gravity = 9.8f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void UpdateMovement()
    {
        foreach (CustomPhysicsObject physicsObject in objectsList)
        {
            physicsObject.velocity += physicsObject.acceleration * Time.deltaTime;
            physicsObject.transform.position += physicsObject.velocity * Time.deltaTime;
        }
    }

    void UpdateCollision()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdateMovement();
        UpdateCollision();
    }
}
