# 技术栈

## 框架与运行时
- .NET Framework 4.5.1（主程序）
- .NET Framework 4.5（Controller、CommonUtility）
- .NET Framework 3.5（BLL/DAL/Model - 旧版数据层）
- Windows Forms（UI框架）

## 构建系统
- Visual Studio 2022（推荐）
- MSBuild
- NuGet包管理

## 主要依赖库
- UI: SunnyUI 3.6.5, HZH_Controls, WeifenLuo.WinFormsUI.Docking
- 图像处理: OpenCvSharp 4.0, EmguCV, NationalInstruments.Vision
- 通信: HslCommunication, SimpleTCP, TouchSocket, SuperSocket
- 数据库: SqlSugar 5.0, System.Data.SQLite
- 序列化: Newtonsoft.Json, CsvHelper, MiniExcel
- 状态机: Stateless
- 数学计算: MathNet.Numerics
- 音频: NAudio

## 硬件SDK
- 海康条码扫描器 (MvCodeReaderSDK)
- 大恒相机 (GxIAPINET)
- NI视觉 (NationalInstruments.Vision)
- ZLG CAN设备
- Toomoss USB设备

## 数据库
- SQL Server（主数据库）
- SQLite（本地缓存）

## 常用命令

### 构建
```powershell
# 使用MSBuild构建解决方案
msbuild CheckSystem.sln /p:Configuration=Debug /p:Platform="Any CPU"

# 或使用Visual Studio命令行
devenv CheckSystem.sln /Build Debug
```

### NuGet还原
```powershell
nuget restore CheckSystem.sln
```

### 清理
```powershell
msbuild CheckSystem.sln /t:Clean
```

## 配置文件
- App.config: 应用程序配置（数据库连接字符串、设备参数）
- XML配置文件: 检测参数、产品配置
