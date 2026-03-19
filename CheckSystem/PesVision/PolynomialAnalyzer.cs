using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.LinearAlgebra;

namespace CheckSystem.PesVision
{
    public class PointProcessor
    {
        #region 数据结构定义
        public class Point : IEquatable<Point>
        {
            public double X { get; }
            public double Y { get; }

            public Point(double x, double y)
            {
                X = x;
                Y = y;
            }

            public bool Equals(Point other) =>
                Math.Abs(X - other.X) < 1e-9 && Math.Abs(Y - other.Y) < 1e-9;

            public override string ToString() => $"({X:F4}, {Y:F4})";
        }

        public class FeaturePoints
        {
            public Point FirstUpConcave { get; }
            public Point FirstDownConcave { get; }
            public Point Inflection { get; }

            public FeaturePoints(Point up, Point down, Point inflection)
            {
                FirstUpConcave = up;
                FirstDownConcave = down;
                Inflection = inflection;
            }
        }
        #endregion

        #region 预处理模块
        /// <summary>
        /// 高斯滤波器（5点窗口）
        /// </summary>
        public static List<Point> GaussianFilter(List<Point> points, double sigma = 1.4)
        {
            double[] kernel = GenerateGaussianKernel(5, sigma);
            List<Point> filtered = new List<Point>();

            for (int i = 0; i < points.Count; i++)
            {
                double sumX = 0, sumY = 0, weightSum = 0;
                for (int j = -2; j <= 2; j++)
                {
                    // 修正后的边界控制
                    int idx = i + j;
                    idx = idx < 0 ? 0 : (idx >= points.Count ? points.Count - 1 : idx);

                    double weight = kernel[j + 2];
                    sumX += points[idx].X * weight;
                    sumY += points[idx].Y * weight;
                    weightSum += weight;
                }
                filtered.Add(new Point(sumX / weightSum, sumY / weightSum));
            }
            return filtered;
        }

        private static double[] GenerateGaussianKernel(int size, double sigma)
        {
            double[] kernel = new double[size];
            double sum = 0;
            int half = size / 2;

            for (int i = -half; i <= half; i++)
            {
                double val = Math.Exp(-(i * i) / (2 * sigma * sigma));
                kernel[i + half] = val;
                sum += val;
            }

            return kernel.Select(v => v / sum).ToArray();
        }

        /// <summary>
        /// 改进型道格拉斯-普克算法（迭代实现）
        /// </summary>
        public static List<Point> DouglasPeucker(List<Point> points, double epsilon)
        {
            if (points.Count < 3) return new List<Point>(points);

            Stack<Tuple<int, int>> stack = new Stack<Tuple<int, int>>();
            HashSet<int> keep = new HashSet<int> { 0, points.Count - 1 };
            stack.Push(Tuple.Create(0, points.Count - 1));

            while (stack.Count > 0)
            {
                var stackPop = stack.Pop();
                var start = stackPop.Item1;
                var end = stackPop.Item2;
                double maxDist = 0;
                int index = start;

                for (int i = start + 1; i < end; i++)
                {
                    double dist = PerpendicularDistance(points[i], points[start], points[end]);
                    if (dist > maxDist)
                    {
                        maxDist = dist;
                        index = i;
                    }
                }

                if (maxDist > epsilon)
                {
                    keep.Add(index);
                    stack.Push(Tuple.Create(start, index));
                    stack.Push(Tuple.Create(index, end));
                }
            }

            return points.Where((p, i) => keep.Contains(i))
                .OrderBy(p => p.X)
                .ToList();
        }

        private static double PerpendicularDistance(Point pt, Point lineStart, Point lineEnd)
        {
            double dx = lineEnd.X - lineStart.X;
            double dy = lineEnd.Y - lineStart.Y;

            if (dx == 0 && dy == 0)
                return Math.Sqrt(Math.Pow(pt.X - lineStart.X, 2) + Math.Pow(pt.Y - lineStart.Y, 2));

            double t = ((pt.X - lineStart.X) * dx + (pt.Y - lineStart.Y) * dy) / (dx * dx + dy * dy);
            t = Math.Max(0, Math.Min(1, t));

            double projX = lineStart.X + t * dx;
            double projY = lineStart.Y + t * dy;

            return Math.Sqrt(Math.Pow(pt.X - projX, 2) + Math.Pow(pt.Y - projY, 2));
        }

