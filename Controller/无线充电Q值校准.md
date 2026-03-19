1、前提条件：
①、处于周期唤醒状态
②、电源模式有效
③、关闭寻钥匙功能


2、进入工厂模式
发送命令：0x10DBFEF1——10 0B 45 AA 59 47 4B 4A
    21 SS SS KK KK CC 00 00

SS SS：工厂模式种子，自定义随机数
KK KK：工厂模式密钥，通过工厂模式算法，计算种子SS SS得出密钥KK KK
CC：校验和，使用校验和算法计算报文数据长度和数据内容得出（帧格式的10、21字节除外），例如：CC = ~ ( 0B + 45 + AA + 59 + 47 + 4B + 4A + SS + SS + KK + KK )

工厂模式算法：
unsigned int calcKey(unsigned int seed)
{
			#define  TMP_CODE 0X7755

			unsigned char bSeed[2];
			unsigned int remainder;
			unsigned char n;
			unsigned char i;

			bSeed[0] = (unsigned char)(seed >> 8); /* MSB */
			bSeed[1] = (unsigned char)seed; /* LSB */
			remainder = 0x4351;

			for (n = 0; n < 2; n++)
			{
				remainder ^= ((bSeed[n]) << 8);
				for (i = 0; i < 8; i++)
				{
					remainder +=  TMP_CODE;
					remainder ^=  TMP_CODE;
					remainder -=  TMP_CODE;
				}
			}
			return remainder;
}

发送之后即可进入工厂模式，无回复报文。


3、启动Q值校准
发送命令：0x10DBFEF1——10 03 43 20 99 00 00 00
接收命令：0x14DAF1BE——10 03 43 20 99 00 00 00
接收到此报文后，校准开始，需要等待7秒钟的自校准过程，7秒后校准完成继续下一步获取校准参数。

4、获取校准参数
发送命令：0x10DBFEF1——10 03 43 30 89 00 00 00
接收命令：0x14DAF1BE——10 15 43 30 XX1 XX1 YY1 YY1
  0x14DAF1BE——21 ZZ1 ZZ1 XX2 XX2 YY2 YY2 ZZ2
  0x14DAF1BE——22 ZZ2 XX3 XX3 YY3 YY3 ZZ3 ZZ3
0x14DAF1BE——23 CC 00 00 00 00 00 00

XX1 XX1：线圈1的Q值，高字节在前，低字节在后
YY1 YY1：线圈1的频率
ZZ1 ZZ1：线圈1的阻抗
XX1 XX1：线圈2的Q值
YY1 YY1：线圈2的频率
ZZ1 ZZ1：线圈2的阻抗
XX1 XX1：线圈3的Q值
YY1 YY1：线圈3的频率
ZZ1 ZZ1：线圈3的阻抗
CC：校验和，除帧格式的10、21、22、23之外的数据内容的校验和，同进入工厂模式命令一样。

例如：
发送命令：0x10DBFEF1——10 03 43 30 89 00 00 00
接收命令：0x14DAF1BE——10 15 43 30 01 2E 24 CA
  0x14DAF1BE——21 01 C1 01 33 25 B5 01
  0x14DAF1BE——22 A6 01 2F 24 BC 01 C1
0x14DAF1BE——23 11 00 00 00 00 00 00

线圈1的Q值：01 2E = 302
线圈1的频率：24 CA = 9418
线圈1的阻抗：01 C1 = 449
线圈2的Q值：01 33 = 307
线圈2的频率：25 B5 = 9653
线圈2的阻抗：01 A6 = 422
线圈3的Q值：01 2F = 303
线圈3的频率：24 BC = 9404
线圈3的阻抗：01 C1 = 449

Q值管控范围：100~450
频率管控范围：5000~10000
阻抗管控范围：0~2000

