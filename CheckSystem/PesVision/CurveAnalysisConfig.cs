//using System;
//using System.Collections.Generic;
//using System.Linq;
//using Point = OpenCvSharp.Point2d;

//namespace CheckSystem.PesVision
//{
//    public class CurveAnalysisConfig
//    {
//        public double SmoothingSigma { get; set; }
//        public double NoiseThreshold { get; set; }
//        public int DownsampleEpsilon { get; set; }
//        public double ExtremumThreshold { get; set; }
//        public double InflectionThreshold { get; set; }

//        public CurveAnalysisConfig()
//        {
//            SmoothingSigma = 1.5;
//            NoiseThreshold = 0.05;
//            DownsampleEpsilon = 3;
//            ExtremumThreshold = 0.1;
//            InflectionThreshold = 0.02;
//        }
//    }

//    public class AnalysisResult
//    {
//        public List<Point> ExtremumPoints { get; private set; }
//        public List<Point> InflectionPoints { get; private set; }

//        public AnalysisResult(List<Point> extremums, List<Point> inflections)
//        {
//            ExtremumPoints = extremums;
//            InflectionPoints = inflections;
//        }
//    }

//    public static class CurveAnalyzer
//    {
//        public static AnalysisResult AnalyzeCurve(List<Point> rawPoints, CurveAnalysisConfig config)
//        {
//            var processedPoints = PreprocessData(rawPoints, config);
//            Derivatives derivatives = CalculateDerivatives(processedPoints);

//            return new AnalysisResult(
//                FindExtremums(processedPoints, derivatives.First, config),
//                FindInflections(processedPoints, derivatives.Second, config)
//            );
//        }

//        #region Preprocessing
//        private static List<Point> PreprocessData(List<Point> points, CurveAnalysisConfig config)
//        {
//            var smoothed = GaussianFilter(points, config.SmoothingSigma);
//            var downsampled = DouglasPeuckerReduce(smoothed, config.DownsampleEpsilon);
//            return downsampled.Where(p => p.Y > config.NoiseThreshold).ToList();
//        }

//        private static List<Point> GaussianFilter(List<Point> points, double sigma)
//        {
//            int kernelSize = (int)(sigma * 4);
//            kernelSize = kernelSize % 2 == 0 ? kernelSize + 1 : kernelSize;
//            double[] kernel = CreateGaussianKernel(kernelSize, sigma);

//            return points.Select((p, i) =>
//            {
//                if (i < kernelSize / 2 || i >= points.Count - kernelSize / 2)
//                    return p;

//                double sum = 0;
//                for (int j = -kernelSize / 2; j <= kernelSize / 2; j++)
//                    sum += points[i + j].Y * kernel[j + kernelSize / 2];

//                return new Point(p.X, sum);
//            }).ToList();
//        }

//        private static double[] CreateGaussianKernel(int size, double sigma)
//        {
//            double[] kernel = new double[size];
//            double sum = 0;
//            int half = size / 2;

//            for (int i = -half; i <= half; i++)
//            {
//                kernel[i + half] = Math.Exp(-(i * i) / (2 * sigma * sigma));
//                sum += kernel[i + half];
//            }

//            return kernel.Select(v => v / sum).ToArray();
//        }

//        private static List<Point> DouglasPeuckerReduce(List<Point> points, double epsilon)
//        {
//            if (points.Count <= 2) return new List<Point>(points);

//            double dmax = 0;
//            int index = 0;
//            Point start = points.First();
//            Point end = points.Last();

//            for (int i = 1; i < points.Count - 1; i++)
//            {
//                double d = PerpendicularDistance(points[i], start, end);
//                if (d > dmax)
//                {
//                    dmax = d;
//                    index = i;
//                }
//            }

//            if (dmax > epsilon)
//            {
//                var firstSegment = points.Take(index + 1).ToList();
//                var secondSegment = points.Skip(index).ToList();

//                var reduced1 = DouglasPeuckerReduce(firstSegment, epsilon);
//                var reduced2 = DouglasPeuckerReduce(secondSegment, epsilon);

