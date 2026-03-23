# FogVisionAnalysis 控制器接口文档

**命名空间**: `Controller`
**父类**: `ControllerBase`
**控制器描述**: FOG视觉分析控制器

### 接口列表

| 控制器名称 | 控制器描述 | 名称 | 类型 | 类别 | 参数 | 说明 | 示例 |
|-----------|-----------|------|------|------|------|------|------|
| FogVisionAnalysis | FOG视觉分析控制器 | AnalysisResultFog | string | 字段 | - | FOG分析结果 | - |
| FogVisionAnalysis | FOG视觉分析控制器 | AnalysisResultBase64String | string | 字段 | - | 分析结果Base64字符串 | - |
| FogVisionAnalysis | FOG视觉分析控制器 | EMax | double | 字段 | - | EMax值 | - |
| FogVisionAnalysis | FOG视觉分析控制器 | V_5U | double | 字段 | - | V_5U值 | - |
| FogVisionAnalysis | FOG视觉分析控制器 | V_5B | double | 字段 | - | V_5B值 | - |
| FogVisionAnalysis | FOG视觉分析控制器 | H_10L | double | 字段 | - | H_10L值 | - |
| FogVisionAnalysis | FOG视觉分析控制器 | H_10R | double | 字段 | - | H_10R值 | - |
| FogVisionAnalysis | FOG视觉分析控制器 | CameraSn | string | 字段 | - | 当前使用相机SN号 | - |
| FogVisionAnalysis | FOG视觉分析控制器 | Ratio | double | 字段 | - | 当前使用相机的镜头大小 | 8000 |
| FogVisionAnalysis | FOG视觉分析控制器 | PixelScale | double | 字段 | - | 当前使用相机像元大小 | 2.2 |
| FogVisionAnalysis | FOG视觉分析控制器 | CameraDistanceCm | double | 字段 | - | 相机物距 | 41 |
| FogVisionAnalysis | FOG视觉分析控制器 | FresnelLensDistanceCm | double | 字段 | - | 菲涅尔透镜物距 | 64 |
| FogVisionAnalysis | FOG视觉分析控制器 | Images | Dictionary | 字段 | - | 图像字典 | - |
| FogVisionAnalysis | FOG视觉分析控制器 | AnalyzeFog | void | 方法 | string fogSrcName | 分析FOG | - |
| FogVisionAnalysis | FOG视觉分析控制器 | ImageDeNoise | void | 方法 | Mat imagesrc... | 图像预处理 | - |
| FogVisionAnalysis | FOG视觉分析控制器 | CalculateScaleRatio | double | 方法 | double pixelSizeMicrons... | 计算像素-实际长度比例系数 | - |
| FogVisionAnalysis | FOG视觉分析控制器 | CalculateFresnelLensDistancePixel | double | 方法 | - | 计算菲涅尔透镜距离像素 | - |
| FogVisionAnalysis | FOG视觉分析控制器 | CalculateRotAngle | double | 方法 | Point pointA, Point pointB | 计算旋转角度 | - |
| FogVisionAnalysis | FOG视觉分析控制器 | OpenCamera | void | 方法 | uint exposeTime | 打开相机 | - |
| FogVisionAnalysis | FOG视觉分析控制器 | CaptureImage | void | 方法 | string name, int captureCount | 捕获图像 | - |
| FogVisionAnalysis | FOG视觉分析控制器 | CloseCamera | void | 方法 | - | 关闭相机 | - |
| FogVisionAnalysis | FOG视觉分析控制器 | AppendImage | void | 方法 | string name, Mat mat | 添加图像 | - |
