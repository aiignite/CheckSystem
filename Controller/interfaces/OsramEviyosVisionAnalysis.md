# OsramEviyosVisionAnalysis 控制器接口文档

## 控制器信息

| 控制器名称 | 控制器描述 |
|-----------|-----------|
| OsramEviyosVisionAnalysis |  |

## 公共字段

| 名称 | 类型 | 类别 | 描述 |
|------|------|------|------|
| UsingCameraSn | string | 读/写 | R/W, 摄像头序列号 |
| MaxCaptureCount | int | 读/写 | R/W, 最大拍摄张数 |
| VisionAnalysisResult | string | 读/写 | R/W, 图像分析结果 |
| VisionAnalysisBitmapBase64String | string | 读/写 | R/W, 图像数据 |
| StandStandLx | int | 读/写 | R/W, 标准亮度 |
| XyRatioMin | float | 读/写 | R/W, XY比例最小值 |
| XyRatioMax | float | 读/写 | R/W, XY比例最大值 |
| IsCv2ShowImage | bool | 读/写 | R/W, 是否显示图像 |
| StardandMin | int | 读/写 | 最小标准值 |
| StardandMax | int | 读/写 | 最大标准值 |
| BrightSpotBitmapBase64String | string | 读/写 | R/W, 找亮斑图像数据 |
| BrightSpotAnalysisResult | string | 读/写 | R/W, 找亮斑图像分析结果 |
| SpotMin | int | 读/写 | 亮斑最小值 |
| SpotMax | int | 读/写 | 亮斑最大值 |

## 公共方法

| 名称 | 参数 | 类型 | 说明 | 示例 |
|------|------|------|------|------|
| SnapAndAnalysis | exposureTime: int | 方法 | 抓拍并检测 | SnapAndAnalysis(100) |
| ClearBitmapResult | 无 | 方法 | 清除位图结果 |  |
| VisionAnalysis | mat: Mat, resultBitmap: string, errorMsg: string | 方法 | 视觉分析 |  |
| FindBrightSpot | mat: Mat, resultBitmap: string, errorMsg: string | 方法 | 找亮斑 |  |
| SnapAndAnalysisBrightSpot | exposureTime: int | 方法 | 抓拍并检测亮斑 | SnapAndAnalysisBrightSpot(100) |
