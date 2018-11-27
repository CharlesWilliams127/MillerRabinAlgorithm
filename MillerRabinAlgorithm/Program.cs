using System;
using System.Collections.Generic;
using System.Linq;

namespace MillerRabinAlgorithm
{
    class Program
    {
        static void Main(string[] args)
        {
            // create a mapping of non-prime numbers with "false-positive" prime tests
            Dictionary<int, double> primeProbablities = new Dictionary<int, double>();

            for(int n = 105001; n < 115000; n+=2)
            {
                // these two values will be used to count the # of total iterations
                // as well as the # of iterations that return prime
                int primeCount = 0;
                int totalCount = 0;

                // in order to achieve 3 digit precision we must test with 1000 x values
                for(int x = 0; x < 1000; x++)
                {
                    // if the number is potentially prime, increase the prime counter
                    if (MillerRabin(n))
                        primeCount++;
                    // increase the total counter regardless
                    totalCount++;
                }

                // # of iterations that returned likely prime, divided by total # of iterations
                double errorProb = (double)primeCount / (double)totalCount;

                // check using a slow but sure way if the value is actually prime
                // if not, but some of our tests counted it as prime we know we
                // have some false positives
                if (!IsPrime(n))
                {
                    // this number is not prime so we will add it to our error
                    // probability dictionary with its correct error percentage
                    primeProbablities.Add(n, errorProb);
                }
            }

            // sort our dictionary so that the values with the greatest error probability
            // are at the top
            var orderedErrorProbs = primeProbablities.OrderByDescending(p => p.Value);

            // write top 10 ints and their error probabilities
            for(int i = 0; i < 10; i++)
            {
                Console.WriteLine(orderedErrorProbs.ElementAt(i));
            }
        }

        /// <summary>
        /// The actual Miller-Rabin Primality Testing Algorithm, will return false if
        /// n is not a prime, and true if it is. 
        /// </summary>
        /// <param name="n">The number to perform the test on</param>
        /// <returns>True if n is a potential prime, false if not</returns>
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

            // represents the # of 2's that n - 1 can be factored into
            int s = 0;
            // represents the greatest prime factor of n-1
            int m = n - 1;

            // factor n - 1 to get the powers we need to raise a to
            while (m % 2 == 0)
            {
                s++;
                m = m / 2;
            }

            // perform checks to see if the number is prime
            Random r = new Random();
            // calculate an a value, this should be a number that is between 0
            // and n - 1
            int a = r.Next(n - 1) + 1;

            // create a temporary variable so we're not permanantly modifying m
            int temp = m;
            long mod = 1;
            for (int j = 0; j < temp; ++j)
            {
                mod = (mod * a) % n;
            }
            while (temp != n - 1 && mod != 1 && mod != n - 1)
            {
                mod = (mod * mod) % n;
                temp *= 2;
            }

            // if both of these conditions are met then our value is not a prime
            if (mod != n - 1 && temp % 2 == 0) return false;

            // otherwise, we have a potential prime
            return true;
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