        /// <summary>
        /// 基于阈值的噪声过滤
        /// </summary>
        public static List<Point> ThresholdFilter(List<Point> points, double maxDelta = 0.1)
        {
            List<Point> filtered = new List<Point> { points[0] };
            for (int i = 1; i < points.Count; i++)
            {
                double dx = points[i].X - points[i - 1].X;
                double dy = points[i].Y - points[i - 1].Y;
                if (Math.Sqrt(dx * dx + dy * dy) <= maxDelta)
                {
                    filtered.Add(points[i]);
                }
            }
            return filtered;
        }
        #endregion

        #region 多项式拟合模块
        /// <summary>
        /// 5次多项式拟合（带数值稳定处理）
        /// </summary>
        public static double[] PolynomialFit(List<Point> points, int degree = 5)
        {
            // 数据归一化
            double meanX = points.Average(p => p.X);
            double scaleX = 1.0 / (points.Max(p => p.X) - points.Min(p => p.X));

            var A = Matrix<double>.Build.Dense(points.Count, degree + 1);
            var b = Vector<double>.Build.Dense(points.Count);

            for (int i = 0; i < points.Count; i++)
            {
                double x = (points[i].X - meanX) * scaleX;
                for (int j = 0; j <= degree; j++)
                {
                    A[i, j] = Math.Pow(x, degree - j);
                }
                b[i] = points[i].Y;
            }

            var coeff = A.TransposeThisAndMultiply(A).Inverse() * A.Transpose() * b;
            return coeff.Select(c => c * scaleX).ToArray(); // 反归一化
        }

        /// <summary>
        /// 多项式求导（返回系数数组）
        /// </summary>
        public static double[] PolynomialDerivative(double[] coeff)
        {
            int degree = coeff.Length - 1;
            return coeff.Take(degree)
                .Select((c, i) => c * (degree - i))
                .ToArray();
        }
        #endregion

        #region 特征点检测
        /// <summary>
        /// 查找关键特征点
        /// </summary>
        public static FeaturePoints FindFeaturePoints(double[] originalCoeff)
        {
            var firstDeriv = PolynomialDerivative(originalCoeff);
            var secondDeriv = PolynomialDerivative(firstDeriv);

            // 求解二阶导数极值点
            var criticalPoints = FindRealRoots(PolynomialDerivative(secondDeriv));
            var candidates = criticalPoints.Select(x => new Point(x, EvaluatePolynomial(secondDeriv, x)))
                .OrderBy(p => p.X)
                .ToList();

            // 寻找第一个上凹点（二阶导数极大值）
            Point upConcave = candidates.OrderByDescending(p => p.Y).First();

            // 寻找后续下凹点（二阶导数极小值）
            Point downConcave = candidates.Where(p => p.X > upConcave.X)
                .OrderBy(p => p.Y)
                .FirstOrDefault();

            // 寻找拐点（二阶导数零点）
            double inflectionX = FindRoot(
                x => EvaluatePolynomial(secondDeriv, x),
                upConcave.X,
                downConcave.X
            );

            return new FeaturePoints(
                upConcave,
                downConcave,
                new Point(inflectionX, EvaluatePolynomial(originalCoeff, inflectionX))
            );
        }

        private static double EvaluatePolynomial(double[] coeff, double x)
        {
            double result = 0;
            int degree = coeff.Length - 1;
            for (int i = 0; i < coeff.Length; i++)
            {
                result += coeff[i] * Math.Pow(x, degree - i);
            }
            return result;
        }

        /// <summary>
        /// 求解多项式实根（使用伴矩阵特征值法）
        /// </summary>
        /// <param name="coefficients">多项式系数，从高次到低次排列</param>
        /// <returns>实根列表（已排序）</returns>
        public static List<double> FindRealRoots(double[] coefficients)
        {
            // 去除高次零系数
            int degree = coefficients.Length - 1;
            while (degree > 0 && Math.Abs(coefficients[coefficients.Length - degree - 1]) < 1e-10)
                degree--;

            if (degree == 0)
                return new List<double>();

            // 构建伴矩阵
            var companionMatrix = Matrix<double>.Build.Dense(degree, degree, 0);
            double leading = coefficients[coefficients.Length - degree - 1];

            for (int i = 0; i < degree - 1; i++)
                companionMatrix[i + 1, i] = 1;

            for (int i = 0; i < degree; i++)
                companionMatrix[0, i] = -coefficients[coefficients.Length - degree + i] / leading;

            // 计算特征值
            var evd = companionMatrix.Evd();
            var roots = evd.EigenValues.ToArray();

            // 筛选并处理实根
            return roots.Where(r => Math.Abs(r.Imaginary) < 1e-8)
                .Select(r => r.Real)
                .OrderBy(x => x)
                .ToList();
        }

