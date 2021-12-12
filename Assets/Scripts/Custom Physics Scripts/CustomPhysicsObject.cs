using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomPhysicsObject : MonoBehaviour
{
    [Header("Properties")]
    public float mass;
    public float bounciness = 0.5f;
    public Material material;

    [Header("Movement")]
    public bool motionless = false;
    public Vector3 velocity = Vector3.zero;
    public Vector3 acceleration = Vector3.zero;
    public bool useSystemGravity = true;

    [Header("Collision")]
    public CollisionType collisionType;
    bool hasCollision = false;

    // Are used for resetting the scene
    Vector3 initialPosition;
    Vector3 initialVelocity;
    Vector3 initialAcceleration;

    //A placeholder for the Y velocity when gravity is disabled
    float tempYVelocityHolder;

    //Used for friction
    CustomPhysicsObject groundObject;

    // Start is called before the first frame update
    void Start()
    {
        //Automatically add object to list
        FindObjectOfType<CustomPhysicsSystem>().objectsList.Add(this);

        //Set initial values in case simulation is reset
        initialPosition = transform.position;
        initialVelocity = velocity;
        initialAcceleration = acceleration;
    }

    //Use System Gravity Flag
    public bool UseSystemGravity()
    {
        return useSystemGravity;
    }

    //Has Collision Flag
    public bool HasCollision()
    {
        return hasCollision;
    }

    //Set Has Collision Flag
    public void SetHasCollision(bool flag)
    {
        hasCollision = flag;
    }


    // Get the ground object of this current physics object
    public CustomPhysicsObject GetGroundObject()
    {
        return groundObject;
    }

    // Set the ground object of this current physics object
    public void SetGroundObject(CustomPhysicsObject gObject)
    {
        groundObject = gObject;
    }

    //Toggles Y movement and sets it back once enabled again
    public void ToggleGravity(bool toggle)
    {
        if (!toggle)
        {
            tempYVelocityHolder = velocity.y;
            velocity.y = 0;
        }
        else
            velocity.y = tempYVelocityHolder;
    }

    //Resets the Object back to its initial State

    public void ResetObjectState()
    {
        transform.position = initialPosition;
        velocity = initialVelocity;
        acceleration = initialAcceleration;
    }

    //===== Initial Value Getters and Setters =====
    public Vector3 GetInitialPosition() { return initialPosition; }
    public Vector3 GetInitialVelocity() { return initialVelocity; }
    public Vector3 GetInitialAcceleration() { return initialAcceleration; }
}
