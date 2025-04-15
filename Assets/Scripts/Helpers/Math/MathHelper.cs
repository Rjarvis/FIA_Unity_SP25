using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Random;

namespace Helpers.Math
{
    public static class MathHelper
    {
        public static int[] GenerateXNumbersForColorRGB(int x)
        {
            List<int> toReturn = new List<int>();
            for (int i = 0; i < x; i++)
            {
                toReturn.Add(Random.Range(0, 256));//Max RGB value, min 0
            }

            return toReturn.ToArray();
        }
    }
}