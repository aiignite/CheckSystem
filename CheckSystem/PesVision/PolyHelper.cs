using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;
using OpenCvSharp;

namespace CheckSystem.PesVision
{
    public static class PolyHelper
    {
        public enum ExtremumType { Peak, Valley }

        public class Extremum
        {
            public double SecondDeriv;
            public double XActual;
            public double YValue;
            public ExtremumType ExtremumType;

            public Extremum(double xActual, double yValue, double[] coeff2Nd)
            {
                XActual = xActual;
                YValue = yValue;
                SecondDeriv = EvaluatePolynomial(coeff2Nd, xActual);
                ExtremumType = SecondDeriv < 0 ? ExtremumType.Peak : ExtremumType.Valley;
            }

            public Extremum(double xActual, double yValue, double secondDeriv)
            {
                XActual = xActual;
                YValue = yValue;
                SecondDeriv = secondDeriv;
                ExtremumType = SecondDeriv < 0 ? ExtremumType.Peak : ExtremumType.Valley;
            }
        }

        /// <summary>
        /// 数据标准化处理
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public static Tuple<double[], double[], double, double> NormalizeData(
            List<Tuple<double, double>> points)
        {
            // double[] xNorm, double[] yNorm, double xMean, double xStd

            // 数据标准化
            var x = points.Select(p => p.Item1).ToArray();
            var y = points.Select(p => p.Item2).ToArray();

            var xMean = x.Average();
            var xStd = x.StandardDeviation();
            var yMean = y.Average();

            return Tuple.Create(x, y, xMean, xStd);
        }

        /// <summary>
        /// 带正则化的多项式拟合（岭回归）
        /// </summary>
        /// <param name="xNorm"></param>
        /// <param name="yNorm"></param>
        /// <param name="order"></param>
        /// <param name="lambda"></param>
        /// <returns></returns>
        public static double[] RidgePolynomialFit(double[] xNorm, double[] yNorm, int order, double lambda = 0.1)
        {
            //// 构建设计矩阵
            //var design = Matrix<double>.Build.DenseOfRowArrays(
            //    xNorm.Select(xi => Enumerable.Range(0, order + 1)
            //        .Select(j => Math.Pow(xi, j)).ToArray()));

            //// 添加正则化项
            //var regularization = Matrix<double>.Build.DiagonalIdentity(order + 1) * lambda;
            //var coefficients = (design.Transpose() * design + regularization).Inverse() * design.Transpose() * Vector<double>.Build.Dense(yNorm);

            //return coefficients.ToArray();

            //var fitResult= Polynomial.Fit(xNorm, yNorm, order);
            //return fitResult.Coefficients;

            return Fit.Polynomial(xNorm, yNorm, order);
        }

        /// <summary>
        /// 计算导数的系数
        /// </summary>
        /// <param name="coefficients"></param>
        /// <returns></returns>
        public static double[] CalculateDerivativeCoefficients(double[] coefficients)
        {
            //var deriv = new List<double>();
            //for (var i = 1; i < coefficients.Length; i++)
            //{
            //    deriv.Add(coefficients[i] * i);
            //}

            //return deriv.ToArray();

            return Enumerable.Range(1, coefficients.Length - 1).Select(i => coefficients[i] * i).ToArray();
        }

        /// <summary>
        /// 多项式求值
        /// </summary>
        /// <param name="coefficients"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        public static double EvaluatePolynomial(double[] coefficients, double x)
        {
            return Polynomial.Evaluate(x, coefficients);
            //return coefficients.Select((t, i) => t * Math.Pow(x, i)).Sum();
        }

        /// <summary>
        /// 寻找极值点方法
        /// </summary>
        /// <param name="points"></param>
        /// <param name="coeff"></param>
        /// <param name="coeff1St"></param>
        /// <param name="coeff2Nd"></param>
        /// <param name="xMean"></param>
        /// <param name="xStd"></param>
        /// <returns></returns>
        public static List<Extremum> FindExtrema(List<Tuple<double, double>> points, double[] coeff, double[] coeff1St, double[] coeff2Nd, double xMean, double xStd)
        {
            var extrema = new List<Extremum>();
            // 求一阶导数根
            var xRange = points.Select(p => p.Item1).ToList();
            var roots = FindRealRoots(coeff1St, xRange.Min(), xRange.Max());

            foreach (var root in roots)
            {
                // 计算二阶导数值
                var secondDerivValue = EvaluatePolynomial(coeff2Nd, root);

                if (Math.Abs(secondDerivValue) < 1e-6)
                    continue;

                extrema.Add(new Extremum(root, EvaluatePolynomial(coeff, root), secondDerivValue));

                //if (secondDerivValue < 0)
                //{
                //    result.Peaks.Add(root);
                //}
                //else
                //{
                //    result.Valleys.Add(root);
                //}
            }
            return extrema;

            //var extrema = new List<Extremum>();
            //var prevSlope = double.NaN;

            //for (var i = 0; i < points.Count - 1; i++)
            //{
            //    var x1 = points[i].Item1;
            //    var x2 = points[i + 1].Item1;

            //    // 标准化处理
            //    var xNorm1 = (x1 - xMean) / xStd;
            //    var xNorm2 = (x2 - xMean) / xStd;

            //    var slope1 = EvaluatePolynomial(coeff1St, xNorm1);
            //    var slope2 = EvaluatePolynomial(coeff1St, xNorm2);

            //    // 检测斜率符号变化
            //    if (Math.Sign(slope1) != Math.Sign(slope2))
            //    {
            //        // 使用二分法寻找精确解
            //        var root = FindRoots.OfFunction(x => EvaluatePolynomial(coeff1St, x), xNorm1, xNorm2, 1e-6);

            //        // 逆标准化
            //        var xActual = root * xStd + xMean;
            //        var yValue = EvaluatePolynomial(coeff, xActual);
            //        var secondDeriv = EvaluatePolynomial(coeff2Nd, xActual);

            //        extrema.Add(new Extremum(xActual, yValue, secondDeriv));
            //    }
            //}
            //return extrema;
        }

