using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace CommonUtility
{
    public static class HighPrecisionTimer
    {
        private static readonly bool IsHighResolution;
        private static readonly long Frequency;

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool QueryPerformanceFrequency(out long lpFrequency);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool QueryPerformanceCounter(out long lpPerformanceCount);

        static HighPrecisionTimer()
        {
            IsHighResolution = QueryPerformanceFrequency(out Frequency);
            if (!IsHighResolution)
            {
                //throw new Win32Exception();
                Console.WriteLine("High-resolution performance counter not supported");
            }
        }

        public static long GetTimestamp()
        {
            if (!IsHighResolution)
            {
                return Stopwatch.GetTimestamp();
                //throw new InvalidOperationException("High-resolution performance counter not supported");
            }

            long timestamp;
            QueryPerformanceCounter(out timestamp);
            return timestamp;
        }

        public static double GetMillisecondsElapsed(long startTimestamp)
        {
            if (!IsHighResolution)
            {
                // throw new InvalidOperationException("High-resolution performance counter not supported");
                var endTimestamp = Stopwatch.GetTimestamp();
                return (endTimestamp - startTimestamp) * 1000f / 10000000f;
            }
            else
            {
                long endTimestamp;
                QueryPerformanceCounter(out endTimestamp);
                return (endTimestamp - startTimestamp) * 1000.0 / Frequency;
            }
        }

        public static double GetTimestampIntervalMs(long startTimestamp, long endTimestamp)
        {
            if (!IsHighResolution)
                return (endTimestamp - startTimestamp) * 1000f / 10000000f;
            else
                return (endTimestamp - startTimestamp) * 1000.0 / Frequency;
        }
    }
}
