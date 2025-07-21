using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Running;

namespace ConsoleApp12
{
    using Couple = (int a, int c);
    using Triple = ((int a, int c) p1, (int a, int c) p2, (int a, int c) p3);
    public static class A
    {

        public static (int a, int c) Single(this int x) => (x, 1);
        public static Couple Increment(this Couple p) => (p.a, p.c + 1);

        // Scan [1,2,3,4,5] 0 (+) => [0,1,3,6,10,15]
        internal static IEnumerable<TAccumulate> Scan<TSource, TAccumulate>(
            this IEnumerable<TSource> source,
            TAccumulate seed,
            Func<TAccumulate, TSource, TAccumulate> func)
        {
            yield return seed;
            foreach (var item in source)
            {
                seed = func(seed, item);
                yield return seed;
            }
        }
    }
    public class Program
    {
        static int[] nums1 = [1, 2, 3, 4, 10, 10, 4, 10, 5, 5, 5];
        static int[] nums2 = [8, 7, 2, 6, 4, 6, 5, 7, 9, 9, 10, 5, 3, 3, 2, 6, 3, 1, 1, 4, 4, 2, 7, 8, 8, 2, 9, 2, 1, 1, 1, 3, 4, 5, 1, 6, 7, 1, 2, 6, 4, 4, 6, 4, 7, 9, 8, 6, 2, 9, 10, 3, 8, 9, 6, 10, 10, 3, 6, 9, 1, 4, 7, 4, 7, 8, 2, 4, 9, 5, 1, 7, 9, 4, 9, 10, 6, 9, 2, 10, 5, 6, 10, 1, 2, 4, 4, 7, 2, 2, 6, 3, 10, 1, 4, 10, 5, 9, 6, 5, 2, 10, 2, 7, 4, 8, 5, 6, 8, 3, 2, 6, 8, 2, 3, 3, 3, 7, 8, 8, 8, 9, 5, 4, 4, 5, 2, 6, 10, 2, 7];
        static int[] nums4 = [9, 5, 1, 7, 7, 8, 2, 2, 4, 6, 4, 9, 5, 8, 10, 2, 4, 9, 6, 4, 2, 10, 9, 6, 7, 8, 5, 6, 10, 7, 2, 3, 4, 6, 4, 7, 2, 4, 6, 1, 4, 6, 10, 10, 2, 8, 1, 9, 3, 7, 10, 6, 6, 5, 8, 10, 9, 8, 3, 7, 6, 1, 2, 4, 5, 3, 5, 9, 2, 7, 5, 4, 2, 7, 2, 3, 1, 3, 7, 8, 4, 4, 8, 5, 8, 6, 10, 2, 5, 1, 8, 8, 7, 7, 2, 9, 2, 3, 4, 1, 4, 9, 10, 3, 5, 9, 4, 7, 7, 3, 4, 2, 5, 3, 7, 10, 7, 7, 3, 3, 4, 7, 5, 9, 8, 7, 9, 6, 5, 7, 6, 2, 5, 2, 5, 8, 6, 10, 7, 6, 3, 3, 3, 9, 5, 7, 7, 9, 8, 8, 8, 6, 1, 8, 8, 8, 5, 10, 4, 2, 4, 5, 6, 5, 3, 2, 4, 7, 5, 6, 9, 7, 6, 4, 9, 1, 9, 4, 10, 7, 3, 8, 4, 2, 8, 3, 2, 4, 4, 3, 10, 9, 9, 5, 9, 2, 6, 7, 9, 5, 3, 5, 2, 1, 9, 7, 6, 4, 8, 3, 6, 9, 3, 5, 5, 10, 6, 4, 9, 6, 7, 3, 1, 5, 8, 5, 4, 4, 3, 9, 6, 7, 9, 5, 5, 5, 4, 2, 3, 4, 10, 2, 8, 5, 6, 8, 4, 8, 6, 8, 4, 3, 4, 8, 7, 9, 8, 2, 5, 1, 2, 5, 6, 4, 6, 2, 8, 10, 3, 1, 8, 7, 6, 6, 6, 1, 7, 6, 6, 8, 6, 8, 6, 1, 4, 2, 4, 9, 9, 4, 1, 4, 7, 2, 3, 9, 7, 9, 4, 6, 5, 1, 5, 3, 4, 8, 3, 3, 5, 7, 6, 2, 5, 8, 2, 8, 4, 8, 8, 2, 6, 8, 8, 9, 7, 6, 8, 8, 3, 2, 6, 6, 6, 2, 3, 4, 7, 2, 6, 3, 10, 5, 2, 6, 2, 5, 6, 7, 9, 3, 9, 5, 4, 9, 8, 8, 7, 6, 6, 5, 5, 5, 7, 4, 2, 7, 6, 2, 8, 8, 10, 6, 2, 10, 5, 6, 9, 10, 10, 4, 5, 10, 5, 9, 3, 8, 2, 5, 8, 9, 7, 4, 7, 5, 3, 5, 6, 9, 6, 3, 4, 4, 9, 9, 6, 8, 6, 3, 1, 8, 10, 7, 7, 5, 4, 5, 1, 9, 5, 9, 7, 3, 6, 8, 9, 7, 3, 9, 10, 6, 1, 8, 1, 4, 6, 6, 5, 10, 7, 9, 3, 3, 9, 3, 3, 9, 6, 6, 2, 2, 7, 5, 7, 9, 4, 6, 8, 9, 3, 2, 7, 2, 10, 8, 8, 10, 7, 5, 5, 10, 1, 3, 5, 6, 7, 6, 4, 9, 5, 4, 8, 7, 7, 3, 10, 3, 4, 6, 3, 7, 1, 6, 9, 1, 7, 9, 8, 4, 6, 8, 8, 5, 2, 5, 3, 6, 10, 2, 3, 3, 9, 9, 3, 8, 4, 7, 6, 9, 8, 8, 2, 6, 4, 8, 6, 9, 6, 3, 5, 6, 2, 1, 5, 6, 5, 3, 8, 1, 1, 9, 10, 6, 9, 2, 5, 7, 8, 9, 8, 7, 1, 5, 1, 1, 1, 8, 8, 4, 9, 8, 10, 2, 8, 3, 4, 5, 9, 6, 8, 7, 3, 2, 9, 1, 7, 3, 4, 4, 6, 2, 2, 2, 8, 5, 5, 4, 3, 4, 3, 7, 2, 6, 8, 3, 8, 3, 5, 5, 8, 3, 5, 6, 8, 3, 2, 9, 7, 2, 2, 4, 7, 2, 4, 4, 7, 6, 1, 10, 3, 8, 7, 9, 7, 6, 6, 5, 4, 8, 6, 1, 6, 5, 2, 5, 4, 3, 5, 3, 9, 5, 8, 2, 2, 5, 2, 1, 2, 1, 7, 9, 8, 4, 7, 8, 2, 8, 4, 3, 5, 6, 7, 6, 6, 9, 3, 7, 9, 6, 10, 4, 4, 9, 6, 1, 7, 10, 5, 3, 10, 3, 8, 1, 3, 7, 4, 7, 6, 8, 3, 7, 6, 2, 5, 9, 3, 9, 8, 4, 8, 9, 3, 7, 5, 5, 9, 9, 7, 6, 2, 8, 6, 8, 7, 1, 8, 2, 4, 8, 1, 1, 8, 1, 3, 3, 10, 3, 4, 9, 5, 5, 8, 8, 7, 1, 2, 5, 1, 9, 3, 9, 7, 10, 8, 8, 9, 8, 5, 3, 10, 7, 2, 2, 3, 4, 9, 5, 4, 2, 8, 2, 4, 6, 3, 6, 10, 9, 2, 9, 3, 4, 5, 1, 9, 6, 6, 6, 5, 6, 3, 3, 6, 3, 8, 2, 2, 9, 4, 2, 4, 5, 9, 3, 6, 2, 2, 7, 10, 2, 1, 9, 9, 1, 5, 8, 8, 8, 4, 3, 4, 5, 4, 6, 8, 4, 10, 10, 8, 5, 2, 8, 8, 7, 5, 2, 3, 2, 7, 9, 5, 3, 2, 6, 6, 2, 6, 5, 3, 10, 8, 2, 3, 5, 7, 9, 7, 5, 5, 3, 2, 6, 2, 7, 2, 3, 9, 10, 10, 1, 5, 9, 9];
        public static void Main(string[] args)
        {
            var mergeArraySummary = BenchmarkRunner.Run<SubArraysBenchmark>();
            //var summary = BenchmarkRunner.Run<MyBenchmark>();
        }