        /// <summary>
        /// 在指定区间内寻找函数零点（改进版牛顿法）
        /// </summary>
        /// <param name="func">目标函数</param>
        /// <param name="lower">区间下限</param>
        /// <param name="upper">区间上限</param>
        /// <param name="maxIterations">最大迭代次数</param>
        /// <param name="tolerance">收敛容差</param>
        /// <returns>找到的根</returns>
        public static double FindRoot(Func<double, double> func,
            double lower,
            double upper,
            int maxIterations = 100,
            double tolerance = 1e-8)
        {
            // 初始猜测值
            double x0 = (lower + upper) / 2;
            double f0 = func(x0);

            // 符号检查
            if (Math.Abs(f0) < tolerance)
                return x0;

            // 牛顿迭代参数
            double h = 0.001;
            for (int i = 0; i < maxIterations; i++)
            {
                // 数值微分（中心差分法）
                double df = (func(x0 + h) - func(x0 - h)) / (2 * h);

                // 防止除零
                if (Math.Abs(df) < 1e-12)
                    df = df < 0 ? -1e-12 : 1e-12;

                double x1 = x0 - f0 / df;

                // 区间约束
                if (x1 < lower || x1 > upper)
                    x1 = (x0 + (f0 > 0 ? lower : upper)) / 2;

                double f1 = func(x1);

                // 收敛判断
                if (Math.Abs(f1) < tolerance)
                    return x1;

                // 更新迭代参数
                x0 = x1;
                f0 = f1;
            }

            throw new InvalidOperationException($"未能在{maxIterations}次迭代内收敛");
        }
        #endregion

        #region 完整处理流程
        public static FeaturePoints ProcessData(List<Point> rawPoints)
        {
            // 预处理流程
            var filtered = GaussianFilter(rawPoints);
            //filtered = ThresholdFilter(filtered);
            filtered = DouglasPeucker(filtered, 0.01);

            // 多项式拟合
            var coeff = PolynomialFit(filtered);

            // 特征点检测
            return FindFeaturePoints(coeff);
        }
        #endregion
    }
}


//using System;
//using System.Collections.Generic;
//using System.Linq;

//namespace CheckSystem.PesVision
//{
//    public class PolynomialAnalyzer
//    {
//        // 核心拟合方法（含异常处理）
//        public static double[] FitFifthOrder(IEnumerable<Tuple<double, double>> dataPoints, int maxRetry = 3)
//        {
//            // 参数验证
//            if (dataPoints == null || !dataPoints.Any())
//                throw new ArgumentException("数据点不能为空");

//            var points = dataPoints.ToList();
//            if (points.Count < 6)
//                throw new ArgumentException("至少需要6个数据点进行五次多项式拟合");

//            // 矩阵构建
//            double[,] designMatrix = CreateDesignMatrix(points);
//            double[] yVector = points.Select(p => p.Item2).ToArray();

//            // 鲁棒性矩阵运算
//            for (int attempt = 0; attempt < maxRetry; attempt++)
//            {


//                try
//                {
//                    double[,] xtx = MatrixMultiply(MatrixTranspose(designMatrix), designMatrix);
//                    double[,] xtxInverse = MatrixInverse(xtx);
//                    double[] xty = MatrixVectorMultiply(MatrixTranspose(designMatrix), yVector);

//                    return MatrixVectorMultiply(xtxInverse, xty);
//                }
//                catch (InvalidOperationException ex) when (attempt < maxRetry - 1)
//                {
//                    // 处理病态矩阵：添加正则化项
//                    double lambda = 1e-5 * Math.Pow(10, attempt);
//                    // xtx = AddRegularization(xtx, lambda);
//                }
//            }
//            throw new InvalidOperationException("无法稳定求解系数矩阵");
//        }

//        #region 矩阵运算核心
//        private static double[,] CreateDesignMatrix(List<Tuple<double, double>> points)
//        {
//            int n = points.Count;
//            var matrix = new double[n, 6];
//            for (int i = 0; i < n; i++)
//            {
//                double x = points[i].Item1;
//                for (int j = 0; j < 6; j++)
//                {
//                    matrix[i, j] = Math.Pow(x, j);
//                }
//            }
//            return matrix;
//        }

