using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    const int NUM_MATERIALS = 7;
    static float[] materialCoefficient = {0.05f, 0.45f, 0.85f, 0.15f, 0.7f, 0.25f, 0.0f};

    public static float GetMaterialCoefficient(Material mat)
    {
        return materialCoefficient[(int)mat]; 
    }

    public static float GetKineticCoefficient(Material matA, Material matB)
    {
        float coefficientA = materialCoefficient[(int)matA];
        float coefficientB = materialCoefficient[(int)matB];

        return (coefficientA + coefficientB) * 0.5f;
    }

    public static float GetStaticCoefficient(Material matA, Material matB)
    {
        return GetKineticCoefficient(matA, matB) * 1.1f;
    }
};
