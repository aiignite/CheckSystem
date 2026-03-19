using MathNet.Numerics.LinearAlgebra.Double;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CheckSystem.PesVision
{
    public class CurveAnalyzer
    {
        public class Point
        {
            public double X { get; set; }
            public double Y { get; set; }
            public Point(double x, double y) { X = x; Y = y; }
        }

        // 数据预处理（兼容.NET 4.5语法）
        public static List<Point> PreprocessPoints(List<Point> points)
        {
            var sorted = points.OrderBy(p => p.X).ToList();
            var invalidIndices = new HashSet<int>();

            for (int i = 0; i < sorted.Count - 2; i++)
            {
                double y1 = sorted[i].Y;
                double y2 = sorted[i + 1].Y;
                double y3 = sorted[i + 2].Y;

                bool increasing = (y1 <= y2) && (y2 <= y3);
                bool decreasing = (y1 >= y2) && (y2 >= y3);

                if (!(increasing || decreasing))
                {
                    invalidIndices.Add(i + 1);
                }
            }

            return sorted.Where((p, index) => !invalidIndices.Contains(index)).ToList();
        }

        // 多项式拟合（兼容旧版MathNet）
        public static bool TryPolynomialFit(List<Point> points, out double[] coefficients)
        {
            coefficients = new double[6];
            if (points.Count < 6) return false;

            var xArray = points.Select(p => p.X).ToArray();
            var yArray = points.Select(p => p.Y).ToArray();

            try
            {
                var design = Matrix.Build.DenseOfColumnVectors(
                    Vector.Build.Dense(xArray.Select(x => Math.Pow(x, 5)).ToArray()),
                    Vector.Build.Dense(xArray.Select(x => Math.Pow(x, 4)).ToArray()),
                    Vector.Build.Dense(xArray.Select(x => Math.Pow(x, 3)).ToArray()),
                    Vector.Build.Dense(xArray.Select(x => Math.Pow(x, 2)).ToArray()),
                    Vector.Build.Dense(xArray.Select(x => x).ToArray()),
                    Vector.Build.Dense(xArray.Length, 1.0));

                var yVector = Vector.Build.Dense(yArray);
                var solution = design.QR().Solve(yVector);

                coefficients[0] = solution[0]; // a
                coefficients[1] = solution[1]; // b
                coefficients[2] = solution[2]; // c
                coefficients[3] = solution[3]; // d
                coefficients[4] = solution[4]; // e
                coefficients[5] = solution[5]; // f
                return true;
            }
            catch
            {
                return false;
            }
        }

        // 导数计算结构体
        public struct Derivatives
        {
            public double[] FirstDerivCoeffs;
            public double[] SecondDerivCoeffs;
        }

       public static void CalculateDerivatives(double[] coeffs, out Derivatives derivatives)
        {
            derivatives = new Derivatives
            {
                FirstDerivCoeffs = new double[]
                {
                    5 * coeffs[0],  // ax^4
                    4 * coeffs[1],  // bx^3
                    3 * coeffs[2],  // cx^2
                    2 * coeffs[3],  // dx
                    coeffs[4]       // e
                },
                SecondDerivCoeffs = new double[]
                {
                    20 * coeffs[0], // ax^3
                    12 * coeffs[1], // bx^2
                    6 * coeffs[2],  // cx
                    2 * coeffs[3]   // d
                }
            };
        }

        // 拐点结构体
        public struct InflectionPoint
        {
            public Point Location;
            public string ConcavityType;
        }

       public static void FindCriticalPoints(double[] secondDerivCoeffs, double[] origCoeffs, out List<Point> extrema, out List<InflectionPoint> inflectionPoints)
        {
            extrema = new List<Point>();
            inflectionPoints = new List<InflectionPoint>();

            // 寻找二次导数极值点（三次导数根）
            var cubicDerivCoeffs = new double[]
            {
                60 * origCoeffs[0], // 3*20a
                24 * origCoeffs[1], // 2*12b
                6 * origCoeffs[2]   // 1*6c
            };

            foreach (var root in FindRealRoots(cubicDerivCoeffs))
            {
                double y = EvaluatePolynomial(secondDerivCoeffs, root);
                extrema.Add(new Point(root, y));
            }

            // 寻找拐点（二次导数根）
            foreach (var root in FindRealRoots(secondDerivCoeffs))
            {
                double y = EvaluatePolynomial(origCoeffs, root);
                double left = EvaluatePolynomial(secondDerivCoeffs, root - 1e-5);
                double right = EvaluatePolynomial(secondDerivCoeffs, root + 1e-5);

                var point = new InflectionPoint
                {
                    Location = new Point(root, y),
                    ConcavityType = (left > 0 && right < 0) ? "下凹点" : "上凹点"
                };
                inflectionPoints.Add(point);
            }
        }

        // 多项式求值
        static double EvaluatePolynomial(double[] coeffs, double x)
        {
            double result = 0;
            for (int i = 0; i < coeffs.Length; i++)
            {
                result += coeffs[i] * Math.Pow(x, coeffs.Length - 1 - i);
            }
            return result;
        }

        // 实数根查找（兼容旧版MathNet）
        static IEnumerable<double> FindRealRoots(double[] coeffs)
        {
            if (coeffs.Length == 3) // 二次方程
            {
                double a = coeffs[0], b = coeffs[1], c = coeffs[2];
                double discriminant = b * b - 4 * a * c;
                if (discriminant < 0) yield break;
                if (discriminant == 0) yield return -b / (2 * a);
                else
                {
                    yield return (-b + Math.Sqrt(discriminant)) / (2 * a);
                    yield return (-b - Math.Sqrt(discriminant)) / (2 * a);
                }
            }
            else if (coeffs.Length == 4) // 三次方程
            {
                // 系数顺序为 [x^3系数, x^2系数, x系数, 常数项]
                foreach (var root in SolveCubicAnalytic(coeffs[0], coeffs[1], coeffs[2], coeffs[3]))
                {
                    yield return root;
                }
            }
        }

        // 自定义立方根计算方法（兼容所有.NET版本）
        static double CubeRoot(double x)
        {
            if (x < 0)
                return -Math.Pow(-x, 1.0 / 3.0);  // 处理负数立方根
            else
                return Math.Pow(x, 1.0 / 3.0);     // 正数立方根
        }

        // 更新后的卡丹公式实现
        static List<double> SolveCubicAnalytic(double a, double b, double c, double d)
        {
            // 转换为简化形式: t³ + pt + q = 0
            double denominator = 3 * a * a;
            double p = (3 * a * c - b * b) / denominator;
            double q = (2 * b * b * b - 9 * a * b * c + 27 * a * a * d) / (27 * a * a * a);

            double discriminant = (q * q) / 4 + (p * p * p) / 27;

            List<double> realRoots = new List<double>();

            const double epsilon = 1e-14; // 浮点误差阈值

            if (discriminant > epsilon)
            {
                // 一个实根
                double sqrtD = Math.Sqrt(discriminant);
                double u = CubeRoot(-q / 2 + sqrtD);
                double v = CubeRoot(-q / 2 - sqrtD);
                realRoots.Add(u + v - b / (3 * a));
            }
            else if (Math.Abs(discriminant) <= epsilon)
            {
                // 两个实根（重复根）
                double u = CubeRoot(-q / 2);
                realRoots.Add(2 * u - b / (3 * a));
                realRoots.Add(-u - b / (3 * a));
            }
            else
            {
                // 三个实根（三角解)
                double theta = Math.Acos(3 * q * Math.Sqrt(-3 / p) / (2 * p));
                double sqrtTerm = 2 * Math.Sqrt(-p / 3);
                for (int k = 0; k < 3; k++)
                {
                    realRoots.Add(
                        sqrtTerm * Math.Cos((theta + 2 * Math.PI * k) / 3) -
                        b / (3 * a)
                    );
                }
            }

            // 过滤无效解（浮点误差修正）
            return realRoots
                .Where(root =>
                    Math.Abs(a * root * root * root +
                             b * root * root +
                             c * root + d) < 1e-10)
                .ToList();
        }
    }
}