        // 在指定范围内寻找实数根
        private static List<double> FindRealRoots(double[] coefficients, double minX, double maxX)
        {
            Console.WriteLine($"正在求解 {coefficients.Length - 1} 阶多项式的根");
            Console.WriteLine($"多项式系数: [{string.Join(", ", coefficients)}]");

            var roots = new List<double>();
            try
            {
                var poly = new Polynomial(coefficients);
                var allRoots = poly.Roots();

                foreach (var root in allRoots)
                {
                    //if (root.Imaginary == 0 && root.Real >= minX && root.Real <= maxX)
                    //    roots.Add(root.Real);

                    if (Math.Abs(root.Imaginary) < 1e-8 && root.Real >= minX && root.Real <= maxX)
                    {
                        roots.Add(root.Real);
                        Console.WriteLine($"接受根: {root.Real} (虚部: {root.Imaginary})");
                    }
                    else
                    {
                        Console.WriteLine($"拒绝根: {root} (范围外或虚部过大)");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"求根错误: {ex.Message}");
            }
            return roots;
        }

        public static List<double> FindNumericalInflectionPoints(double[] x, double[] y)
        {
            List<double> inflectionPoints = new List<double>();

            // 计算二阶导数数值近似
            double[] d2y = new double[y.Length];
            for (int i = 2; i < y.Length - 2; i++)
            {
                // 使用五阶中心差分公式提高精度
                d2y[i] = (-y[i + 2] + 16 * y[i + 1] - 30 * y[i] + 16 * y[i - 1] - y[i - 2]) / (12 * Math.Pow(x[1] - x[0], 2));
            }

            // 寻找过零点
            for (int i = 1; i < d2y.Length; i++)
            {
                if (d2y[i] * d2y[i - 1] < 0) // 符号变化检测
                {
                    // 线性插值精确化
                    double t = d2y[i - 1] / (d2y[i - 1] - d2y[i]);
                    double x0 = x[i - 1] + t * (x[i] - x[i - 1]);
                    inflectionPoints.Add(x0);
                }
            }

            return inflectionPoints.Distinct().ToList();
        }

        #region 高斯核滤波

        public static List<Point> PreprocessData(List<Point> points)
        {
            var smoothed = GaussianFilter(points, 1.5);
            var downsampled = DouglasPeuckerReduce(smoothed,3);
            return downsampled.Where(p => p.Y > 0.05).ToList();
        }

        private static List<Point> GaussianFilter(List<Point> points, double sigma)
        {
            int kernelSize = (int)(sigma * 4);
            kernelSize = kernelSize % 2 == 0 ? kernelSize + 1 : kernelSize;
            double[] kernel = CreateGaussianKernel(kernelSize, sigma);

            return points.Select((p, i) =>
            {
                if (i < kernelSize / 2 || i >= points.Count - kernelSize / 2)
                    return p;

                double sum = 0;
                for (int j = -kernelSize / 2; j <= kernelSize / 2; j++)
                    sum += points[i + j].Y * kernel[j + kernelSize / 2];

                return new Point(p.X, sum);
            }).ToList();
        }

        private static double[] CreateGaussianKernel(int size, double sigma)
        {
            double[] kernel = new double[size];
            double sum = 0;
            int half = size / 2;

            for (int i = -half; i <= half; i++)
            {
                kernel[i + half] = Math.Exp(-(i * i) / (2 * sigma * sigma));
                sum += kernel[i + half];
            }

            return kernel.Select(v => v / sum).ToArray();
        }

        private static List<Point> DouglasPeuckerReduce(List<Point> points, double epsilon)
        {
            if (points.Count <= 2) return new List<Point>(points);

            double dmax = 0;
            int index = 0;
            Point start = points.First();
            Point end = points.Last();

            for (int i = 1; i < points.Count - 1; i++)
            {
                double d = PerpendicularDistance(points[i], start, end);
                if (d > dmax)
                {
                    dmax = d;
                    index = i;
                }
            }

            if (dmax > epsilon)
            {
                var firstSegment = points.Take(index + 1).ToList();
                var secondSegment = points.Skip(index).ToList();

                var reduced1 = DouglasPeuckerReduce(firstSegment, epsilon);
                var reduced2 = DouglasPeuckerReduce(secondSegment, epsilon);

                return reduced1.Concat(reduced2).Distinct(new PointComparer()).ToList();
            }
            else
            {
                return new List<Point> { start, end };
            }
        }

        private static double PerpendicularDistance(Point p, Point lineStart, Point lineEnd)
        {
            double dx = lineEnd.X - lineStart.X;
            double dy = lineEnd.Y - lineStart.Y;
            return Math.Abs(dy * p.X - dx * p.Y + lineEnd.X * lineStart.Y - lineEnd.Y * lineStart.X)
                   / Math.Sqrt(dy * dy + dx * dx);
        }

        private class PointComparer : IEqualityComparer<Point>
        {
            public bool Equals(Point a, Point b)
            {
                return a.X == b.X && a.Y == b.Y;
            }

            public int GetHashCode(Point obj)
            {
                return obj.X.GetHashCode() ^ obj.Y.GetHashCode();
            }
        }

        #endregion
    }
}
