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

    }

    //Moves the objects in the scene (if they have a motion)
    void UpdateMovement()
    {
        foreach (CustomPhysicsObject physicsObject in objectsList)
        {
            //Skips to the next object if the current one is flagged as motionless
            if (physicsObject.motionless)
                continue;

            Vector3 accel = physicsObject.acceleration;

            //If the object is using the system's gravity, replace its y acceleration with the systems gravity
            if (physicsObject.UseSystemGravity())
                physicsObject.velocity += new Vector3(accel.x, gravity, accel.y) * Time.deltaTime;
            else
                physicsObject.velocity += accel * Time.deltaTime;

            physicsObject.transform.position += physicsObject.velocity * Time.deltaTime;
        }
    }

    //This function only checks the type of the objects to perform the necessary Check and Handling, which are done by the handler functions
    void UpdateCollision()
    {
        //Multiple loops required since we need to check the collision of different objects in the list with each other
        //for (int indexA = 0; indexA < objectsList.Count - 1; indexA++)
        for (int indexA = 0; indexA < objectsList.Count; indexA++)
        {
            //for (int indexB = indexA + 1; indexB < objectsList.Count; indexB++)
            for (int indexB = 0; indexB < objectsList.Count; indexB++)
            {
                CustomPhysicsObject objectA = objectsList[indexA];
                CustomPhysicsObject objectB = objectsList[indexB];

                //No need to check collision with itself
                if (objectA == objectB)
                    continue;

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
                                else if (objectA.GetGroundObject() == objectB)
                                   objectA.SetGroundObject(null);
                                break;

                            //Handle Sphere-AABB Collision
                            case TypeEnum.AABB:
                                SphereCollisionType sphere3 = (SphereCollisionType)objectA.collisionType;
                                AABBCollisionType aabb = (AABBCollisionType)objectB.collisionType;

                                if (CollisionCheckHandler.Sphere_AABBCollision(sphere3, aabb))
                                    OnCollisionHandler.OnSphere_AABBCollision(objectA, objectB);
                                else if (objectA.GetGroundObject() == objectB)
                                    objectA.SetGroundObject(null);
                                break;
                        }
                        break;
                    case TypeEnum.PLANE:
                        switch (objectB.collisionType.GetType())
                        {
                            //Handle Plane-Sphere Collision
                            case TypeEnum.SPHERE:
                                PlaneCollisionType plane = (PlaneCollisionType)objectA.collisionType;
                                SphereCollisionType sphere = (SphereCollisionType)objectB.collisionType;

                                if (CollisionCheckHandler.Plane_SphereCollision(plane, sphere))
                                    OnCollisionHandler.OnPlane_SphereCollision(objectA, objectB);
                                break;

                            //Handle Plane-AABB Collision
                            case TypeEnum.AABB:
                                PlaneCollisionType plane1 = (PlaneCollisionType)objectA.collisionType;
                                AABBCollisionType aabb = (AABBCollisionType)objectB.collisionType;

                                if (CollisionCheckHandler.Plane_AABBCollision(plane1, aabb))
                                    OnCollisionHandler.OnPlane_AABBCollision(objectA, objectB);
                                break;
                        }
                        break;
                    case TypeEnum.AABB:
                        switch(objectB.collisionType.GetType())
                        {
                            //Handle AABB-Sphere Collision
                            case TypeEnum.SPHERE:
                                AABBCollisionType aabb = (AABBCollisionType)objectA.collisionType;
                                SphereCollisionType sphere = (SphereCollisionType)objectB.collisionType;

                                if (CollisionCheckHandler.AABB_SphereCollision(aabb, sphere))
                                    OnCollisionHandler.OnAABB_SphereCollision(objectA, objectB);
                                break;

                            //Handle AABB-Plane Collision
                            case TypeEnum.PLANE:
                                AABBCollisionType aabb1 = (AABBCollisionType)objectA.collisionType;
                                PlaneCollisionType plane = (PlaneCollisionType)objectB.collisionType;

                                if (CollisionCheckHandler.AABB_PlaneCollision(aabb1, plane))
                                    OnCollisionHandler.OnAABB_PlaneCollision(objectA, objectB);
                                else if (objectA.GetGroundObject() == objectB)
                                    objectA.SetGroundObject(null);
                                break;

                            //Handle AABB-AABB Collision
                            case TypeEnum.AABB:
                                AABBCollisionType aabb2 = (AABBCollisionType)objectA.collisionType;
                                AABBCollisionType aabb3 = (AABBCollisionType)objectB.collisionType;

                                if (CollisionCheckHandler.AABB_AABBCollision(aabb2, aabb3))
                                    OnCollisionHandler.OnAABB_AABBCollision(objectA, objectB);
                                else if (objectA.GetGroundObject() == objectB)
                                    objectA.SetGroundObject(null);
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
