using System;
using System.Collections.Generic;

namespace CommonUtility
{
    public static class ModbusOperater
    {
        /// <summary>
        /// 打包modbus数据
        /// </summary>
        /// <param name="codeId">功能码</param>
        /// <param name="startAddress">起始地址</param>
        /// <param name="regLen">寄存器数量</param>
        /// <param name="values">数据内容</param>
        /// <param name="stationId">站号</param>
        /// <returns>modbus打包数据除帧头的4个字节外</returns>
        public static IEnumerable<byte> GetModbusWriteValues(
            byte codeId,
            string startAddress,
            ushort regLen,
            byte[] values,
            byte stationId = 0x01)
        {
            var sendBytes = new List<byte>();

            if (values != null)
                sendBytes.AddRange(GetBytesLength(values));

            sendBytes.Add(stationId);
            sendBytes.Add(codeId);

            var startAddr =
                BitConverter.GetBytes(Convert.ToUInt16(startAddress));
            Array.Reverse(startAddr);
            sendBytes.AddRange(startAddr);

            sendBytes.Add(BitConverter.GetBytes(regLen)[1]);
            sendBytes.Add(BitConverter.GetBytes(regLen)[0]);
            sendBytes.Add(BitConverter.GetBytes(regLen * 2)[0]);

            if (values != null)
                sendBytes.AddRange(values);

            return sendBytes.ToArray();
        }
        
        public static IEnumerable<byte> GetBytesLength(byte[] valueBytes)
        {
            var bytesLen =
                BitConverter.GetBytes((ushort) (valueBytes.Length + 7));
            Array.Reverse(bytesLen);
            return bytesLen;
        }
    }
}