        [MemoryDiagnoser]
        //[HardwareCounters(
        //    HardwareCounter.CacheMisses,      // Промахи кэша
        //    HardwareCounter.InstructionRetired, // Количество инструкций
        //    HardwareCounter.BranchMispredictions // Ошибки предсказания ветвлений
        //)]
        public class MyBenchmark
        {
            [Benchmark]
            public void TestMethod1()
            {
                Method3_insanity13(nums1);
            }
            [Benchmark]
            public void TestMethod2()
            {
                Method3_insanity13(nums2);
            }
            [Benchmark]
            public void TestMethod3()
            {
                Method3_insanity13(nums4);
            }
        }

        //[RPlotExporter]
        [MemoryDiagnoser]
        //[SimpleJob] // external-process execution
        //[InProcess] // in-process execution
        public class SubArraysBenchmark
        {
            const int DATA_LENGTH = 10000;
            private static (int From, int To) _arrayLength = (0, 5000);
            private static (int From, int To) _arrayStart = (0, 9999);
            private static readonly Random _random = new(DateTime.Now.Millisecond);
            private IReadOnlyCollection<int[]> _testData;


            [GlobalSetup]
            public void Setup()
            {
                _testData = Enumerable.Range(0, DATA_LENGTH)
                    .Select(x =>
                        Enumerable.Range(0, _random.Next(_arrayLength.From, _arrayLength.To))
                    .Select(x => _random.Next(_arrayStart.From, _arrayStart.To)).ToArray()).ToArray();
            }

