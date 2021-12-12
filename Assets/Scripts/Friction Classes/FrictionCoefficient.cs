using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Enum for Material. Makes selection easier to read
public enum Material
{
    PLASTIC = 0,
    SMOOTH_LEATHER = 1,
    ROUGH_LEATHER = 2,
    POLYESTER = 3,
    WOOD = 4,
    METAL = 5,
    FRICTIONLESS_SURFACE = 6,
}

public static class FrictionCoefficient
{
    //Provides the Num of Materials and the base coefficient of each material
    const int NUM_MATERIALS = 7;
    const float STATIC_FRICTION_MULTIPLIER = 1.1f;
    static float[] materialCoefficient = {0.05f, 0.45f, 0.85f, 0.15f, 0.7f, 0.25f, 0.0f};

    // Returns the Kinetic coefficent
    public static float GetMaterialCoefficient(Material mat)
    {
        return materialCoefficient[(int)mat]; 
    }

    // Returns the kinetic coefficent between 2 objects based on their materials
    public static float GetKineticCoefficient(Material matA, Material matB)
    {
        float coefficientA = materialCoefficient[(int)matA];
        float coefficientB = materialCoefficient[(int)matB];

        return (coefficientA + coefficientB) * 0.5f;
    }

    // Returns the static coefficent between 2 objects based on their materials
    // For easy conversion, the static coefficent is the kinetic coefficent modified by a multiplier
    public static float GetStaticCoefficient(Material matA, Material matB)
    {
        return GetKineticCoefficient(matA, matB) * STATIC_FRICTION_MULTIPLIER;
    }
};
