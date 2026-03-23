# PesVisionAnalysisNDLB 控制器接口文档

**命名空间**: `Controller`
**父类**: `ControllerBase`
**控制器描述**: PesVisionAnalysisNDLB

### 接口列表

| 控制器名称 | 控制器描述 | 名称 | 类型 | 类别 | 参数 | 说明 | 示例 |
|-----------|-----------|------|------|------|------|------|------|
| PesVisionAnalysisNDLB | 当前使用相机SN号 | CameraSn | string | 字段 | - | R/W,当前使用相机SN号 | - |
| PesVisionAnalysisNDLB | 当前使用相机的镜头大小 | Ratio | double | 字段 | - | R/W,当前使用相机的镜头大小-通常采用的4mm即4000um | - |
| PesVisionAnalysisNDLB | 当前使用相机像元大小 | PixelScale | double | 字段 | - | R/W,当前使用相机像元大小-通常采用的是2.2um | - |
| PesVisionAnalysisNDLB | 相机物距 | CameraDistanceCm | double | 字段 | - | R/W,相机物距 | - |
| PesVisionAnalysisNDLB | 菲涅尔透镜物距 | FresnelLensDistanceCm | double | 字段 | - | R/W,菲涅尔透镜物距 | - |
| PesVisionAnalysisNDLB | 模板匹配路徑 | MatchTempFilePath | string | 字段 | - | R/W,模板匹配路徑 | - |