//        private static double[,] MatrixTranspose(double[,] matrix)
//        {
//            int rows = matrix.GetLength(0);
//            int cols = matrix.GetLength(1);
//            var result = new double[cols, rows];
//            for (int i = 0; i < rows; i++)
//                for (int j = 0; j < cols; j++)
//                    result[j, i] = matrix[i, j];
//            return result;
//        }

//        private static double[,] MatrixMultiply(double[,] a, double[,] b)
//        {
//            int aRows = a.GetLength(0);
//            int aCols = a.GetLength(1);
//            int bCols = b.GetLength(1);
//            var result = new double[aRows, bCols];

//            for (int i = 0; i < aRows; i++)
//                for (int j = 0; j < bCols; j++)
//                    for (int k = 0; k < aCols; k++)
//                        result[i, j] += a[i, k] * b[k, j];
//            return result;
//        }

//        private static double[] MatrixVectorMultiply(double[,] matrix, double[] vector)
//        {
//            int rows = matrix.GetLength(0);
//            int cols = matrix.GetLength(1);
//            var result = new double[rows];
//            for (int i = 0; i < rows; i++)
//                for (int j = 0; j < cols; j++)
//                    result[i] += matrix[i, j] * vector[j];
//            return result;
//        }

//        private static double[,] MatrixInverse(double[,] matrix)
//        {
//            int n = matrix.GetLength(0);
//            double[,] augmented = AugmentMatrix(matrix);

//            // 高斯-约旦消元
//            for (int i = 0; i < n; i++)
//            {
//                // 寻找主元
//                int pivot = i;
//                for (int j = i + 1; j < n; j++)
//                    if (Math.Abs(augmented[j, i]) > Math.Abs(augmented[pivot, i]))
//                        pivot = j;

//                if (Math.Abs(augmented[pivot, i]) < 1e-10)
//                    throw new InvalidOperationException("矩阵不可逆");

//                // 交换行
//                if (pivot != i)
//                    SwapRows(augmented, i, pivot);

//                // 归一化
//                double divisor = augmented[i, i];
//                for (int j = i; j < 2 * n; j++)
//                    augmented[i, j] /= divisor;

//                // 消元
//                for (int j = 0; j < n; j++)
//                {
//                    if (j != i && Math.Abs(augmented[j, i]) > 1e-10)
//                    {
//                        double factor = augmented[j, i];
//                        for (int k = i; k < 2 * n; k++)
//                            augmented[j, k] -= factor * augmented[i, k];
//                    }
//                }
//            }

//            // 提取逆矩阵
//            double[,] inverse = new double[n, n];
//            for (int i = 0; i < n; i++)
//                for (int j = 0; j < n; j++)
//                    inverse[i, j] = augmented[i, j + n];
//            return inverse;
//        }
//        #endregion

//        #region 辅助方法
//        private static double[,] AugmentMatrix(double[,] matrix)
//        {
//            int n = matrix.GetLength(0);
//            double[,] augmented = new double[n, 2 * n];
//            for (int i = 0; i < n; i++)
//            {
//                for (int j = 0; j < n; j++)
//                    augmented[i, j] = matrix[i, j];
//                augmented[i, i + n] = 1.0;
//            }
//            return augmented;
//        }

//        private static void SwapRows(double[,] matrix, int row1, int row2)
//        {
//            int cols = matrix.GetLength(1);
//            for (int j = 0; j < cols; j++)
//            {
//                double temp = matrix[row1, j];
//                matrix[row1, j] = matrix[row2, j];
//                matrix[row2, j] = temp;
//            }
//        }

//        private static double[,] AddRegularization(double[,] xtx, double lambda)
//        {
//            int n = xtx.GetLength(0);
//            for (int i = 0; i < n; i++)
//                xtx[i, i] += lambda;
//            return xtx;
//        }
//        #endregion

//        #region 极值与拐点分析
//        // 查找极值点（一阶导数实根）
//        public static List<Tuple<double, double>> FindExtrema(double[] coefficients)
//        {
//            double[] firstDerivative = GetDerivativeCoefficients(coefficients);
//            return FindRealRoots(firstDerivative)
//                .Select(x => Tuple.Create(x, Evaluate(coefficients, x)))
//                .ToList();
//        }

