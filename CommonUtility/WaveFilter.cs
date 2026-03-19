using System;
using System.Collections.Generic;

namespace CommonUtility
{
    public static class WaveFilter
    {
        //AD采集次数
        //private static int _filterNumber = 10;

        public static float MidianAverageFileter(float[] floats)
        {
            if (floats == null || floats.Length == 0)
            {
                return -9999f;
            }

            try
            {
                var filterNumber = floats.Length;

                var valueAdList = new List<float>();
                valueAdList.AddRange(floats);
                //for (var i = 0; i < _filterNumber; i++)
                //{               
                //    //Trace.WriteLine(valueADList[i]);
                //    //Thread.Sleep(2);
                //}

                //冒泡排序
                for (var j = 0; j < filterNumber - 1; j++)
                {
                    for (var i = 0; i < filterNumber - 1 - j; i++)
                    {
                        if (!(valueAdList[i] > valueAdList[i + 1]))
                            continue;
                        var temp = valueAdList[i];
                        valueAdList[i] = valueAdList[i + 1];
                        valueAdList[i + 1] = temp;
                    }
                }

                var sum = 0F;
                const int n = 4; //N必须为偶数
                //去掉N/2个最大值和最小值
                for (var i = n / 2; i < filterNumber - n / 2; i++)
                    sum += valueAdList[i];
                return sum / (filterNumber - n);
            }
            catch (Exception e)
            {
                return floats[0];
            }
        }

        /// <summary>
        /// 移动平均,曲线平滑
        /// </summary>
        /// <param name="rawData">原曲线数组</param>
        /// <param name="step">步长</param>
        /// <returns></returns>
        public static unsafe double[] Smoothing(double[] rawData, int step = 3)
        {
            var smooth = new double[rawData.Length];
            fixed (double* o = smooth, r = rawData)
            {
                for (var i = step; i < rawData.Length; i++)
                {
                    double total = 0;
                    var s = step * 2 + 1;
                    for (var j = i - step; j < i + step + 1; j++)
                    {
                        if (j < rawData.Length)
                        {
                            total += r[j];
                        }
                        else
                        {
                            break;
                        }
                    }
                    o[i] = total / s;
                }

                //Head fill
                for (var i = 0; i < step; i++)
                {
                    o[i] = o[step];
                }
                //Tail fill
                var tail = rawData.Length - (rawData.Length % (step + 1)) - 1;
                for (var j = tail; j < rawData.Length; j++)
                {
                    o[j] = o[tail - 1];
                }
            }

            return smooth;
        }

        /// <summary>
        /// 中值滤波,去毛刺
        /// </summary>
        /// <param name="rawData"></param>
        /// <param name="step"></param>
        /// <returns></returns>
        public static unsafe double[] MedianFilter(double[] rawData, int step = 3)
        {
            var length = step * 2 + 1;
            var smooth = new double[rawData.Length];
            var median = new double[length];
            fixed (double* o = smooth, r = rawData, m = median)
            {
                for (var i = step; i < rawData.Length; i++)
                {
                    var s = i - step;
                    var k = 0;
                    for (var j = i - step; j < i + step + 1; j++)
                    {
                        if (j < rawData.Length)
                        {
                            m[k] = r[j];
                        }
                        else
                        {
                            break;
                        }
                        k++;
                    }
                    o[i] = SortBubbleAscendingOrder(median)[step];
                }
                //Head fill
                for (var i = 0; i < step; i++)
                {
                    o[i] = o[step];
                }
                //Tail fill
                var tail = rawData.Length - (rawData.Length % (step + 1)) - 1;
                for (var j = tail; j < rawData.Length; j++)
                {
                    o[j] = o[tail - 1];
                }
            }

            return smooth;
        }

        /// <summary>
        /// 冒泡升序
        /// </summary>
        /// <param name="rawData"></param>
        /// <returns></returns>
        private static unsafe double[] SortBubbleAscendingOrder(double[] rawData)
        {
            var outResult = new double[rawData.Length];
            fixed (double* o = outResult, r = rawData)
            {
                for (var i = 0; i < rawData.Length; i++)
                {
                    o[i] = r[i];
                    for (var j = i; j > 0; j--)
                    {
                        if (!(o[j] < o[j - 1]))
                            continue;
                        var t = o[j];
                        o[j] = o[j - 1];
                        o[j - 1] = t;
                    }
                }
            }

            return outResult;
        }

        /// <summary>
        /// 冒泡降序
        /// </summary>
        /// <param name="rawData"></param>
        /// <returns></returns>
        private static unsafe double[] SortBubbleDescendingOrder(double[] rawData)
        {
            var outResult = new double[rawData.Length];
            fixed (double* o = outResult, r = rawData)
            {
                for (var i = 0; i < rawData.Length; i++)
                {
                    o[i] = r[i];
                    for (var j = i; j > 0; j--)
                    {
                        if (!(o[j] > o[j - 1]))
                            continue;
                        var t = o[j];
                        o[j] = o[j - 1];
                        o[j - 1] = t;
                    }
                }
            }

            return outResult;
        }
    }
}