            [Benchmark]
            public void DimsFromDergachyVersion()
            {
                foreach (var testItem in _testData)
                    Method2_DimsFromDergachy(testItem);
            }

            [Benchmark]
            public void DenisNP_Version()
            {
                foreach (var testItem in _testData)
                    Method1_DenisNP(testItem);
            }

            [Benchmark]
            public void insanity13_Version()
            {
                foreach (var testItem in _testData)
                    Method3_insanity13(testItem);
            }

            [Benchmark]
            public void Andrewsbukin_Version()
            {
                foreach (var testItem in _testData)
                    Method4_andrewsbukin(testItem);
            }
        }
        static int Method1_DenisNP(int[] data)
        {
            int left = 0;
            int right = 0;
            bool movingRight = true;
            bool isSumValid = true;
            int currentSum = 0;
            int maxSum = int.MinValue;
            Dictionary<int, int> counts = new();

            while (true)
            {
                if (movingRight)
                {
                    if (right == data.Length)
                    {
                        return maxSum;
                    }
                    int rEl = data[right];
                    right++;

                    currentSum += rEl;
                    if (!counts.ContainsKey(rEl))
                    {
                        counts[rEl] = 1;
                    }
                    else
                    {
                        counts[rEl]++;
                    }

                    if (counts.Count > 2)
                    {
                        isSumValid = false;
                        movingRight = false;
                    }
                }
                else
                {
                    if (left == data.Length)
                    {
                        return maxSum;
                    }

                    int lEl = data[left];
                    left++;

                    currentSum -= lEl;
                    if (counts.ContainsKey(lEl))
                    {
                        counts[lEl]--;
                        if (counts[lEl] == 0)
                        {
                            counts.Remove(lEl);
                        }
                    }

                    if (counts.Count <= 2)
                    {
                        isSumValid = true;
                        movingRight = true;
                    }
                }

                if (isSumValid && currentSum > maxSum)
                {
                    maxSum = currentSum;
                }
            }
        }
        static int Method4_andrewsbukin(int[] data)
        {
            if (data.Length <= 2)
                return data.Sum();

            int res = 0;
            int curSum = 0;
            int curSum2 = 0;
            int a = 0;
            int b = 0;
            for (int i = 0; i < data.Length; i++)
            {
                int n = data[i];
                if(a == 0)
                {
                    a = n;
                    curSum += n;
                }
                else
                {
                    if(b == 0)
                    {
                        b = n;
                        curSum += n;
                        curSum2 = n;
                    }
                    else
                    {
                        if(n == a || n == b)
                        {
                            curSum += n;
                            if(n == b)
                            {
                                curSum2 += n;
                            }
                            else
                            {
                                int t = a;
                                a = b;
                                b = t;
                                curSum2 = n;
                            }
                        }
                        else
                        {
                            if (curSum > res) res = curSum;
                            curSum = curSum2 + n;
                            curSum2 = n;
                            a = b;
                            b = n;
                        }
                    }
                }
            }

            return res;
        }

        static int Method3_insanity13(int[] data)
        {
            if (data.Length <= 2)
                return data.Sum();

            int maxSum = 0;
            Span<int> numberTypes = stackalloc int[2];

            for (int startIndex = 0; startIndex < data.Length - 1; startIndex++)
            {
                numberTypes[0] = data[startIndex];
                int subSum = data[startIndex];
                int endIndex = startIndex + 1;
                bool isSecondFound = false;

                while (endIndex < data.Length && (!isSecondFound || numberTypes.Contains(data[endIndex])))
                {
                    if (!isSecondFound && numberTypes[0] != data[endIndex])
                    {
                        numberTypes[1] = data[endIndex];
                        isSecondFound = true;
                    }

                    subSum += data[endIndex];
                    endIndex++;
                }

                if (subSum > maxSum)
                    maxSum = subSum;

                if (endIndex >= data.Length)
                    break;
            }

            return maxSum;
        }

        static int Method2_DimsFromDergachy(int[] data)
        {
            Triple start = ((0, 0), (0, 0), (0, 0));
            return data.Scan(start, Next)
                     .Max(t => t.p1.a * t.p1.c + t.p2.a * t.p2.c);
        }

        private static Triple Next(Triple prev, int x)
        {
            (var p1, var p2, var p3) = prev;
            if (p1.a == x)
                return (p2, p1.Increment(), x.Single());

            if (p2.a == x)
                return (p1, p2.Increment(), p3.Increment());

            return (p3, x.Single(), x.Single());
        }
    }
}
