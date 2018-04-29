using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace CoreDefinitions.Helpers
{
    public static class CryptographyMath
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

        public static long PowerSearch(long z, long pow, long mod)
        {
            long x = 1;
            for (long j = 0; j < pow; j++)
            {
                x = (x * z) % mod;
            }
            return x;
        }

        public static List<long> Factorization(long p)
        {
            var d = 2;
            var primeFactors = new List<long>();
            while (d * d <= p)
            {
                while ((p % d) == 0)
                {
                    primeFactors.Add(d);
                    p = (long)Math.Floor((double)p / (double)d);
                }
                d += 1;
            }
            if (p > 1)
            {
                primeFactors.Add(p);
            }
            return primeFactors;
        }

        public static long ModuloInversion(long b, long n)
        {
            var gcd = ExtendedGCD(b, n);
            var g = gcd[0];
            var x = gcd[1];
            if (g == 1)
            {
                return x % n;
            }
            throw new Exception("Так не пойдёт");
        }

        public static long FixModulo(long value, long module)
        {
            return value >= 0 ? value : value + module;
        }

        public static long ChineseRemainderAlgorithm(List<KeyValuePair<long, long>> pairs)
        {
            var N = pairs[0].Value;
            var X = 0L;
            foreach (var ni in pairs.Skip(1))
            {
                N *= ni.Value;
            }
            foreach (var tuple in pairs)
            {
                var mi = N / tuple.Value;
                var gcd = ExtendedGCD(mi, tuple.Value);
                X += mi * tuple.Key * gcd[1];
            }
            var result = X % N;
            return FixModulo(result, N);
        }

        public static long ShankssSmallStepBigStepAlgorithm(long alpha, long beta, long n)
        {
            var m = Convert.ToInt32(Math.Ceiling(Math.Sqrt(n - 1)));
            var a = BigInteger.ModPow(alpha, m, n);
            var b = ExtendedGCD(alpha, n)[1];

            var L1 = new List<KeyValuePair<long, long>>();
            var L2 = new List<KeyValuePair<long, long>>();

            for (long k = 0; k < m; k++)
            {
                var valueL1 = FixModulo((long)(BigInteger.ModPow(a, k, n)), n);
                var valueL2 = FixModulo((long)(beta * BigInteger.Pow(b, (int)k) % n), n);
                L1.Add(new KeyValuePair<long, long>(k, valueL1));
                L2.Add(new KeyValuePair<long, long>(k, valueL2));
            }

            var sortedL1 = L1.OrderBy(o => o.Value).ToList();
            var sortedL2 = L2.OrderBy(o => o.Value).ToList();

            int i = 0, j = 0;
            while (i < m && j < m)
            {
                if (sortedL1[j].Value == sortedL2[i].Value)
                {
                    return m * sortedL1[j].Key + sortedL2[i].Key % n;
                }
                else if (Math.Abs(sortedL1[j].Value) > Math.Abs(sortedL2[i].Value))
                {
                    i = i + 1;
                }
                else
                {
                    j = j + 1;
                }
            }

            throw new Exception("Этого не может быть...");
        }

        public static KeyValuePair<long, long> CongruencePair(long g, long h, long p, long q, long e, long e1, long e2)
        {
            var alphaInverse = ModuloInversion(e1, p);
            var x = 0L;
            foreach (var i in Enumerable.Range(1, (int)(e)))
            {
                var a = FixModulo((long)(BigInteger.ModPow(e1, BigInteger.Pow(q, (int)(e - 1)), p)), p);
                var b = FixModulo((long)(BigInteger.ModPow((e2 * BigInteger.Pow(alphaInverse, (int)x)), (BigInteger.Pow(q, (int)(e - i))), p)), p);
                x += ShankssSmallStepBigStepAlgorithm(a, b, p) * (long)(BigInteger.Pow(q, i - 1));
            }
            return new KeyValuePair<long, long>(x, (long)(BigInteger.Pow(q, (int)e)));
        }
    }
}