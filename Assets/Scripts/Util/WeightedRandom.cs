using UnityEngine;

public static class WeightedRandom
{
    public static T Choose<T>(T[] items, float[] weights)
    {
        float total = 0;
        for (int i = 0; i < weights.Length; i++)
            total += weights[i];

        float random = Random.value * total;

        for (int i = 0; i < weights.Length; i++)
        {
            if (random < weights[i])
                return items[i];

            random -= weights[i];
        }

        return items[items.Length - 1];
    }
}