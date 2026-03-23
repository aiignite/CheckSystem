# MdfHeadlampAnalysis 控制器接口文档

**命名空间**: `Controller`
**父类**: `ControllerBase`
**控制器描述**: MdfHeadlampAnalysis

### 接口列表

| 控制器名称 | 控制器描述 | 名称 | 类型 | 类别 | 参数 | 说明 | 示例 |
|-----------|-----------|------|------|------|------|------|------|
| MdfHeadlampAnalysis | R/W,当前使用相机SN号 | CameraSn | string | 字段 | - | 当前使用相机SN号 | - |
| MdfHeadlampAnalysis | R/W,当前使用相机的镜头大小-通常采用的4mm即4000um | Ratio | double | 字段 | - | 当前使用相机的镜头大小 | - |
| MdfHeadlampAnalysis | R/W,当前使用相机像元大小-通常采用的是2.2um | PixelScale | double | 字段 | - | 当前使用相机像元大小 | - |
| MdfHeadlampAnalysis | R/W,相机物距 | CameraDistanceCm | double | 字段 | - | 相机物距 | - |
| MdfHeadlampAnalysis | R/W,菲涅尔透镜物距 | FresnelLensDistanceCm | double | 字段 | - | 菲涅尔透镜物距 | - |
| MdfHeadlampAnalysis | R/W,截止线扫描X轴起始像素 | ScanStartX | int | 字段 | - | 截止线扫描X轴起始像素 | - |
| MdfHeadlampAnalysis | R/W,截止线扫描X轴结束像素 | ScanEndX | int | 字段 | - | 截止线扫描X轴结束像素 | - |
| MdfHeadlampAnalysis | R/W,最大检测区域H | ScanDegreeH | int | 字段 | - | 最大检测区域H | - |
| MdfHeadlampAnalysis | R/W,最大检测区域V | ScanDegreeV | int | 字段 | - | 最大检测区域V | - |
| MdfHeadlampAnalysis | 拐点像素X | InflectionPixelPointX | int | 字段 | - | 拐点像素X坐标 | - |
| MdfHeadlampAnalysis | 拐点像素Y | InflectionPixelPointY | int | 字段 | - | 拐点像素Y坐标 | - |
| MdfHeadlampAnalysis | HV点灰度值 | GrayHv | double | 字段 | - | HV点灰度值 | - |
| MdfHeadlampAnalysis | B50L点灰度值 | GrayB50L | double | 字段 | - | B50L点灰度值 | - |
| MdfHeadlampAnalysis | 75R点灰度值 | Gray75R | double | 字段 | - | 75R点灰度值 | - |
| MdfHeadlampAnalysis | 75L点灰度值 | Gray75L | double | 字段 | - | 75L点灰度值 | - |
| MdfHeadlampAnalysis | 50L点灰度值 | Gray50L | double | 字段 | - | 50L点灰度值 | - |
| MdfHeadlampAnalysis | 25L点灰度值 | Gray25L | double | 字段 | - | 25L点灰度值 | - |
| MdfHeadlampAnalysis | 50V点灰度值 | Gray50V | double | 字段 | - | 50V点灰度值 | - |
| MdfHeadlampAnalysis | 50R点灰度值 | Gray50R | double | 字段 | - | 50R点灰度值 | - |
| MdfHeadlampAnalysis | 25R点灰度值 | Gray25R | double | 字段 | - | 25R点灰度值 | - |
| MdfHeadlampAnalysis | EMax点H方向角度 | EMaxPointDegreeH | double | 字段 | - | EMax点H方向角度 | - |
| MdfHeadlampAnalysis | EMax点V方向角度 | EMaxPointDegreeV | double | 字段 | - | EMax点V方向角度 | - |
| MdfHeadlampAnalysis | EMax点灰度值 | GrayEMax | double | 字段 | - | EMax点灰度值 | - |
| MdfHeadlampAnalysis | P1点灰度值 | GrayP1 | double | 字段 | - | P1点灰度值 | - |
| MdfHeadlampAnalysis | P2点灰度值 | GrayP2 | double | 字段 | - | P2点灰度值 | - |
| MdfHeadlampAnalysis | P3点灰度值 | GrayP3 | double | 字段 | - | P3点灰度值 | - |
| MdfHeadlampAnalysis | P4点灰度值 | GrayP4 | double | 字段 | - | P4点灰度值 | - |
| MdfHeadlampAnalysis | P5点灰度值 | GrayP5 | double | 字段 | - | P5点灰度值 | - |
| MdfHeadlampAnalysis | P6点灰度值 | GrayP6 | double | 字段 | - | P6点灰度值 | - |
| MdfHeadlampAnalysis | P1P2P3灰度值之和 | GraySumP1P2P3 | double | 字段 | - | P1P2P3灰度值之和 | - |
| MdfHeadlampAnalysis | P4P5P6灰度值之和 | GraySumP4P5P6 | double | 字段 | - | P4P5P6灰度值之和 | - |
| MdfHeadlampAnalysis | P7点灰度值 | GrayP7 | double | 字段 | - | P7点灰度值 | - |
| MdfHeadlampAnalysis | P8点灰度值 | GrayP8 | double | 字段 | - | P8点灰度值 | - |
| MdfHeadlampAnalysis | 2.5L梯度 | Grad2d5L | double | 字段 | - | 2.5L梯度 | - |
| MdfHeadlampAnalysis | 分析结果 | AnalysisResult | string | 字段 | - | 分析结果 | - |
| MdfHeadlampAnalysis | 分析结果Base64字符串 | AnalysisResultBase64String | string | 字段 | - | 分析结果Base64字符串 | - |
| MdfHeadlampAnalysis | MDF H方向角度 | MdfHDegree | double | 字段 | - | MDF H方向角度 | - |
| MdfHeadlampAnalysis | 采集图像 | CaptureImage | void | 方法 | string name, uint exposeTime, int captureCount | 采集图像 | CaptureImage("img1", 10000, 5) |
| MdfHeadlampAnalysis | 读取图像 | ReadImage | void | 方法 | string name, string filePath | 从文件读取图像 | ReadImage("img1", "C:\\test.bmp") |
| MdfHeadlampAnalysis | 清除所有图像 | ClearAllImage | void | 方法 | - | 清除所有图像 | ClearAllImage() |
| MdfHeadlampAnalysis | 添加图像 | AppendImage | void | 方法 | string name, Mat mat | 添加图像到缓存 | AppendImage("img1", mat) |
| MdfHeadlampAnalysis | 根据°计算像素 | DegreeToPixel | int | 方法 | double degree | 根据角度计算像素值 | DegreeToPixel(0.5) |
| MdfHeadlampAnalysis | 分析图像 | AnalyzeImage | void | 方法 | string srcName, string mdfSrcName | 分析图像 | AnalyzeImage("src", "mdf") |
| MdfHeadlampAnalysis | 清除分析结果 | ClearAnalysisResult | void | 方法 | - | 清除分析结果 | ClearAnalysisResult() |
