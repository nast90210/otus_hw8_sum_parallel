using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace otus_hw9_sum_parallel
{
    class Program
    {
        static void Main(string[] args)
        {
            var col1 = GetArray(100000);
            var col2 = GetArray(1000000);
            var col3 = GetArray(10000000);

            var sw = new Stopwatch();
            
            sw.Start();
            var result1 = Sum(col1);
            sw.Stop();
            
            Console.WriteLine(result1);
            Console.WriteLine("Simple " + sw.Elapsed);
            sw.Reset();
            
            sw.Start();
            var result2 = ThreadSum(col1);
            sw.Stop();

            Console.WriteLine(result2);
            Console.WriteLine("Thread " + sw.Elapsed);
            sw.Reset();
            
            sw.Start();
            var result3 = ParallelSum(col1);
            sw.Stop();
            
            Console.WriteLine(result3);
            Console.WriteLine("Parallel " + sw.Elapsed);
            
        }

        private static int ParallelSum(int[] array)
        {
            int result = array.AsParallel().Sum();
            return result;
        }

        private static int ThreadSum(int[] array)
        {
            var result = 0;

            var splitArray = new List<int[]>();
            var z = array.Length / 10;
            
            for (int i = 0; i < 10; i++)
            {
                splitArray.Add(array.Skip(z * i).Take(z).ToArray());
            }

            foreach (var tempArray in splitArray)
            {
                var thread = new Thread(() => result += Sum(tempArray));     
                thread.Start();                    
                thread.Join();
            }
            
            return result;
        }

        private static int Sum(int[] array)
        {
            var result = 0;

            for (int i = 0; i < array.Length; i++)
            {
                result += array[i];
            }
            
            return result;
        }
        
        private static int[] GetArray(int max)
        {
            var result = new int[max];
            for (int i = 1; i < max ; i++)
            {
                result[i] = i;
            }

            return result;
        }
    }
}