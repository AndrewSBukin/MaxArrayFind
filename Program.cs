using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Running;

namespace ConsoleApp12
{
    using Couple = (int a, int c);
    using Triple = ((int a, int c) p1, (int a, int c) p2, (int a, int c) p3);

    public class Program
    {
        public static void Main(string[] args)
        {
            var mergeArraySummary = BenchmarkRunner.Run<SubArraysBenchmark>();
        }

        //[RPlotExporter]
        [MemoryDiagnoser]
        //[SimpleJob] // external-process execution
        //[InProcess] // in-process execution
        //[HardwareCounters(
        //    HardwareCounter.CacheMisses,      // Промахи кэша
        //    HardwareCounter.InstructionRetired, // Количество инструкций
        //    HardwareCounter.BranchMispredictions // Ошибки предсказания ветвлений
        //)]
        public class SubArraysBenchmark
        {
            const int DATA_LENGTH = 10000;
            private static (int From, int To) _arrayLength = (0, 5000);
            private static (int From, int To) _arrayStart = (1, 100);
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
            int i = 0;
            int curSum = data[i];
            int curSum2 = 0;
            int a = data[i];
            int b = 0;
            i++;
            for (; i < data.Length; i++)
            {
                int n = data[i];
                if (n == a)
                    curSum += n;
                else
                {
                    b = n;
                    curSum += n;
                    curSum2 = n;
                    i++;
                    break;
                }
            }
            for (; i < data.Length; i++)
            {
                int n = data[i];
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
}
