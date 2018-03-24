using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreDefinitions.Helpers
{
    class CryptographyMath
    {
        public static long Euler(long num)
        {
            long result = num;
            for (int i = 2; i * i <= num; ++i)
            {
                if (num % i == 0)
                {
                    while (num % i == 0)
                    {
                        num /= i;
                    }
                    result -= result / i;
                }
            }
            if (num > 1)
            {
                result -= result / num;
            }
            return result;
        }

        public static long[] ExtendedGCD(long a, long b)
        {
            long[] result = new long[3];
            if (b == 0)
            {
                result[0] = a;
                result[1] = 1;
                result[2] = 0;
                return result;
            }
            var sub_res = ExtendedGCD(b, a % b);
            result[0] = sub_res[0];
            result[1] = sub_res[2];
            result[2] = sub_res[1] - (a / b) * sub_res[2];
            return result;
        }

        public static long FastPower(long value, long power, long n)
        {
            ulong val = (ulong)value;
            ulong result = 1;
            while (power != 0)
            {
                if ((power & 1) == 1)
                {
                    result = ((result * val) % (ulong)n);
                }
                val = (val * val) % (ulong)n;
                power = power >> 1;
            }
            return (long)result;
        }

    }
}
