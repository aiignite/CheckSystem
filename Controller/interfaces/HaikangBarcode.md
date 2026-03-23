# HaikangBarcode 接口文档

## 控制器信息

| 项目 | 内容 |
|------|------|
| 控制器名称 | HaikangBarcode |
| 描述 | (无描述) |
| 命名空间 | Controller |
| 基类 | ControllerBase |

## 公共字段

| 名称 | 类型 | 描述 |
|------|------|------|
| BarcodeCount | int | 需要条码数量 |
| ChangeResult | string | R,切换参数结果 |
| IsGetBar | bool | R/W,是否获取到条码 |
| Barcode1 | string | R,条码1 |
| Barcode2 | string | R,条码2 |
| Barcode3 | string | R,条码3 |
| Barcode4 | string | R,条码4 |
| Barcode5 | string | R,条码5 |
| Barcode6 | string | R,条码6 |
| Barcode7 | string | R,条码7 |
| Barcode8 | string | R,条码8 |
| Barcode9 | string | R,条码9 |
| Barcode10 | string | R,条码10 |
| Barcode11 | string | R,条码11 |
| Barcode12 | string | R,条码12 |
| Barcode13 | string | R,条码13 |
| Barcode14 | string | R,条码14 |
| Barcode15 | string | R,条码15 |
| Barcode16 | string | R,条码16 |

## 公共方法

| 名称 | 参数 | 返回类型 | 描述 |
|------|------|----------|------|
| InitTcpClient | ipstress: string | void | 连接 |
| InitTcpClientChangeByDate0109 | ipPort: string | void | 连接(带参数) |
| TcpClientClose | 无 | void | 关闭连接 |
| ClientWrite | msg: string | void | ClientWrite |
| ChangeParameter | idx: int | void | 切换参数 |
| ClearBarcode | 无 | void | 清除条码 |

## 方法详细说明

### InitTcpClient

**描述**: 连接

**参数**:
- `ipstress` (string): IP和端口 (格式: "192.168.1.100:8080")

**示例代码**:
```csharp
controller.InitTcpClient("192.168.1.100:8080");
```

---

### InitTcpClientChangeByDate0109

**描述**: 连接(带参数)

**参数**:
- `ipPort` (string): IP和端口 (格式: "192.168.1.100:8080")

**示例代码**:
```csharp
controller.InitTcpClientChangeByDate0109("192.168.1.100:8080");
```

---

### TcpClientClose

**描述**: 关闭连接

**示例代码**:
```csharp
controller.TcpClientClose();
```

---

### ClientWrite

**描述**: ClientWrite

**参数**:
- `msg` (string): 发送消息

**示例代码**:
```csharp
controller.ClientWrite("<Set,Acq,1>");
```

---

### ChangeParameter

**描述**: 切换参数

**参数**:
- `idx` (int): 参数索引

**示例代码**:
```csharp
controller.ChangeParameter(1);
string result = controller.ChangeResult;
```

---

### ClearBarcode

**描述**: 清除条码

**示例代码**:
```csharp
controller.ClearBarcode();
bool hasBarcode = controller.IsGetBar;
string barcode1 = controller.Barcode1;
```
