# VisionAnalysis 控制器接口文档

**命名空间**: `Controller`
**父类**: `ControllerBase`
**控制器描述**: VisionAnalysis

### 接口列表

| 控制器名称 | 控制器描述 | 名称 | 类型 | 类别 | 参数 | 说明 | 示例 |
|-----------|-----------|------|------|------|------|------|------|
| VisionAnalysis | R/W,最大拍摄数量 | _maxStaticCaptureCount | int | 字段 | - | 最大拍摄数量 | - |
| VisionAnalysis | R/W,相机SN | CameraSn | string | 字段 | - | 相机序列号 | - |
| VisionAnalysis | R/W,是否使用灰度图 | IsUseGray | bool | 字段 | - | 是否使用灰度图 | - |
| VisionAnalysis | 通过相机sn连接相机 | ConnectCamera | 方法 | 方法 | string sn | 通过相机sn连接相机 | - |
| VisionAnalysis | 关闭相机 | CloseCamera | 方法 | 方法 | - | 关闭相机 | - |
| VisionAnalysis | 设置增益 | SetGain | 方法 | 方法 | int gain | 设置增益 | - |
| VisionAnalysis | 设置曝光 | SetExposure | 方法 | 方法 | int exposure | 设置曝光 | - |
| VisionAnalysis | 抓拍一张 | Capture | 方法 | 方法 | string buffName | 抓拍一张 | - |
| VisionAnalysis | 启动连续抓拍 | StartGrab | 方法 | 方法 | string buffName, int count | 启动连续抓拍 | - |
| VisionAnalysis | 释放图像缓存 | ReleaseBuff | 方法 | 方法 | - | 释放图像缓存 | - |