//        // 查找拐点（二阶导数实根）
//        public static List<Tuple<double, double>> FindInflectionPoints(double[] coefficients)
//        {
//            double[] secondDerivative = GetDerivativeCoefficients(
//                GetDerivativeCoefficients(coefficients));
//            return FindRealRoots(secondDerivative)
//                .Select(x => Tuple.Create(x, Evaluate(coefficients, x)))
//                .ToList();
//        }
//        #endregion

//        #region 微分与求根核心
//        // 获取导数系数（自动降阶）
//        private static double[] GetDerivativeCoefficients(double[] coeff)
//        {
//            if (coeff.Length < 2) return new double[0];
//            return coeff.Skip(1)
//                .Select((c, i) => c * (i + 1))
//                .ToArray();
//        }

//        // 多项式求根（支持任意次数）
//        private static List<double> FindRealRoots(double[] coefficients,
//            double tolerance = 1e-8,
//            int maxIterations = 100)
//        {
//            coefficients = RemoveLeadingZeros(coefficients);
//            if (coefficients.Length == 0) return new List<double>();
//            if (coefficients.Length == 1) return new List<double>();

//            // 使用自适应初始猜测的牛顿法
//            return NewtonSolver.FindAllRoots(coefficients, tolerance, maxIterations);
//        }

//        // 清理前导零系数
//        private static double[] RemoveLeadingZeros(double[] coeff)
//        {
//            int start = 0;
//            while (start < coeff.Length && Math.Abs(coeff[start]) < 1e-15)
//                start++;
//            return coeff.Skip(start).ToArray();
//        }
//        #endregion

//        #region 牛顿法求解器
//        private static class NewtonSolver
//        {
//            // 多阶段搜索策略
//            public static List<double> FindAllRoots(double[] coeff,
//                double tolerance,
//                int maxIterations)
//            {
//                var roots = new HashSet<double>();
//                int degree = coeff.Length - 1;

//                // 阶段1：尝试有理数初始猜测
//                foreach (var guess in GenerateRationalGuesses(coeff))
//                    TryFindRoot(coeff, guess, roots, tolerance, maxIterations);

//                // 阶段2：随机采样搜索
//                for (int i = 0; i < 10 * degree; i++)
//                    TryFindRoot(coeff, RandomGuess(coeff), roots, tolerance, maxIterations);

//                return roots.OrderBy(x => x).ToList();
//            }

//            // 生成有理数候选根（根据有理根定理）
//            private static IEnumerable<double> GenerateRationalGuesses(double[] coeff)
//            {
//                if (coeff.Last() == 0) yield break;

//                double pMax = Math.Abs(coeff[0]);
//                double qMax = Math.Abs(coeff.Last());
//                for (double p = 1; p <= pMax; p++)
//                    for (double q = 1; q <= qMax; q++)
//                        if (p % q == 0)
//                        {
//                            yield return p / q;
//                            yield return -p / q;
//                        }
//            }

//            // 随机猜测生成策略
//            private static double RandomGuess(double[] coeff)
//            {
//                double bound = Math.Max(1, coeff.Take(5).Max(c => Math.Abs(c)));
//                return (2 * new Random().NextDouble() - 1) * bound;
//            }

//            // 单个根的迭代求解
//            private static void TryFindRoot(double[] coeff,
//                double initialGuess,
//                HashSet<double> roots,
//                double tolerance,
//                int maxIterations)
//            {
//                double x = initialGuess;
//                for (int i = 0; i < maxIterations; i++)
//                {
//                    double fx = Evaluate(coeff, x);
//                    double dfx = Evaluate(GetDerivativeCoefficients(coeff), x);

//                    if (Math.Abs(fx) < tolerance)
//                    {
//                        double rounded = Math.Round(x / tolerance) * tolerance;
//                        if (!roots.Any(r => Math.Abs(r - rounded) < 10 * tolerance))
//                            roots.Add(rounded);
//                        return;
//                    }

//                    if (Math.Abs(dfx) < 1e-15) break;
//                    x -= fx / dfx;
//                }
//            }
//        }
//        #endregion

//        #region 多项式求值
//        public static double Evaluate(double[] coefficients, double x)
//        {
//            double result = 0;
//            for (int i = 0; i < coefficients.Length; i++)
//                result += coefficients[i] * Math.Pow(x, i);
//            return result;
//        }
//        #endregion

