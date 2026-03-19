using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckSystem.PesVision
{
    public class CurveProcessorDeepSeek
    {
        public class Point
        {
            public double X { get; set; }
            public double Y { get; set; }
            public Point(double x, double y) { X = x; Y = y; }
        }

        public class AnalysisResult
        {
            public double X { get; set; }
            public double Y { get; set; }
            public string Type { get; set; } // 上凹点、下凹点、拐点
        }

        // 预处理：剔除连续三列不单调的中间点
        public List<Point> Preprocess(List<Point> points)
        {
            var sorted = points.OrderBy(p => p.X).ToList();
            var removeIndices = new HashSet<int>();

            for (int i = 0; i < sorted.Count - 2; i++)
            {
                double y0 = sorted[i].Y, y1 = sorted[i + 1].Y, y2 = sorted[i + 2].Y;
                bool monotonic = (y1 >= y0 && y2 >= y1) || (y1 <= y0 && y2 <= y1);
                if (!monotonic) removeIndices.Add(i + 1);
            }

            return sorted.Where((p, i) => !removeIndices.Contains(i)).ToList();
        }

        // 多项式拟合
        public Polynomial FitPolynomial(List<Point> points, int order, double lambda = 0.1)
        {
            double[] x = points.Select(p => p.X).ToArray();
            double[] y = points.Select(p => p.Y).ToArray();
            return Polynomial.Fit(x, y, order);

            //var x = points.Select(p => (double)p.X).ToArray();
            //var y = points.Select(p => (double)p.Y).ToArray();

            //// 构建设计矩阵
            //var design = Matrix<double>.Build.DenseOfRowArrays(
            //    x.Select(xi => Enumerable.Range(0, order + 1)
            //        .Select(j => Math.Pow(xi, j)).ToArray()));

            //// 添加正则化项
            //var regularization = Matrix<double>.Build.DiagonalIdentity(order + 1) * lambda;
            //var coefficients = (design.Transpose() * design + regularization).Inverse() * design.Transpose() * Vector<double>.Build.Dense(y);

            //return coefficients.ToArray();
        }

        public double[] FirstDerivative(double[] coefficients)
        {
            return Enumerable.Range(1, coefficients.Length - 1)
                .Select(i => coefficients[i] * i)
                .ToArray();
        }

        public double[] SecondDerivative(double[] coefficients)
        {
            return Enumerable.Range(2, coefficients.Length - 2)
                .Select(i => coefficients[i] * i * (i - 1))
                .ToArray();
        }

        public double? FindInflectionPoint(double[] secondDerivCoeffs)
        {
            if (secondDerivCoeffs.Length < 2) return null;

            //// 处理不同阶数情况
            //return secondDerivCoeffs.Length switch
            //{
            //    1 => null, // 常数函数无拐点
            //    2 => -secondDerivCoeffs[0] / secondDerivCoeffs[1], // 线性函数
            //    _ => SolveQuadraticExtremum(secondDerivCoeffs) // 二次及以上
            //};

            return null;
        }

        // 多项式求导
        public Polynomial Derivative(Polynomial poly)
        {
            var coeff = poly.Coefficients.Select((c, i) => i * c).Skip(1).ToArray();
            return new Polynomial(coeff);
        }

        // 查找关键点
        public List<AnalysisResult> FindCriticalPoints(Polynomial poly, Polynomial secondDeriv)
        {
            var results = new List<AnalysisResult>();

            // 找二次导数的根（拐点）
            foreach (var root in FindRealRoots(secondDeriv))
            {
                // 检查凹凸性变化
                double epsilon = 1e-5;
                double left = secondDeriv.Evaluate(root - epsilon);
                double right = secondDeriv.Evaluate(root + epsilon);

                if (Math.Sign(left) != Math.Sign(right))
                {
                    results.Add(new AnalysisResult
                    {
                        X = root,
                        Y = poly.Evaluate(root),
                        Type = "拐点"
                    });
                }
            }

            // 找一次导数的根（极值点）
            var firstDeriv = Derivative(poly);
            foreach (var root in FindRealRoots(firstDeriv))
            {
                double secondDerivVal = secondDeriv.Evaluate(root);
                results.Add(new AnalysisResult
                {
                    X = root,
                    Y = poly.Evaluate(root),
                    Type = secondDerivVal > 0 ? "上凹点" :
                           secondDerivVal < 0 ? "下凹点" : "未知"
                });
            }

            return results;
        }

        // 辅助方法：找实数根
        private IEnumerable<double> FindRealRoots(Polynomial poly)
        {
            return poly.Roots()
                .Where(r => r.Imaginary == 0)
                .Select(r => r.Real)
                .Where(r => !double.IsNaN(r));
        }

        #region 查找顶点

        // 4. 查找二次导数顶点
        public double? FindSecondDerivativeVertex(double[] secondDerivCoeffs)
        {
            if (secondDerivCoeffs.Length < 3) return null;

            double a = secondDerivCoeffs[2];  // x²系数
            double b = secondDerivCoeffs[1];  // x项系数

            if (Math.Abs(a) < double.Epsilon) return null;
            return -b / (2 * a);
        }

        #endregion
    }
}
