using System;
using System.Collections;
using System.Collections.Generic;

namespace GMTools
{
    public static class ExtendSystemRandom
    {
        /// <summary>
        /// Next radomize float inclusive min and max values
        /// </summary>
        /// <param name="rnd"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns>float</returns>
        public static float NextFloat(this System.Random rnd, float min, float max)
        {
            return (float)(min + rnd.NextDouble() * (max - min));
        }
    }
}