//        #region 数据预处理模块
//        // 高斯滤波（处理边界镜像填充）
//        public static List<Tuple<double, double>> ApplyGaussianFilter(
//            List<Tuple<double, double>> data,
//            int windowSize = 5,
//            double sigma = 1.0)
//        {
//            double[] kernel = GenerateGaussianKernel(windowSize, sigma);
//            int pad = windowSize / 2;
//            var paddedData = PadDataMirror(data, pad);
//            var smoothed = new List<Tuple<double, double>>();

//            for (int i = pad; i < paddedData.Count - pad; i++)
//            {
//                double sumX = 0, sumY = 0, weightSum = 0;
//                for (int j = -pad; j <= pad; j++)
//                {
//                    double w = kernel[j + pad];
//                    sumX += paddedData[i + j].Item1 * w;
//                    sumY += paddedData[i + j].Item2 * w;
//                    weightSum += w;
//                }
//                smoothed.Add(Tuple.Create(sumX / weightSum, sumY / weightSum));
//            }
//            return smoothed;
//        }

//        // 生成高斯卷积核
//        private static double[] GenerateGaussianKernel(int size, double sigma)
//        {
//            double[] kernel = new double[size];
//            double sum = 0;
//            int mid = size / 2;
//            for (int i = 0; i < size; i++)
//            {
//                kernel[i] = Math.Exp(-0.5 * Math.Pow((i - mid) / sigma, 2));
//                sum += kernel[i];
//            }
//            return kernel.Select(k => k / sum).ToArray();
//        }

//        // 镜像填充边界处理
//        private static List<Tuple<double, double>> PadDataMirror(
//            List<Tuple<double, double>> data, int padLength)
//        {
//            var padded = new List<Tuple<double, double>>();
//            for (int i = padLength; i > 0; i--)
//                padded.Add(data[Math.Min(i, data.Count - 1)]);
//            padded.AddRange(data);
//            for (int i = 2; i <= padLength + 1; i++)
//                padded.Add(data[Math.Max(data.Count - i, 0)]);
//            return padded;
//        }

//        // 道格拉斯-普克降采样
//        public static List<Tuple<double, double>> DouglasPeuckerReduction(
//            List<Tuple<double, double>> points,
//            double epsilon = 0.01)
//        {
//            if (points.Count < 3) return points;

//            int index = 0;
//            double dmax = 0;
//            int last = points.Count - 1;

//            // 寻找最大偏移点
//            for (int i = 1; i < last; i++)
//            {
//                double d = PerpendicularDistance(
//                    points[i],
//                    points[0],
//                    points[last]);
//                if (d > dmax)
//                {
//                    index = i;
//                    dmax = d;
//                }
//            }

//            List<Tuple<double, double>> result = new List<Tuple<double, double>>();
//            if (dmax > epsilon)
//            {
//                var left = DouglasPeuckerReduction(
//                    points.GetRange(0, index + 1), epsilon);
//                var right = DouglasPeuckerReduction(
//                    points.GetRange(index, last - index + 1), epsilon);

//                result.AddRange(left.Take(left.Count - 1));
//                result.AddRange(right);
//            }
//            else
//            {
//                result.Add(points.First());
//                result.Add(points.Last());
//            }
//            return result.Distinct().ToList();
//        }

//        // 计算点到直线的垂直距离
//        private static double PerpendicularDistance(
//            Tuple<double, double> point,
//            Tuple<double, double> lineStart,
//            Tuple<double, double> lineEnd)
//        {
//            double dx = lineEnd.Item1 - lineStart.Item1;
//            double dy = lineEnd.Item2 - lineStart.Item2;

//            // 处理垂直线段
//            if (Math.Abs(dx) < 1e-10 && Math.Abs(dy) < 1e-10)
//                return Math.Sqrt(
//                    Math.Pow(point.Item1 - lineStart.Item1, 2) +
//                    Math.Pow(point.Item2 - lineStart.Item2, 2));

//            double t = ((point.Item1 - lineStart.Item1) * dx +
//                       (point.Item2 - lineStart.Item2) * dy) /
//                       (dx * dx + dy * dy);
//            t = Math.Max(0, Math.Min(1, t));

//            double projX = lineStart.Item1 + t * dx;
//            double projY = lineStart.Item2 + t * dy;
//            return Math.Sqrt(
//                Math.Pow(point.Item1 - projX, 2) +
//                Math.Pow(point.Item2 - projY, 2));
//        }
//        #endregion

//    }
//}

