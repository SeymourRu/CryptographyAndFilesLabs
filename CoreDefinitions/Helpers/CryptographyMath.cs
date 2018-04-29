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

        #region Pohling-Hellman

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

        #endregion Pohling-Hellman

        #region Dixon
        private static long FastExponentiation(long a, int n)
        {
            long ans = 1;
            while (n > 0)
            {
                if ((n & 1) > 0)
                {
                    ans = ans * a;
                }
                a = a * a;
                n = n >> 1;
            }
            return ans;
        }

        private static int deciToBinary(int num)
        {
            int bin;
            if (num != 0)
            {
                bin = (num % 2) + 10 * deciToBinary(num / 2);
                return bin;
            }
            else
            {
                return 0;
            }
        }

        public static bool IsPower(int n)
        {
            if ((-n & n) == n)
            {
                return true;//2 ^ k
            }
            var lgn = 1 + (deciToBinary(Math.Abs(n)).ToString().Length - 2);
            for (int b = 2; b < lgn; b++)
            {
                var lowa = 1L;
                var higha = 1L << (lgn / b + 1);
                while (lowa < (higha - 1))
                {
                    var mida = (lowa + higha) >> 1;
                    var ab = FastExponentiation(mida, b);
                    if (ab > n)
                    {
                        higha = mida;
                    }
                    else if (ab < n)
                    {
                        lowa = mida;
                    }
                    else
                    {
                        return true; //mida ^ b
                    }
                }
            }
            return false;
        }

        public static IEnumerable<int> SieveOfEratosthenes(int num)
        {
            return Enumerable.Range(1, Convert.ToInt32(Math.Floor(Math.Sqrt(num))))
                .Aggregate(Enumerable.Range(1, num).ToList(),
                (result, index) =>
                {
                    result.RemoveAll(i => i > result[index] && i % result[index] == 0);
                    return result;
                }
                );
        }

        public static bool IsSmooth(long num, List<long> baseFactor, out List<long> factors)
        {
            try
            {
                var simpleFactors = new List<long>();
                int b, c;

                while ((num % 2) == 0)
                {
                    num = num / 2;
                    simpleFactors.Add(2);
                }
                b = 3; c = (int)Math.Sqrt(num) + 1;
                while (b < c)
                {
                    if ((num % b) == 0)
                    {
                        if (num / b * b - num == 0)
                        {
                            simpleFactors.Add(b);
                            num = num / b;
                            c = (int)Math.Sqrt(num) + 1;
                        }
                        else
                        {
                            b += 2;
                        }
                    }
                    else
                    {
                        b += 2;
                    }
                }

                simpleFactors.Add(num);
                factors = new List<long>();
                foreach (var bf in baseFactor)
                {
                    factors.Add(simpleFactors.Count(x => x == bf));
                }
                return true;
            }
            catch (Exception ex)
            {
                factors = new List<long>();
                System.Windows.Forms.MessageBox.Show(ex.Message);
                System.Windows.Forms.MessageBox.Show(ex.StackTrace);
                return false;
            }
        }

        public static int TheTruePowerfullGauss(List<List<double>> a, List<double> ans)
        {
            int n = (int)a.Count();
            int m = (int)a[0].Count() - 1;
            double eps = 0.0001;
            List<int> where = new List<int>(Enumerable.Repeat(-1, m));
            ans.AddRange(Enumerable.Repeat(-1.0, m));

            for (int col = 0, row = 0; col < m && row < n; ++col)
            {
                int sel = row;
                for (int i = row; i < n; ++i)
                    if (Math.Abs(a[i][col]) > Math.Abs(a[sel][col]))
                        sel = i;
                if (Math.Abs(a[sel][col]) < eps)
                    continue;
                for (int i = col; i <= m; ++i)
                {
                    var value = a[sel][i];
                    a[sel][i] = a[row][i];
                    a[row][i] = value;
                }
                where[col] = row;

                for (int i = 0; i < n; ++i)
                    if (i != row)
                    {
                        double c = a[i][col] / a[row][col];
                        for (int j = col; j <= m; ++j)
                            a[i][j] -= a[row][j] * c;
                    }
                ++row;
            }

            for (int i = 0; i < m; ++i)
                if (where[i] != -1)
                    ans[i] = a[where[i]][m] / a[where[i]][i];
            for (int i = 0; i < n; ++i)
            {
                double sum = 0;
                for (int j = 0; j < m; ++j)
                    sum += ans[j] * a[i][j];
                if (Math.Abs(sum - a[i][m]) > eps)
                    return 0;
            }

            for (int i = 0; i < m; ++i)
                if (where[i] == -1)
                    return int.MaxValue;
            return 1;
        }

        #endregion Dixon
    }
}