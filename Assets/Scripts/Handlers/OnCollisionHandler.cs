using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCollisionHandler : MonoBehaviour
{
    // Used to apply momentum between 2 objects
    public static void ApplyKinematicRespose(CustomPhysicsObject Object1, CustomPhysicsObject Object2, Vector3 collisionNormal, bool frictionBroken)
    {
        // Get the velocity relative to object 2
        Vector3 relativeVelocity = Object2.velocity - Object1.velocity;
        float relativeNormalVecocity = Vector3.Dot(relativeVelocity, collisionNormal);

        if (relativeNormalVecocity >= 0.0f)
            return; // No bounce

        // determine the impulse of the objects
        float restitution = 0.5f * (Object1.bounciness + Object2.bounciness);
        float changeInVelocity = -relativeNormalVecocity * (1.0f + restitution);
        float impulse = changeInVelocity;

        // adjust the velocities of the objects depending on whether the the object in question can actually move
        // Is Object 2 motionless OR friction is too powerful to move it?
        if (!Object1.motionless && (Object2.motionless || !frictionBroken))
        {
            Object1.velocity -= collisionNormal * impulse;
        }
        // Is object 1 motionless
        else if (Object1.motionless && !Object2.motionless)
        {
            Object2.velocity += collisionNormal * impulse;
        }
        // both objects are moveable. Adjust the impulse based on their masses and apply individual momentum to each
        else
        {
            impulse = (changeInVelocity / (1.0f / Object1.mass + 1.0f / Object2.mass));
            Object1.velocity -= collisionNormal * (impulse / Object1.mass);
            Object2.velocity += collisionNormal * (impulse / Object2.mass);
        }

        Vector3 relativeSurfaceVelocity = relativeVelocity - (relativeNormalVecocity * collisionNormal);

        // Only apply friction if the object was not held down by static friction (either the force was greater or it wasn't grounded)
        if (frictionBroken)
            ApplyFriction(Object1, Object2, relativeSurfaceVelocity, collisionNormal);
    }

    // Used to apply friction between 2 objects
    public static void ApplyFriction(CustomPhysicsObject Obj1, CustomPhysicsObject Obj2, Vector3 relativeSurfaceVelocity1to2, Vector3 normal1to2)
    {
        float gravity = FindObjectOfType<CustomPhysicsSystem>().gravity;
        Vector3 grav = new Vector3(0.0f, gravity, 0.0f);
        float minFrictionSpeed = 0.025f;
        float relativeSpeed = relativeSurfaceVelocity1to2.magnitude;

        // Velocity is so slow that friction should stop it completely (hence velocity at 0)
        if (Mathf.Abs(relativeSpeed) <= minFrictionSpeed)
        {
            Obj1.velocity.x = 0.0f;
            Obj1.velocity.z = 0.0f;
            return;
        }

        // determine the frictional acceleration being applied based on the collision normal between the 2 objects
        Vector3 directionToApplyFriction = relativeSurfaceVelocity1to2 / relativeSpeed;
        float gravityAccelerationAlongNormal = Vector3.Dot(grav, normal1to2);

        // Only apply the frictional acceleration if object 1 is not motionless AND has a ground object
        if (Obj1.GetGroundObject() != null && !Obj1.motionless)
        {
            float kFrictionCoefficientA = FrictionCoefficient.GetKineticCoefficient(Obj1.material, Obj1.GetGroundObject().material);
            Vector3 frictionAccelerationA = directionToApplyFriction * -gravityAccelerationAlongNormal * kFrictionCoefficientA;
            Obj1.velocity -= frictionAccelerationA * Time.fixedDeltaTime;
        }

        // Only apply the frictional acceleration if object 2 is not motionless AND has a ground object
        if (Obj2.GetGroundObject() != null && !Obj2.motionless)
        {
            float kFrictionCoefficientB = FrictionCoefficient.GetKineticCoefficient(Obj2.material, Obj2.GetGroundObject().material);
            Vector3 frictionAccelerationB = -directionToApplyFriction * -gravityAccelerationAlongNormal * kFrictionCoefficientB;
            Obj2.velocity -= frictionAccelerationB * Time.fixedDeltaTime;
        }
    }

    // A check used to see if an object at rest has its friction broken open collision. 
    public static bool StaticFrictionBroken(CustomPhysicsObject ObjectA, CustomPhysicsObject ObjectB, Vector3 collisionNormal)
    {
        // There is no static friction if the object has no ground or if the object is already moving
        if (ObjectB.GetGroundObject() != null && ObjectB.velocity.x == 0.0f && ObjectB.velocity.z == 0)
        {

            // Motionless object should not move, regardless of friction
            if (ObjectB.motionless)
                return false;

            //Determine Impulse Force applied between the 2 objects
            Vector3 relativeVelocity = ObjectB.velocity - ObjectA.velocity;
            float relativeNormalVecocity = Vector3.Dot(relativeVelocity, collisionNormal);

            float restitution = 0.5f * (ObjectA.bounciness + ObjectB.bounciness);
            float impulse = -relativeNormalVecocity * (1.0f + restitution) * ObjectA.mass;

            //Determine static friction between the object at rest and its ground
            float gravity = FindObjectOfType<CustomPhysicsSystem>().gravity;
            float staticCoefficient = FrictionCoefficient.GetStaticCoefficient(ObjectB.material, ObjectB.GetGroundObject().material);

            float staticFrictionForce = Mathf.Abs(staticCoefficient * ObjectB.mass * gravity);
            float collisionForce = impulse / Time.deltaTime;

            // Only if the impulse force is greater than the force of friction does the restless object have movement
            return (collisionForce >= staticFrictionForce);
        }
        else
            //default cause if static friction does not apply
            return true;
    }

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

        // Applies momentum to the spheres. no need to check for static friction due to the non-flat nature of spheres
        ApplyKinematicRespose(sphere1, sphere2, collisionNormal, true);
    }

    //Handles the Sphere-Plane Collision
    public static void OnSphere_PlaneCollision(CustomPhysicsObject sphere, CustomPhysicsObject plane)
    {
        //Cast the collision types
        PlaneCollisionType planeCollision = (PlaneCollisionType)plane.collisionType;
        SphereCollisionType sphereCollision = (SphereCollisionType)sphere.collisionType;

        Vector3 planeNormal = planeCollision.GetPlaneNormal();

        // Perform the displacement of the sphere so it does not penetrate the plane
        Vector3 displacement = sphere.transform.position - plane.transform.position;
        float dotProduct = Vector3.Dot(planeNormal, displacement);
        float distance = Mathf.Abs(dotProduct);
        float penetration = sphereCollision.radius - distance;

        sphere.transform.position += planeNormal * penetration;

        // If the plane is collided with is a ground plane, set the plane as the sphere's current ground object
        if (planeNormal.y == 1.0f)
            sphere.SetGroundObject(plane);

        //Apply momentum (The plane does not move, so we need to pass the plane normal relative to the sphere since that is the collision normal)
        ApplyKinematicRespose(sphere, plane, -planeNormal, true);
    }

    // Handles the Sphere - AABB Collision
    public static void OnSphere_AABBCollision(CustomPhysicsObject sphere, CustomPhysicsObject aabb)
    {
        SphereCollisionType sphereComponent = (SphereCollisionType)sphere.collisionType;
        AABBCollisionType aabbComponent = (AABBCollisionType)aabb.collisionType;

        // get the 'extends' (how far out it reaches from the center) of the bounding box
        Vector3 halfSizeB = aabbComponent.GetSize() * 0.5f;

        // determine the penetration between each of the axis
        Vector3 displacementAB = aabb.transform.position - sphere.transform.position;
        float distance = displacementAB.magnitude;

        float penetrationX = (sphereComponent.radius + halfSizeB.x - Mathf.Abs(displacementAB.x));
        float penetrationY = (sphereComponent.radius + halfSizeB.y - Mathf.Abs(displacementAB.y));
        float penetrationZ = (sphereComponent.radius + halfSizeB.z - Mathf.Abs(displacementAB.z));

        if (penetrationX < 0 || penetrationY < 0 || penetrationZ < 0)
        {
            return;
        }

        Vector3 collisionNormal = Vector3.zero;
        Vector3 miniumTranslationVector = Vector3.zero;

        // Find the lowest penetration depth between the sphere and the bounding box, and prepare the normals to move in that direction
        if (penetrationX <= penetrationY && penetrationX <= penetrationZ)
        {
            collisionNormal = new Vector3(Mathf.Sign(displacementAB.x), 0.0f, 0.0f);
            miniumTranslationVector = collisionNormal * -penetrationX;
        }
        else if (penetrationY <= penetrationX && penetrationY <= penetrationZ)
        {
            collisionNormal = new Vector3(0.0f, Mathf.Sign(displacementAB.y), 0.0f);
            miniumTranslationVector = collisionNormal * -penetrationY;
        }
        else if (penetrationZ <= penetrationY && penetrationZ <= penetrationX)
        {
            collisionNormal = new Vector3(0.0f, 0.0f, Mathf.Sign(displacementAB.z));
            miniumTranslationVector = collisionNormal * -penetrationZ;
        }

        //Determine if friction was broken when the two objects collided
        bool frictionBroken = StaticFrictionBroken(sphere, aabb, collisionNormal);

        //Is the bounding box motionless OR failed to have its static friction broken?
        if (!sphere.motionless && (aabb.motionless || !frictionBroken))
            sphere.transform.position += miniumTranslationVector * 2;
        //Is the sphere motionless?
        else if (sphere.motionless)
            aabb.transform.position -= miniumTranslationVector * 2;
        //Both objects can move, adjust their translation vectors
        else
        {
            sphere.transform.position += miniumTranslationVector;
            aabb.transform.position -= miniumTranslationVector;
        }

        // if the collision normal was a flat ground (or face), set the aabb as the sphere's ground object
        if (collisionNormal.y == -1.0f)
            sphere.SetGroundObject(aabb);

        //// Apply Kinematic Respose
        ApplyKinematicRespose(sphere, aabb, collisionNormal, frictionBroken);
    }


    // Handles the Plane - AABB Collision
    public static void OnPlane_AABBCollision(CustomPhysicsObject plane, CustomPhysicsObject aabb)
    {
        PlaneCollisionType planeComponent = (PlaneCollisionType)plane.collisionType;
        AABBCollisionType aabbComponent = (AABBCollisionType)aabb.collisionType;

        // get the 'extends' (how far out it reaches from the center) of the bounding box
        Vector3 halfSize = aabbComponent.GetMaxPoint() - aabb.transform.position;

        // Get the projection distance between the bounding box and the plain
        float projection = (halfSize.x * Mathf.Abs(planeComponent.GetPlaneNormal().x)) +
                           (halfSize.y * Mathf.Abs(planeComponent.GetPlaneNormal().y)) +
                           (halfSize.z * Mathf.Abs(planeComponent.GetPlaneNormal().z));

        float dot = Vector3.Dot(planeComponent.GetPlaneNormal(), aabb.transform.position) + planeComponent.GetConstant();

        float penetration = dot - Mathf.Abs(projection);

        //Update the AABB's position and velocity
        aabb.transform.position -= penetration * planeComponent.GetPlaneNormal();

        // If the plane it collided with was a ground plane, set that plane as the AABB's ground object
        if (planeComponent.GetPlaneNormal().y == 1.0f)
            aabb.SetGroundObject(plane);

        // Applies Momentum (plane is not moving, so the collision normal is relative to the AABB box)
        ApplyKinematicRespose(aabb, plane, -planeComponent.GetPlaneNormal(), true);
    }

    // Handles the AABB - AABB Collision
    public static void OnAABB_AABBCollision(CustomPhysicsObject aabb1, CustomPhysicsObject aabb2)
    {
        // Get the depth of penetration between the 2 Bounding boxes between all axis
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
        }
        //is Y the smallest peneration value
        else if (penetrationY < penetrationX && penetrationY < penetrationZ)
        {
            collisionNormal = new Vector3(0.0f, Mathf.Sign(displacement.y), 0.0f);
            minimumTranslation = collisionNormal * penetrationY;
        }
        //otherwise, Z is
        else
        {
            collisionNormal = new Vector3(0.0f, 0.0f, Mathf.Sign(displacement.z));
            minimumTranslation = collisionNormal * penetrationZ;
        }
        contactPoint = minimumTranslation + aabb1.transform.position;

        Vector3 translationA = minimumTranslation * -0.5f;
        Vector3 translationB = minimumTranslation * 0.5f;

        // check for static friction on the 2 bounding boxes
        bool frictionBroken = StaticFrictionBroken(aabb1, aabb2, collisionNormal);

        // Is the first bounding box not motionless?
        if (!aabb1.motionless)
            aabb1.transform.position += translationA;
        //Is the second bounding box not motionless AND had static friction broken (if any)
        if (!aabb2.motionless && frictionBroken)
            aabb2.transform.position += translationB;

        // if the collision normal was a flat ground, set the second AABB as the first AABB's ground object
        if (collisionNormal.y == -1.0f)
            aabb1.SetGroundObject(aabb2);

        // Apply momentum + friction
        ApplyKinematicRespose(aabb1, aabb2, collisionNormal, frictionBroken);
    }

    // These functions exist to avoid any naming discrepencies
    public static void OnPlane_SphereCollision(CustomPhysicsObject plane, CustomPhysicsObject sphere) { OnSphere_PlaneCollision(sphere, plane); }

    public static void OnAABB_SphereCollision(CustomPhysicsObject aabb, CustomPhysicsObject sphere) { OnSphere_AABBCollision(sphere, aabb); }

    public static void OnAABB_PlaneCollision(CustomPhysicsObject aabb, CustomPhysicsObject plane) { OnPlane_AABBCollision(plane, aabb); }
}
