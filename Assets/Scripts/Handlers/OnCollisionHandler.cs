using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCollisionHandler : MonoBehaviour
{
    //Handles the Sphere-Sphere Collision
    public static void OnSphere_SphereCollision(CustomPhysicsObject sphere1, CustomPhysicsObject sphere2)
    {
        //Cast spheres.
        SphereCollisionType sphereCollision1 = (SphereCollisionType)sphere1.collisionType;
        SphereCollisionType sphereCollision2 = (SphereCollisionType)sphere2.collisionType;

        //Find the penetration Depth
        Vector3 displacementBetweenSpheres = sphere2.transform.position - sphere1.transform.position;
        float distanceBetween = displacementBetweenSpheres.magnitude;
        float sumRad = sphereCollision1.radius + sphereCollision2.radius;
        float penetration = sumRad - distanceBetween;

        // Normal for the collision.
        Vector3 collisionNormal = displacementBetweenSpheres / distanceBetween;
        
        // Move spheres to stop overlapping.
        Vector3 minimumTranslationVector = collisionNormal * penetration;
        Vector3 translationA = minimumTranslationVector * -0.5f;
        Vector3 translationB = minimumTranslationVector * 0.5f;

        sphere1.transform.position += translationA;
        sphere2.transform.position += translationB;
    }

    //Handles the Sphere-Plane Collision
    public static void OnSphere_PlaneCollision(CustomPhysicsObject sphere, CustomPhysicsObject plane)
    {
        //Cast the collision types
        PlaneCollisionType planeCollision = (PlaneCollisionType)plane.collisionType;
        SphereCollisionType sphereCollision = (SphereCollisionType)sphere.collisionType;

        Vector3 planeNormal = planeCollision.GetPlaneNormal();

        // Stop the velocity on the appropriate axis depending on the plane's normal
        //Is it a YZ plane?
        if (planeNormal.x != 0)
            sphere.velocity.x = 0.0f;
        //Is it an XY plane?
        else if (planeNormal.z != 0)
        {
            sphere.velocity.z = 0.0f;
            sphere.SetIsGrounded(true);
        }
        //Is it an XZ plane?
        else
            sphere.velocity.y = 0.0f;

        // Perform the displacement of the sphere so it does not penetrate the plane
        Vector3 displacement = sphere.transform.position - plane.transform.position;
        float dotProduct = Vector3.Dot(planeNormal, displacement);
        float distance = Mathf.Abs(dotProduct);
        float penetration = sphereCollision.radius - distance;

        sphere.transform.position += planeNormal * penetration;
    }

    // Helper class for Sphere - AABB collision to reduce lines of code
    static bool OnSphere_AABBCollisionHelper(CustomPhysicsObject sphere, Vector3 facePos, Vector3 faceNormal)
    {
        SphereCollisionType sphereComponent = (SphereCollisionType)sphere.collisionType;

        // Same code as Sphere-Plane collision checking, only this time we have a response
        Vector3 distanceVec = sphere.transform.position - facePos;
        float distance = Mathf.Abs(Vector3.Dot(distanceVec, faceNormal));
        float penetration = sphereComponent.radius - distance;

        //If there's a penetration depth, we move the sphere away from that face and stop its velocity
        if (penetration > 0)
        {
            sphere.transform.position += penetration * faceNormal;
            sphere.velocity += new Vector3(Mathf.Abs(sphere.velocity.x) * faceNormal.x, Mathf.Abs(sphere.velocity.y) * faceNormal.y, Mathf.Abs(sphere.velocity.z) * faceNormal.z);
            return true;
        }
        return false;
    }

    // Handles the Sphere - AABB Collision
    public static void OnSphere_AABBCollision(CustomPhysicsObject sphere, CustomPhysicsObject aabb)
    {
        AABBCollisionType aabbComponent = (AABBCollisionType)aabb.collisionType;

        Vector3 mid = aabb.transform.position;

        //VERY DEMANDING IN PROCESSING
        //Treat each of the faces of the AABB box as a plane, and perfect sphere plane collision multiple times
        //Each face requires its position and normal to be passed
        if (OnSphere_AABBCollisionHelper(sphere, new Vector3(mid.x, mid.y + (aabbComponent.height / 2), mid.z), aabbComponent.GetUpNormal()))
            return;
        if (OnSphere_AABBCollisionHelper(sphere, new Vector3(mid.x + (aabbComponent.width / 2), mid.y, mid.z), aabbComponent.GetRightNormal()))
            return;
        if (OnSphere_AABBCollisionHelper(sphere, new Vector3(mid.x, mid.y, mid.z + (aabbComponent.length / 2)), aabbComponent.GetFrontNormal()))
            return;
        if (OnSphere_AABBCollisionHelper(sphere, new Vector3(mid.x, mid.y - (aabbComponent.height / 2), mid.z), aabbComponent.GetDownNormal()))
            return;
        if (OnSphere_AABBCollisionHelper(sphere, new Vector3(mid.x - (aabbComponent.width / 2), mid.y, mid.z), aabbComponent.GetLeftNormal()))
            return;
        if (OnSphere_AABBCollisionHelper(sphere, new Vector3(mid.x, mid.y, mid.z - (aabbComponent.length / 2)), aabbComponent.GetBackNormal()))
            return;
    }


    // Handles the Plane - AABB Collision
    public static void OnPlane_AABBCollision(CustomPhysicsObject plane, CustomPhysicsObject aabb)
    {
        PlaneCollisionType planeComponent = (PlaneCollisionType)plane.collisionType;
        AABBCollisionType aabbComponent = (AABBCollisionType)aabb.collisionType;

        Vector3 halfSize = aabbComponent.GetMaxPoint() - aabb.transform.position;

        float projection = (halfSize.x * Mathf.Abs(planeComponent.GetPlaneNormal().x)) +
                           (halfSize.y * Mathf.Abs(planeComponent.GetPlaneNormal().y)) +
                           (halfSize.z * Mathf.Abs(planeComponent.GetPlaneNormal().z));

        float dot = Vector3.Dot(planeComponent.GetPlaneNormal(), aabb.transform.position) + planeComponent.GetConstant();

        float penetration = dot - Mathf.Abs(projection);

        //Update the AABB's position and velocity
        aabb.transform.position -= penetration * planeComponent.GetPlaneNormal();
        aabb.velocity += new Vector3(Mathf.Abs(aabb.velocity.x) * planeComponent.GetPlaneNormal().x, 
                                     Mathf.Abs(aabb.velocity.y) * planeComponent.GetPlaneNormal().y, 
                                     Mathf.Abs(aabb.velocity.z) * planeComponent.GetPlaneNormal().z);
    }

    // Handles the AABB - AABB Collision
    public static void OnAABB_AABBCollision(CustomPhysicsObject aabb1, CustomPhysicsObject aabb2)
    {
        Vector3 displacement = aabb2.transform.position - aabb1.transform.position;
        Vector3 halfSizeA = aabb1.transform.localScale * 0.5f;
        Vector3 halfSizeB = aabb2.transform.localScale * 0.5f;

        float penetrationX = (halfSizeA.x + halfSizeB.x - Mathf.Abs(displacement.x));
        float penetrationY = (halfSizeA.y + halfSizeB.y - Mathf.Abs(displacement.y));
        float penetrationZ = (halfSizeA.z + halfSizeB.z - Mathf.Abs(displacement.z));

        Vector3 minimumTranslation;
        Vector3 collisionNormal;
        Vector3 contactPoint;

        // is X the smallest penetration value
        if (penetrationX < penetrationY && penetrationX < penetrationZ)
        {
            collisionNormal = new Vector3(Mathf.Sign(displacement.x), 0.0f, 0.0f);
            minimumTranslation = collisionNormal * penetrationX;
            aabb1.velocity.x = 0.0f;
        }
        //is Y the smallest peneration value
        else if (penetrationY < penetrationX && penetrationY < penetrationZ)
        {
            collisionNormal = new Vector3(0.0f, Mathf.Sign(displacement.y), 0.0f);
            minimumTranslation = collisionNormal * penetrationY;
            aabb1.velocity.y = 0.0f;
        }
        //otherwise, Z is
        else
        {
            collisionNormal = new Vector3(0.0f, 0.0f, Mathf.Sign(displacement.z));
            minimumTranslation = collisionNormal * penetrationZ;
            aabb2.velocity.z = 0.0f;
        }
        contactPoint = minimumTranslation + aabb1.transform.position;

        Vector3 translationA = minimumTranslation * -0.5f;
        Vector3 translationB = minimumTranslation * 0.5f;

        aabb1.transform.position += translationA;
        aabb2.transform.position += translationB;
    }

    //Calls The OnSphere_PlaneCollsion function. Exists to avoid naming problems
    public static void OnPlane_SphereCollision(CustomPhysicsObject plane, CustomPhysicsObject sphere) { OnSphere_PlaneCollision(sphere, plane); }

    public static void OnAABB_SphereCollision(CustomPhysicsObject aabb, CustomPhysicsObject sphere) { OnSphere_AABBCollision(sphere, aabb); }

    public static void OnAABB_PlaneCollision(CustomPhysicsObject aabb, CustomPhysicsObject plane) { OnPlane_AABBCollision(plane, aabb); }
}
