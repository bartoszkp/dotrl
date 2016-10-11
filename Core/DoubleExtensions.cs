using System;

namespace Core
{
    public static class DoubleExtensions
    {
        public static double Squared(this double @this)
        {
            return @this * @this;
        }

        public static double NonZeroSignum(this double @this)
        {
            if (@this == 0)
            {
                return 1;
            }

            return Math.Sign(@this);
        }
    }
}
