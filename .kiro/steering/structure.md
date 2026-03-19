# 项目结构

## 解决方案组织
```
CheckSystem.sln
├── CheckSystem/          # 主程序（WinForms应用）
├── Controller/           # 设备控制器层
├── CommonUtility/        # 通用工具库
├── BLL/                  # 业务逻辑层
├── DAL/                  # 数据访问层
├── Model/                # 数据模型层
├── DBUtility/            # 数据库工具
├── StateMachine/         # 状态机模块
├── UserControls/         # 自定义UI控件
├── HZH_Controls/         # 第三方UI控件库
├── DeviceDesign/         # 设备设计模块
└── Lib/                  # 第三方DLL依赖
```

## 核心模块说明

### CheckSystem（主程序）
- 入口点和主窗体
- 各类检测表单（FormCheck, FormSelection等）
- 子模块: CAN/, LIN/, CcdForms/, HelperForms/, RobotForms/

### Controller（设备控制器）
- ControllerBase.cs: 控制器基类
- 产品专用控制器（按车型/产品命名，如Vw416RearLamp.cs）
- 硬件控制器: PowerNgi.cs, SiemensPlc.cs, HikBarcodeScaner.cs等
- 通信控制器: CanFdWithGateway.cs, LinCommunication.cs

### CommonUtility（工具库）
- BusLoader/: CAN/LIN总线通信（zlgcan.cs, toomoss.cs, DbcHelper.cs）
- FileOperator/: 文件操作（CsvFileHelper, ExcelFileHelper, XmlHelper）
- MyCameraSdk/: 相机封装
- HikSdk/: 海康设备SDK封装

### 三层架构（BLL/DAL/Model）
- Model: 数据实体类（与数据库表对应）
- DAL: 数据访问（SQL操作）
- BLL: 业务逻辑

## 命名约定
- 控制器类: 产品型号+功能（如 `Vw416RearLamp`, `SiemensPlc`）
- 表单类: Form+功能（如 `FormCheck`, `FormSelection`）
- 数据模型: 小写前缀+功能（如 `deviceInfo`, `productCheckData`）

## 依赖关系
```
CheckSystem → Controller → CommonUtility → DBUtility
           → BLL → DAL → Model
                      → DBUtility
```
