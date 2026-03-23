# AlcmDevice

## Controller Description

ALCM设备，用于条码生成

## Public Fields

| 名称 | 描述 | 类型 |
|------|------|------|
| Ws1QuiescentCurrent | R,工位1静态电流 | double |
| Ws2QuiescentCurrent | R,工位2静态电流 | double |
| Ws3QuiescentCurrent | R,工位3静态电流 | double |
| Ws4QuiescentCurrent | R,工位4静态电流 | double |
| IsWs1Ok | R,工位1是否OK | ushort |
| IsWs2Ok | R,工位2是否OK | ushort |
| IsWs3Ok | R,工位3是否OK | ushort |
| IsWs4Ok | R,工位4是否OK | ushort |
| Ws1Barcode | R,工位1二维码 | string |
| Ws2Barcode | R,工位2二维码 | string |
| Ws3Barcode | R,工位3二维码 | string |
| Ws4Barcode | R,工位4二维码 | string |
| Ws1PcbaBarcode | R,工位1二维码 | string |
| Ws2PcbaBarcode | R,工位2二维码 | string |
| Ws3PcbaBarcode | R,工位3二维码 | string |
| Ws4PcbaBarcode | R,工位4二维码 | string |
| IsWs1Complete | R,工位1是否完成 | ushort |
| IsWs2Complete | R,工位2是否完成 | ushort |
| IsWs3Complete | R,工位3是否完成 | ushort |
| IsWs4Complete | R,工位4是否完成 | ushort |
| MusicFilePath | R/W,音乐文件路径 | string |

## Public Methods

| 方法名称 | 描述 | 参数 |
|----------|------|------|
| PlayMusic | 播放音乐 |  |
| StopMusic | 停止音乐 |  |
| Generate4Barcode | 生成二维码信息 | generalPartNo, generalVpps, seeyaoDuns, track1, track2, track3 |
