# CcdAutoAssemblyPlateTrack 接口文档

## 控制器信息

| 项目 | 内容 |
|------|------|
| 控制器名称 | CcdAutoAssemblyPlateTrack |
| 描述 | (无描述) |
| 命名空间 | Controller |
| 基类 | ControllerBase |

## 公共字段

| 名称 | 类型 | 描述 |
|------|------|------|
| PlateBarcode | string | 载盘条码 |
| ProductBarcode | string | 产品条码 |
| IsOk | bool | 是否合格 |
| ProductPosition | int | 产品位置 |

## 公共方法

| 名称 | 参数 | 返回类型 | 描述 |
|------|------|----------|------|
| SavePlateBarcode | 无 | void | 保存载盘条码 |
| ReadPlateBarcode | 无 | void | 读取载盘条码 |
| InitServer | port: int, productCount: int | void | 初始化服务器 |
| SendQuery | plateBarcode: string, ackDataList: out List<AckData> | bool | 发送查询 |

## 方法详细说明

### SavePlateBarcode

**描述**: 保存载盘条码

**示例代码**:
```csharp
controller.PlateBarcode = "PLATE123";
controller.ProductBarcode = "PRODUCT456";
controller.ProductPosition = 1;
controller.SavePlateBarcode();
```

---

### ReadPlateBarcode

**描述**: 读取载盘条码

**示例代码**:
```csharp
controller.PlateBarcode = "PLATE123";
controller.ProductPosition = 1;
controller.ReadPlateBarcode();
string productBarcode = controller.ProductBarcode;
bool isOk = controller.IsOk;
```

---

### InitServer

**描述**: 初始化服务器

**参数**:
- `port` (int): 端口号
- `productCount` (int): 产品数量

**示例代码**:
```csharp
controller.InitServer(5000, 4);
```

---

### SendQuery

**描述**: 发送查询

**参数**:
- `plateBarcode` (string): 载盘条码
- `ackDataList` (out List<AckData>): 响应数据列表

**示例代码**:
```csharp
List<CcdAutoAssemblyPlateTrack.AckData> ackDataList;
bool success = controller.SendQuery("PLATE123", out ackDataList);
```

---

## 内部类

### AckData

**描述**: 响应数据

| 名称 | 类型 | 描述 |
|------|------|------|
| Position | int | 位置 |
| PlateBarcode | string | 载盘条码 |
| ProductBarcode | string | 产品条码 |
| IsOk | bool | 是否合格 |