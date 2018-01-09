using System;

namespace PrimeNumbers
{
    static class Util
    {
        public static bool IsPrime(long n)
        {
            for (var i = 2; i * i <= n; i++)
            {
                if (n % i == 0)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