//using System;
//using System.Collections.Generic;

//namespace CheckSystem.PesVision
//{
//    public class CurveAnalyzer
//    {
//        public class DataPoint
//        {
//            public double X { get; set; }
//            public double Y { get; set; }
//        }

//        public class AnalysisResult
//        {
//            public List<DataPoint> LocalMaxima { get; set; }
//            public List<DataPoint> LocalMinima { get; set; }
//            public List<DataPoint> InflectionPoints { get; set; }

//            public AnalysisResult()
//            {
//                LocalMaxima = new List<DataPoint>();
//                LocalMinima = new List<DataPoint>();
//                InflectionPoints = new List<DataPoint>();
//            }
//        }

//        public AnalysisResult AnalyzeCurve(List<DataPoint> dataPoints, double threshold = 1e-5)
//        {
//            var result = new AnalysisResult();
//            dataPoints.Sort((a, b) => a.X.CompareTo(b.X));
//            var derivatives = CalculateDerivatives(dataPoints);
//            FindExtremaAndInflections(dataPoints, derivatives, result, threshold);
//            return result;
//        }

//        // 修改后的导数计算方法
//        private List<Tuple<double, double>> CalculateDerivatives(List<DataPoint> points)
//        {
//            var derivatives = new List<Tuple<double, double>>();

