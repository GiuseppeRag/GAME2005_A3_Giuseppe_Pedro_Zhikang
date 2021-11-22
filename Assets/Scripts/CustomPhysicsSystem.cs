using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomPhysicsSystem : MonoBehaviour
{
    public List<CustomPhysicsObject> objectsList = new List<CustomPhysicsObject>();
    public float gravity = -9.8f;

    // Start is called before the first frame update
    void Start()
    {
        //sets all objects gravity to match that of the system, only if the object allows it
        foreach (CustomPhysicsObject physicsObject in objectsList)
        {
            if (physicsObject.UseSystemGravity())
                physicsObject.acceleration.y = gravity;
        }
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
        //Multiple loops required since we need to check the collision of different objects in the list with each other
        for (int indexA = 0; indexA < objectsList.Count - 1; indexA++)
        {
            for (int indexB = indexA + 1; indexB < objectsList.Count; indexB++)
            {
                CustomPhysicsObject objectA = objectsList[indexA];
                CustomPhysicsObject objectB = objectsList[indexB];

                switch (objectA.collisionType.GetType())
                {
                    case TypeEnum.SPHERE:
                        switch (objectB.collisionType.GetType())
                        {
                            //Handle Sphere-Sphere Collision
                            case TypeEnum.SPHERE:
                                SphereCollisionType sphere1 = (SphereCollisionType)objectA.collisionType;
                                SphereCollisionType sphere2 = (SphereCollisionType)objectB.collisionType;
                               
                                if (CollisionCheckHandler.Sphere_SphereCollision(sphere1, sphere2))
                                    OnCollisionHandler.OnSphere_SphereCollision(objectA, objectB);
                                break;

                            //Handle Sphere-Plane Collision
                            case TypeEnum.PLANE:
                                SphereCollisionType sphere = (SphereCollisionType)objectA.collisionType;
                                PlaneCollisionType plane = (PlaneCollisionType)objectB.collisionType;
                              
                                if (CollisionCheckHandler.Sphere_PlaneCollision(sphere, plane))
                                    OnCollisionHandler.OnSphere_PlaneCollision(objectA, objectB);
                                break;
                        }
                        break;
                    case TypeEnum.PLANE:
                        switch (objectsList[indexB].collisionType.GetType())
                        {
                            //Handle Plane-Sphere Collision
                            case TypeEnum.SPHERE:
                                PlaneCollisionType plane = (PlaneCollisionType)objectA.collisionType;
                                SphereCollisionType sphere = (SphereCollisionType)objectB.collisionType;

                                if (CollisionCheckHandler.Plane_SphereCollision(plane, sphere))
                                    OnCollisionHandler.OnPlane_SphereCollision(objectA, objectB);
                                break;
                        }
                        break;
                }
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdateMovement();
        UpdateCollision();
    }
}
