using System;
using System.Collections.Generic;
using System.Linq;

namespace MillerRabinAlgorithm
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<int, double> primeProbablities = new Dictionary<int, double>();

            // # of iterations that returned likely prime, divided by total # of iterations
            for(int n = 105001; n < 115000; n+=2)
            {
                int primeCount = 0;
                int totalCount = 0;

                for(int x = 0; x < 4; x++)
                {
                    if (MillerRabin(n))
                        primeCount++;
                    totalCount++;
                }

                double errorProb = (double)primeCount / (double)totalCount;

                if (!IsPrime(n))
                {
                    //Console.WriteLine(n);
                    primeProbablities.Add(n, errorProb);
                }
            }

            var orderedErrorProbs = primeProbablities.OrderByDescending(p => p.Value);

            // write top 10 ints and their error probabilities
            for(int i = 0; i < 10; i++)
            {
                Console.WriteLine(orderedErrorProbs.ElementAt(i));
            }
        }

        static bool MillerRabin(int n)
        {
            // these if statements take care of edge cases in prime
            // testing, i.e if n is 2, divisable by 2 or less than 1
            if (n % 2 == 0)
                return false;
            if (n == 2)
                return true;
            if (n <= 1)
                return false;

            // a value to represent -1 mod n
            int negativeOne = n - 1;

            int s = 0;
            int m = n - 1;

            while (m % 2 == 0)
            {
                s++;
                m = m / 2;
            }

            // perform checks to see if the number is prime
            //int x = (int)Math.Pow((double)a, (double)m) % n;

            //// if either of these tests pass, declare n a probable prime
            //if (x == 1 || x  == negativeOne)
            //    return true;

            //while(m != negativeOne)
            //{
            //    x = (x * x) % n;
            //    m *= 2;

            //    if (x == 1)
            //        return false;
            //    if (x == negativeOne)
            //        return true;
            //}
            Random r = new Random();
            int a = r.Next(n - 1) + 1;
            //for (int i = 0; i < k; i++)
            //{

                int temp = m;
                long mod = 1;
                for (int j = 0; j < temp; ++j) mod = (mod * a) % n;
                while (temp != n - 1 && mod != 1 && mod != n - 1)
                {
                    mod = (mod * mod) % n;
                    temp *= 2;
                }

                if (mod != n - 1 && temp % 2 == 0) return false;
            //}
            return true;

            //if ((int)Math.Pow((double)a, (double)(m * 2)) % n == 1)
            //    return false;

            //if ((int)Math.Pow((double)a, (double)(m * 2)) % n == negativeOne)
            //    return true;

            // if no tests pass, declare n composite
            //return false;
        }

        /// <summary>
        /// A method that will garuantee a number to be prime by
        /// brute-force checking it, this will be used to "fact-check"
        /// our Miller-Rabin implementation
        /// </summary>
        /// <param name="n">the number to be checked</param>
        /// <returns>whether or not the number is prime</returns>
        static bool IsPrime(int n)
        {
            // these if statements take care of edge cases in prime
            // testing, i.e if n is 2, divisable by 2 or less than 1
            if (n % 2 == 0)
                return false;
            if (n == 2)
                return true;
            if (n <= 1)
                return false;

            // our boundary tells us the numbers we need to check up to
            // as we only need to check up to the square root of n this
            // serves as our boundary
            var boundary = (int)Math.Floor(Math.Sqrt(n));

            // we then check each number from 3 up until our boundary,
            // we also only check odd numbers as we've already determined
            // that our n value is not divisible by 2
            for (int i = 3; i <= boundary; i += 2)
            {
                // if n is divisible by any of these numbers it is not
                // prime
                if (n % i == 0)
                    return false;
            }
            // if we complete the loop without returning false, then we have
            // determined that n is prime and we can return true
            return true;
        }
    }
}