//            for (int i = 1; i < points.Count - 1; i++)
//            {
//                // 计算一阶导数
//                double dx = points[i + 1].X - points[i - 1].X;
//                if (Math.Abs(dx) < 1e-10) // 防止除零
//                {
//                    derivatives.Add(Tuple.Create(double.NaN, double.NaN));
//                    continue;
//                }

//                double dy = points[i + 1].Y - points[i - 1].Y;
//                double firstDerivative = dy / dx;

//                // 计算二阶导数
//                double d2x = points[i + 1].X - 2 * points[i].X + points[i - 1].X;
//                if (Math.Abs(d2x) < 1e-10) // 二阶导数分母保护
//                {
//                    derivatives.Add(Tuple.Create(firstDerivative, double.NaN));
//                    continue;
//                }

//                double d2y = points[i + 1].Y - 2 * points[i].Y + points[i - 1].Y;
//                double secondDerivative = d2y / (d2x * d2x);

//                derivatives.Add(Tuple.Create(firstDerivative, secondDerivative));
//            }
//            return derivatives;
//        }

//        private void FindExtremaAndInflections(List<DataPoint> points,
//            List<Tuple<double, double>> derivatives,
//            AnalysisResult result,
//            double threshold)
//        {
//            for (int i = 1; i < derivatives.Count; i++)
//            {
//                // 添加NaN值检查
//                if (double.IsNaN(derivatives[i - 1].Item2) ||
//                    double.IsNaN(derivatives[i].Item2))
//                {
//                    continue; // 跳过无效数据点
//                }

//                // 极值检测（添加符号有效性检查）
//                if (IsValidSignChange(derivatives[i - 1].Item1, derivatives[i].Item1))
//                {
//                    var point = points[i + 1];
//                    if (!double.IsNaN(derivatives[i].Item2))
//                    {
//                        if (derivatives[i].Item2 < -threshold)
//                        {
//                            result.LocalMaxima.Add(point);
//                        }
//                        else if (derivatives[i].Item2 > threshold)
//                        {
//                            result.LocalMinima.Add(point);
//                        }
//                    }
//                }

//                // 拐点检测（添加零值保护）
//                if (IsValidSignChange(derivatives[i - 1].Item2, derivatives[i].Item2))
//                {
//                    result.InflectionPoints.Add(points[i + 1]);
//                }
//            }
//        }

//        // 辅助方法：安全检测符号变化
//        private bool IsValidSignChange(double a, double b)
//        {
//            // 排除NaN和无穷大值
//            if (double.IsNaN(a) || double.IsInfinity(a) ||
//                double.IsNaN(b) || double.IsInfinity(b))
//            {
//                return false;
//            }

//            // 处理接近零的情况
//            const double epsilon = 1e-10;
//            if (Math.Abs(a) < epsilon) a = 0;
//            if (Math.Abs(b) < epsilon) b = 0;

//            return Math.Sign(a) != Math.Sign(b);
//        }
//    }
//}

