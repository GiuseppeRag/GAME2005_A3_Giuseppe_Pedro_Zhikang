using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomPhysicsObject : MonoBehaviour
{
    [Header("Properties")]
    public float mass;
    public bool motionless = false;
    public Vector3 velocity = Vector3.zero;
    public Vector3 acceleration = Vector3.zero;
    public bool useSystemGravity = true;
    public bool autoDetermineMass = false;

    [Header("Collision")]
    public CollisionType collisionType;
    bool hasCollision = false;
    bool isGrounded = false;

    // Are used for resetting the scene
    Vector3 initialPosition;
    Vector3 initialVelocity;
    Vector3 initialAcceleration;

    //A placeholder for the Y velocity when gravity is disabled
    float tempYVelocityHolder;

    // Start is called before the first frame update
    void Start()
    {
        //Automatically add object to list
        FindObjectOfType<CustomPhysicsSystem>().objectsList.Add(this);

        //Set initial values in case simulation is reset
        initialPosition = transform.position;
        initialVelocity = velocity;
        initialAcceleration = acceleration;

        //No use right now, perhaps later
        if (autoDetermineMass)
        {
            switch (collisionType.GetType())
            {
                case TypeEnum.SPHERE:
                    SphereCollisionType sphere = (SphereCollisionType)collisionType;

                    mass = (float)((4 / 3) * 3.14 * Mathf.Pow(sphere.radius, 3));
                    break;
            }
        }
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

    //Is Grounded Flag
    public bool IsGrounded()
    {
        return isGrounded;
    }

    //Set Is Grounded Flag
    public void SetIsGrounded(bool grounded)
    {
        isGrounded = grounded;
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
