using UnityEngine;
using Random = System.Random;

public class RandomExtensions
{
    public static readonly int Seed = 69420;
    private Random _random;

    public RandomExtensions() => _random = new Random(Seed);

    public float GetFloat() => (float) _random.NextDouble();

    public Vector3 GetRandomVector(float length = 1)
    {
        return Quaternion.Euler(0, 0, GetFloat() * 360) * Vector2.up * length;
    }
    
    public Vector3 GetRandomVector(float minLength, float maxLength)
    {
        return GetRandomVector(minLength + GetFloat() * (maxLength - minLength));
    }

    private static int _randomSeed;

    public static int GetNextSeed() => _randomSeed++;
}