//using System;
//using System.Collections.Concurrent;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace CheckSystem.PesVision
//{
//    public sealed class PolynomialAnalyzer
//    {
//        private readonly double[] _coefficients;
//        private readonly double[] _firstDerivCoeffs;
//        private readonly double[] _secondDerivCoeffs;
//        private readonly double[] _thirdDerivCoeffs;

//        public PolynomialAnalyzer(double[] coefficients)
//        {
//            if (coefficients == null || coefficients.Length != 6)
//                throw new ArgumentException("需要6个系数 (ax^5+bx^4+cx^3+dx^2+ex+f)");

//            _coefficients = coefficients.ToArray();
//            var derivatives = PrecomputeDerivatives();
//            _firstDerivCoeffs = derivatives.Item1;
//            _secondDerivCoeffs = derivatives.Item2;
//            _thirdDerivCoeffs = derivatives.Item3;
//        }

//        public struct Extremum
//        {
//            public readonly double X;
//            public readonly double Y;
//            public readonly bool IsMaximum;

//            public Extremum(double x, double y, bool isMax)
//            {
//                X = x;
//                Y = y;
//                IsMaximum = isMax;
//            }
//        }

//        public struct InflectionPoint
//        {
//            public readonly double X;
//            public readonly double Y;

//            public InflectionPoint(double x, double y)
//            {
//                X = x;
//                Y = y;
//            }
//        }

//        public class AnalysisResult
//        {
//            public List<Extremum> Extrema { get;  set; }
//            public List<InflectionPoint> Inflections { get;  set; }

//            public AnalysisResult()
//            {
//                Extrema = new List<Extremum>();
//                Inflections = new List<InflectionPoint>();
//            }
//        }

//        public AnalysisResult Analyze(double minX, double maxX, double precision = 1e-6)
//        {
//            var result = new AnalysisResult();

//            FindExtrema(result, minX, maxX, precision);
//            FindInflectionPoints(result, minX, maxX, precision);

//            result.Extrema = result.Extrema.OrderBy(e => e.X).ToList();
//            result.Inflections = result.Inflections.OrderBy(i => i.X).ToList();

//            return result;
//        }

//        private Tuple<double[], double[], double[]> PrecomputeDerivatives()
//        {
//            // 一阶导数系数: 5ax^4 + 4bx^3 + 3cx^2 + 2dx + e
//            var fd = new double[5];
//            fd[0] = 5 * _coefficients[0];
//            fd[1] = 4 * _coefficients[1];
//            fd[2] = 3 * _coefficients[2];
//            fd[3] = 2 * _coefficients[3];
//            fd[4] = _coefficients[4];

//            // 二阶导数系数: 20ax^3 + 12bx^2 + 6cx + 2d
//            var sd = new double[4];
//            sd[0] = 20 * _coefficients[0];
//            sd[1] = 12 * _coefficients[1];
//            sd[2] = 6 * _coefficients[2];
//            sd[3] = 2 * _coefficients[3];

//            // 三阶导数系数: 60ax^2 + 24bx + 6c
//            var td = new double[3];
//            td[0] = 60 * _coefficients[0];
//            td[1] = 24 * _coefficients[1];
//            td[2] = 6 * _coefficients[2];

//            return Tuple.Create(fd, sd, td);
//        }

//        private void FindExtrema(AnalysisResult result, double minX, double maxX, double precision)
//        {
//            var roots = FindAllRoots(_firstDerivCoeffs, minX, maxX, precision);
//            foreach (var x in roots)
//            {
//                var y = Evaluate(_coefficients, x);
//                var sdValue = Evaluate(_secondDerivCoeffs, x);

//                if (Math.Abs(sdValue) < precision) continue;

//                result.Extrema.Add(new Extremum(
//                    x: x,
//                    y: y,
//                    isMax: sdValue < 0
//                ));
//            }
//        }

//        private void FindInflectionPoints(AnalysisResult result, double minX, double maxX, double precision)
//        {
//            var roots = FindAllRoots(_secondDerivCoeffs, minX, maxX, precision);
//            foreach (var x in roots)
//            {
//                var tdValue = Evaluate(_thirdDerivCoeffs, x);
//                if (Math.Abs(tdValue) < precision) continue;

//                if (CheckInflectionValidity(x, precision))
//                {
//                    result.Inflections.Add(new InflectionPoint(
//                        x: x,
//                        y: Evaluate(_coefficients, x)
//                    ));
//                }
//            }
//        }

