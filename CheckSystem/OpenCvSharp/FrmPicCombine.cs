using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using OpenCvSharp.XFeatures2D;
using Size = OpenCvSharp.Size;

namespace CheckSystem.OpenCvSharp
{
    public partial class FrmPicCombine : Form
    {
        public FrmPicCombine()
        {
            InitializeComponent();
            //PicCombine();
        }

        private void btnSelectImg1_Click(object sender, EventArgs e)
        {
            var openFi = new OpenFileDialog();
            openFi.Filter = "图像文件(JPeg, Gif, Bmp, etc.)|*.jpg;*.jpeg;*.gif;*.bmp;*.tif; *.tiff; *.png| JPeg 图像文件(*.jpg;*.jpeg)"
                            + "|*.jpg;*.jpeg |GIF 图像文件(*.gif)|*.gif |BMP图像文件(*.bmp)|*.bmp|Tiff图像文件(*.tif;*.tiff)|*.tif;*.tiff|Png图像文件(*.png)"
                            + "| *.png |所有文件(*.*)|*.*";
            if (openFi.ShowDialog() == DialogResult.OK)
            {
                txtImg1.Text = openFi.FileName;
                pbImg1.BackgroundImage = Image.FromFile(openFi.FileName);
            }
        }

        private void btnSelectImg2_Click(object sender, EventArgs e)
        {
            var openFi = new OpenFileDialog();
            openFi.Filter = "图像文件(JPeg, Gif, Bmp, etc.)|*.jpg;*.jpeg;*.gif;*.bmp;*.tif; *.tiff; *.png| JPeg 图像文件(*.jpg;*.jpeg)"
                            + "|*.jpg;*.jpeg |GIF 图像文件(*.gif)|*.gif |BMP图像文件(*.bmp)|*.bmp|Tiff图像文件(*.tif;*.tiff)|*.tif;*.tiff|Png图像文件(*.png)"
                            + "| *.png |所有文件(*.*)|*.*";
            if (openFi.ShowDialog() == DialogResult.OK)
            {
                txtImg2.Text = openFi.FileName;
                pbImg2.BackgroundImage = Image.FromFile(openFi.FileName);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var strImg1 = txtImg1.Text.Trim();
            var strImg2 = txtImg2.Text.Trim();
            if (string.IsNullOrEmpty(strImg1))
            {
                MessageBox.Show(@"请选择图片1");
                return;
            }
            if (string.IsNullOrEmpty(strImg2))
            {
                MessageBox.Show(@"请选择图片2");
                return;
            }
            var image1 = Image.FromFile(strImg1);
            var image2 = Image.FromFile(strImg2);

            pbResult.BackgroundImage = null;

            var srcImg1 = new Mat(strImg1);
            var srcImg2 = new Mat(strImg2);

            {
                var ret = PicCombine(srcImg1, srcImg2);
                pbResult.BackgroundImage = ret.ToBitmap();
            }

            {
                // 准备实际物体的三维点（如棋盘格角点的世界坐标）
                var objp = new List<Point3f>();
                for (int i = 0; i < 6; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        objp.Add(new Point3f(j, i, 0));
                    }
                }

                var objpoints = new List<Point3f[]>(); // 3D 点
                var imgpoints = new List<Point2f[]>(); // 图像平面上的点

                // 读取目录下的所有图片
                var images = Directory.GetFiles(@"C:\Users\B1438\Pictures\Camera Roll\拼接测试\测试02", "*.jpg");

                var index = 0;

                foreach (var fname in images)
                {
                    using (var img = new Mat(fname))
                    using (var gray = new Mat())
                    {
                        Cv2.CvtColor(img, gray, ColorConversionCodes.BGR2GRAY);

                        // 找到棋盘格角点
                        Point2f[] corners;
                        var ret = Cv2.FindChessboardCorners(gray, new Size(9, 6), out corners);

                        // 如果找到足够的角点，将它们添加到 imgpoints 中
                        if (ret)
                        {
                            objpoints.Add(objp.ToArray());
                            imgpoints.Add(corners);

                            // 可选：可视化角点
                            Cv2.DrawChessboardCorners(img, new Size(9, 6), corners, ret);
                            Cv2.ImShow("img" + index, img);
                            index++;
                            Cv2.WaitKey(500);
                        }
                    }
                }

                //Cv2.DestroyAllWindows();

                // 使用所有的对象点和图像点进行标定
                //Cv2.CalibrateCamera(objpoints, imgpoints, new Size(640, 480), out Mat cameraMatrix, out Mat distCoeffs, out _, out _);

                //Cv2.CalibrateCamera()
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (pbResult.BackgroundImage == null)
            {
                MessageBox.Show(@"结果图为空");
                return;
            }

            var savedialog = new SaveFileDialog();
            savedialog.Filter = @"Jpg 图片|*.jpg|Bmp 图片|*.bmp|Png 图片|*.png";
            savedialog.FilterIndex = 0;
            savedialog.RestoreDirectory = true;//保存对话框是否记忆上次打开的目录
            savedialog.CheckPathExists = true;//检查目录
            savedialog.FileName = System.DateTime.Now.ToString("yyyyMMddHHmmss") + "-"; ;//设置默认文件名
            if (savedialog.ShowDialog() == DialogResult.OK)
            {
                pbResult.BackgroundImage.Save(savedialog.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);// image为要保存的图片
                MessageBox.Show(this, "图片保存成功！", "信息提示");
            }
        }

        private Mat PicCombine(Mat src1, Mat src2)
        {
            //Mat src1 = Cv2.ImRead(@"C:\Users\B1438\Pictures\Camera Roll\测试拼接2\1.png");
            //Mat src2 = Cv2.ImRead(@"C:\Users\B1438\Pictures\Camera Roll\测试拼接2\2.png");

            //Mat src1 = Cv2.ImRead(@"C:\Users\B1438\Pictures\Camera Roll\拼接测试\left_part.jpg");
            //Mat src2 = Cv2.ImRead(@"C:\Users\B1438\Pictures\Camera Roll\拼接测试\right_part.jpg");

            var surf = SURF.Create(2000, upright: true);
            KeyPoint[] kp1, kp2;
            Mat dst1 = new Mat(), dst2 = new Mat();
            surf.DetectAndCompute(src1, null, out kp1, dst1);
            surf.DetectAndCompute(src2, null, out kp2, dst2);

            var flann = new FlannBasedMatcher();
            var dm = flann.KnnMatch(dst1, dst2, 2);

            var dmWhere = dm.Where(x => x[0].Distance < 0.7 * x[1].Distance).ToArray();
            var goodPoints1 = dmWhere.Select(i => kp1[i[0].QueryIdx].Pt).ToArray();
            var goodPoints2 = dmWhere.Select(i => kp2[i[0].TrainIdx].Pt).ToArray();

            //List<Point2f> goodPoints3 = new List<Point2f>();
            //List<Point2f> goodPoints4 = new List<Point2f>();
            //for (int i = 0; i < dm.Length; i++)
            //{
            //    if (dm[i][0].Distance < 0.7 * dm[i][1].Distance)
            //    {
            //        goodPoints3.Add(kp1[dm[i][0].QueryIdx].Pt);
            //        goodPoints4.Add(kp2[dm[i][0].TrainIdx].Pt);
            //    }
            //}

            var mask12 = new Mat();
            var homography12 = Cv2.FindHomography(
                InputArray.Create(goodPoints2.ToArray()),
                InputArray.Create(goodPoints1.ToArray()),
                HomographyMethods.Ransac, 5, mask12);

            var size = new Size(src1.Width + src2.Width, src1.Height);
            var result = new Mat(size, MatType.CV_8UC3);
            Cv2.WarpPerspective(src2, result, homography12, new Size(src1.Width + src2.Width, src1.Height));

            ImageFusion(src1, result, 100);

            src1.CopyTo(result[new Rect(0, 0, src1.Width, src1.Height)]);

            return result;
        }

        /// <summary>
        /// 图像交界处平滑处理
        /// </summary>
        /// <param name="src1"></param>
        /// <param name="src2"></param>
        /// <param name="width"></param>
        private void ImageFusion(Mat src1, Mat src2, int width)
        {
            float widthF = width;
            var height = src1.Height;
            var start = src1.Cols - width;
            var alpha = 0f;
            unsafe
            {
                for (var i = 0; i < height; i++)
                {
                    var ptr1 = (byte*)src1.Ptr(i);
                    var ptr2 = (byte*)src2.Ptr(i);
                    for (var j = start; j < src1.Cols; j++)
                    {
                        if (ptr2[j * 3] == 0 && ptr2[j * 3 + 1] == 0 && ptr2[j * 3 + 2] == 0)
                        {
                            alpha = 0f;
                        }
                        else
                        {
                            alpha = (j - start) / widthF;
                        }
                        ptr1[j * 3] = (byte)(ptr1[j * 3] * (1 - alpha) + ptr2[j * 3] * alpha);
                        ptr1[j * 3 + 1] = (byte)(ptr1[j * 3 + 1] * (1 - alpha) + ptr2[j * 3 + 1] * alpha);
                        ptr1[j * 3 + 2] = (byte)(ptr1[j * 3 + 2] * (1 - alpha) + ptr2[j * 3 + 2] * alpha);
                    }
                }
            }
        }

        /// <summary>
        /// 通过最小距离阈值来过滤部分匹配
        /// </summary>
        /// <param name="matches"></param>
        /// <returns></returns>
        private static List<DMatch> Match_min(DMatch[] matches)
        {
            var reList = new List<DMatch>();
            var minDist = 10000f;
            var maxDist = 0f;
            for (var i = 0; i < matches.Length; i++)
            {
                var dist = matches[i].Distance;
                minDist = dist < minDist ? dist : minDist;
                maxDist = dist > maxDist ? dist : maxDist;
            }
            for (var i = 0; i < matches.Length; i++)
            {
                if (matches[i].Distance <= Math.Max(2 * minDist, 20f))
                {
                    reList.Add(matches[i]);
                }
            }
            return reList;
        }

        // 定义静态变量，用于设置棋盘格的宽度和高度
        private static int BoardSize_Width = 9;
        private static int BoardSize_Height = 6;
        private static Size BoardSize = new Size(BoardSize_Width, BoardSize_Height);

        // 定义静态变量，用于设置每个方格的宽度
        private static int SquareSize = 29;
        private static int winSize = 11;

        private void button2_Click(object sender, EventArgs e)
        {
            // 存储图像文件路径
            List<string> imagesList = new List<string>()
            {
                @"C:\Users\B1438\Pictures\Camera Roll\拼接测试\测试02\1.jpg",
                @"C:\Users\B1438\Pictures\Camera Roll\拼接测试\测试02\2.jpg",
                @"C:\Users\B1438\Pictures\Camera Roll\拼接测试\测试02\3.jpg"
                //@"E:\Projects\PES远光\截止线.png"
            };

            // 存储每个图像的棋盘角点
            List<Point2f[]> imagesPoints = new List<Point2f[]>();

            // 相机内参矩阵和畸变系数
            Mat cameraMatrix = new Mat(), distCoeffs = new Mat();

            // 图像的尺寸
            Size imageSize = new Size();
            bool found = false;

            // 存储角点坐标的 Mat 数组
            Mat[] imagesPointsM = new Mat[imagesList.Count];

            var index = 0;

            // 遍历图像列表
            foreach (var imagePath in imagesList)
            {
                // 读取图像
                Mat view = new Mat(imagePath);

                if (!view.Empty())
                {
                    imageSize = view.Size();
                    Point2f[] pointBuf;

                    // 查找棋盘角点
                    found = Cv2.FindChessboardCorners(view, BoardSize, out pointBuf, ChessboardFlags.AdaptiveThresh | ChessboardFlags.NormalizeImage);

                    if (found)
                    {
                        // 灰度化
                        Mat viewGray = new Mat();
                        Cv2.CvtColor(view, viewGray, ColorConversionCodes.BGR2GRAY);

                        // 亚像素精确化
                        Cv2.CornerSubPix(viewGray, pointBuf, new Size(winSize, winSize), new Size(-1, -1), new TermCriteria(CriteriaType.Eps | CriteriaType.Count, 30, 0.0001));

                        // 存储角点坐标
                        imagesPoints.Add(pointBuf);

                        Mat p = new Mat(pointBuf.Length, 1, MatType.CV_32FC2);
                        for (int i = 0; i < pointBuf.Length; i++)
                        {
                            p.Set(i, 0, pointBuf[i]);
                        }

                        //Mat p = Mat.FromArray<Point2f>(pointBuf);
                        imagesPointsM[imagesList.IndexOf(imagePath)] = p;

                        // 在图像上绘制角点
                        Cv2.DrawChessboardCorners(view, BoardSize, pointBuf, found);
                        Mat temp = view.Clone();
                        Cv2.ImShow("Image View" + index, view);
                        index++;
                        Cv2.WaitKey(500);
                    }
                }
            }

            Mat[] rvecs = new Mat[0];
            Mat[] tvecs = new Mat[0];

            // 运行相机标定
            double totalAvgErr;
            RunCalibration(imagesList.Count, imageSize, out cameraMatrix, out distCoeffs, imagesPointsM, out rvecs, out tvecs, out totalAvgErr);

            // 打印相机矩阵、畸变系数和平均误差
            Console.WriteLine("相机矩阵：\n{0}", Cv2.Format(cameraMatrix) + "\n");
            Console.WriteLine("畸变系数:\n{0}", Cv2.Format(distCoeffs) + "\n");
            Console.WriteLine("平均误差:\n{0}", totalAvgErr + "\n");

            // 畸变校正
            Mat map1 = new Mat();
            Mat map2 = new Mat();
            Rect roi;
            Mat newCameraMatrix = Cv2.GetOptimalNewCameraMatrix(cameraMatrix, distCoeffs, imageSize, 1, imageSize, out roi);
            Cv2.InitUndistortRectifyMap(cameraMatrix, distCoeffs, new Mat(), newCameraMatrix, imageSize, MatType.CV_16SC2, map1, map2);

            // 遍历图像并显示校正后的图像
            foreach (var imagePath in imagesList)
            {
                Mat view = Cv2.ImRead(imagePath, ImreadModes.Color);
                Mat rview = new Mat();
                if (view.Empty())
                    continue;

                // 校正
                Cv2.Remap(view, rview, map1, map2, InterpolationFlags.Linear);
                Cv2.ImShow("Image RView" + index, rview);
                index++;
                Cv2.WaitKey(500);
            }

            Cv2.WaitKey();
        }

        // 运行相机标定
        private static void RunCalibration(int imagesCount, Size imageSize, out Mat cameraMatrix, out Mat distCoeffs, Mat[] imagePoints, out Mat[] rvecs, out Mat[] tvecs, out double totalAvgErr)
        {
            // 初始化相机矩阵和畸变系数
            cameraMatrix = Mat.Eye(new Size(3, 3), MatType.CV_64F);
            distCoeffs = Mat.Zeros(new Size(8, 1), MatType.CV_64F);

            // 计算棋盘角点的世界坐标
            Mat[] objectPoints = CalcBoardCornerPositions(BoardSize, SquareSize, imagesCount);

            // 进行相机标定
            double rms = Cv2.CalibrateCamera(objectPoints, imagePoints, imageSize, cameraMatrix, distCoeffs, out rvecs, out tvecs, CalibrationFlags.None);

            // 检查相机矩阵和畸变系数的范围
            bool ok = Cv2.CheckRange(InputArray.Create(cameraMatrix)) && Cv2.CheckRange(InputArray.Create(distCoeffs));

            // 计算重投影误差
            totalAvgErr = ComputeReprojectionErrors(objectPoints, imagePoints, rvecs, tvecs, cameraMatrix, distCoeffs);
        }

        // 计算棋盘角点的世界坐标
        private static Mat[] CalcBoardCornerPositions(Size BoardSize, float SquareSize, int imagesCount)
        {
            Mat[] corners = new Mat[imagesCount];
            // 遍历每张图片
            for (int k = 0; k < imagesCount; k++)
            {
                Point3f[] p = new Point3f[BoardSize.Height * BoardSize.Width];

                for (int i = 0; i < BoardSize.Height; i++)
                {
                    for (int j = 0; j < BoardSize.Width; j++)
                    {
                        // 计算每个格子的三维坐标并储存在一维数组 p 中
                        p[i * BoardSize.Width + j] = new Point3f(j * SquareSize, i * SquareSize, 0);
                    }
                }
                // 将三维坐标转换成 Mat 类型并存储再 corners 数组中

                corners[k] = new Mat(p.Length, 1, MatType.CV_32FC3);

                for (int i = 0; i < p.Length; i++)
                {
                    // 将每个 Point3f 对象设置到 Mat 中
                    corners[k].Set(i, 0, p[i]);
                }

                //corners[k] = Mat.FromArray<Point3f>(p);
            }
            return corners;
        }

        // 计算重投影误差
        private static double ComputeReprojectionErrors(Mat[] objectPoints, Mat[] imagePoints, Mat[] rvecs, Mat[] tvecs, Mat cameraMatrix, Mat distCoeffs)
        {
            Mat imagePoints2 = new Mat();
            int totalPoints = 0;
            double totalErr = 0, err;

            for (int i = 0; i < objectPoints.Length; ++i)
            {
                Cv2.ProjectPoints(objectPoints[i], rvecs[i], tvecs[i], cameraMatrix, distCoeffs, imagePoints2);

                err = Cv2.Norm(imagePoints[i], imagePoints2, NormTypes.L2);

                int n = objectPoints[i].Width * objectPoints[i].Height;
                totalErr += err * err;
                totalPoints += n;
            }

            return Math.Sqrt(totalErr / totalPoints);
        }
    }
}