//                return reduced1.Concat(reduced2).Distinct(new PointComparer()).ToList();
//            }
//            else
//            {
//                return new List<Point> { start, end };
//            }
//        }
//        #endregion

//        #region Derivatives
//        private struct Derivatives
//        {
//            public double[] First { get; set; }
//            public double[] Second { get; set; }
//        }

//        private static Derivatives CalculateDerivatives(List<Point> points)
//        {
//            int n = points.Count;
//            double[] first = new double[n];
//            double[] second = new double[n];

//            for (int i = 2; i < n - 2; i++)
//            {
//                double dx = points[i + 2].X - points[i - 2].X;
//                first[i] = (-points[i + 2].Y + 8 * points[i + 1].Y - 8 * points[i - 1].Y + points[i - 2].Y) / (12 * dx);
//            }

//            for (int i = 1; i < n - 1; i++)
//            {
//                double dx = points[i + 1].X - points[i].X;
//                if (Math.Abs(dx) < double.Epsilon) continue;

//                double dyPrev = (points[i].Y - points[i - 1].Y) / dx;
//                double dyNext = (points[i + 1].Y - points[i].Y) / dx;
//                second[i] = (dyNext - dyPrev) / dx;
//            }

//            return new Derivatives { First = first, Second = second };
//        }
//        #endregion

//        #region Feature Detection
//        private static List<Point> FindExtremums(List<Point> points, double[] firstDeriv, CurveAnalysisConfig config)
//        {
//            var extremums = new List<Point>();
//            for (int i = 1; i < firstDeriv.Length - 1; i++)
//            {
//                if (Math.Sign(firstDeriv[i]) != Math.Sign(firstDeriv[i + 1]))
//                {
//                    double deltaY = points[i + 1].Y - points[i].Y;
//                    if (Math.Abs(deltaY) > config.ExtremumThreshold)
//                    {
//                        double t = firstDeriv[i] / (firstDeriv[i] - firstDeriv[i + 1]);
//                        double x = points[i].X + t * (points[i + 1].X - points[i].X);
//                        double y = points[i].Y + t * (points[i + 1].Y - points[i].Y);
//                        extremums.Add(new Point(x, y));
//                    }
//                }
//            }
//            return extremums;
//        }

//        private static List<Point> FindInflections(List<Point> points, double[] secondDeriv, CurveAnalysisConfig config)
//        {
//            var inflections = new List<Point>();
//            for (int i = 2; i < secondDeriv.Length - 2; i++)
//            {
//                if (secondDeriv[i] * secondDeriv[i + 1] < 0)
//                {
//                    double k1 = Math.Abs(secondDeriv[i]);
//                    double k2 = Math.Abs(secondDeriv[i + 1]);
//                    if (Math.Abs(k1 - k2) > config.InflectionThreshold)
//                    {
//                        double t = secondDeriv[i] / (secondDeriv[i] - secondDeriv[i + 1]);
//                        double x = points[i].X + t * (points[i + 1].X - points[i].X);
//                        double y = points[i].Y + t * (points[i + 1].Y - points[i].Y);
//                        inflections.Add(new Point(x, y));
//                    }
//                }
//            }
//            return inflections;
//        }
//        #endregion

//        #region Utilities
//        private static double PerpendicularDistance(Point p, Point lineStart, Point lineEnd)
//        {
//            double dx = lineEnd.X - lineStart.X;
//            double dy = lineEnd.Y - lineStart.Y;
//            return Math.Abs(dy * p.X - dx * p.Y + lineEnd.X * lineStart.Y - lineEnd.Y * lineStart.X)
//                   / Math.Sqrt(dy * dy + dx * dx);
//        }

//        private class PointComparer : IEqualityComparer<Point>
//        {
//            public bool Equals(Point a, Point b)
//            {
//                return a.X == b.X && a.Y == b.Y;
//            }

//            public int GetHashCode(Point obj)
//            {
//                return obj.X.GetHashCode() ^ obj.Y.GetHashCode();
//            }
//        }
//        #endregion
//    }
//}