//        private bool CheckInflectionValidity(double x, double precision)
//        {
//            const double delta = 1e-5;
//            var left = Evaluate(_secondDerivCoeffs, x - delta);
//            var right = Evaluate(_secondDerivCoeffs, x + delta);
//            return Math.Sign(left) != Math.Sign(right);
//        }

//        private static List<double> FindAllRoots(double[] coefficients,
//            double minX, double maxX,
//            double precision = 1e-6)
//        {
//            var candidates = new ConcurrentBag<double>();
//            int sampleCount = Math.Max(50, (int)((maxX - minX) / precision));

//            Parallel.For(0, sampleCount, () => 0, (i, state, local) =>
//            {
//                double start = minX + i * (maxX - minX) / sampleCount;
//                double end = minX + (i + 1) * (maxX - minX) / sampleCount;

//                try
//                {
//                    var root = HybridRootFinder.FindRoot(
//                        x => Evaluate(coefficients, x),
//                        start, end, precision
//                    );
//                    if (!double.IsNaN(root))
//                        candidates.Add(root);
//                }
//                catch { /* 忽略收敛失败 */ }
//                return local;
//            }, local => { });

//            return candidates
//                .Where(x => x >= minX && x <= maxX)
//                .Distinct(new ToleranceComparer(precision * 10))
//                .OrderBy(x => x)
//                .ToList();
//        }

//        public static double Evaluate(double[] coefficients, double x)
//        {
//            double result = 0;
//            for (int i = 0; i < coefficients.Length; i++)
//            {
//                result = result * x + coefficients[i];
//            }
//            return result;
//        }
//    }

//    public static class HybridRootFinder
//    {
//        private const int MaxIterations = 100;

//        public static double FindRoot(Func<double, double> f,
//            double a, double b,
//            double tolerance = 1e-6)
//        {
//            double fa = f(a), fb = f(b);

//            if (Math.Abs(fa) < tolerance) return a;
//            if (Math.Abs(fb) < tolerance) return b;

//            if (Math.Sign(fa) == Math.Sign(fb))
//                return double.NaN;

//            double c = a, fc = fa, d = b - a, e = d;

//            for (int iter = 0; iter < MaxIterations; iter++)
//            {
//                if (Math.Abs(fc) < Math.Abs(fb))
//                {
//                    Swap(ref a, ref b);
//                    Swap(ref fa, ref fb);
//                }

//                double tol = 2 * tolerance * Math.Abs(b) + tolerance;
//                double m = 0.5 * (a - b);

//                if (Math.Abs(m) <= tol || fb == 0)
//                    return b;

//                if (Math.Abs(e) >= tol && Math.Abs(fa) > Math.Abs(fb))
//                {
//                    double p, q;
//                    ComputeDerivative(f, b, out p, out q);

//                    if (p != 0)
//                    {
//                        double delta = fb / p;
//                        if (2 * Math.Abs(delta) < Math.Abs(e))
//                        {
//                            e = d;
//                            d = delta;
//                            double t = b - delta;
//                            if (t >= a && t <= b)
//                            {
//                                b = t;
//                                fb = f(b);
//                                continue;
//                            }
//                        }
//                    }
//                }

//                d = m;
//                e = d;
//                double cPrev = b;
//                b += (a - b > 0) ? tol : -tol;
//                fb = f(b);
//            }
//            return double.NaN;
//        }

//        private static void ComputeDerivative(Func<double, double> f, double x,
//            out double firstDeriv, out double secondDeriv)
//        {
//            const double h = 1e-6;
//            double fx = f(x);
//            double fxh = f(x + h);
//            double fxhh = f(x + 2 * h);

//            firstDeriv = (-fxhh + 4 * fxh - 3 * fx) / (2 * h);
//            secondDeriv = (fxhh - 2 * fxh + fx) / (h * h);
//        }

//        private static void Swap<T>(ref T a, ref T b)
//        {
//            T temp = a;
//            a = b;
//            b = temp;
//        }
//    }

//    public class ToleranceComparer : IEqualityComparer<double>
//    {
//        private readonly double _tolerance;

//        public ToleranceComparer(double tolerance = 1e-6)
//        {
//            _tolerance = tolerance;
//        }

//        public bool Equals(double x, double y)
//        {
//            return Math.Abs(x - y) <= _tolerance;
//        }

//        public int GetHashCode(double obj)
//        {
//            return 0;
//        }
//    }
//}