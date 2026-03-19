using System;
using System.Runtime.InteropServices;
using System.Text;

namespace CommonUtility
{
    public class WriteNowControlApi
    {
        public delegate UInt32 WN_FileTransferProgressProc(UInt32 file_size, UInt32 file_pos);
        public const string DllFilename = @"\DllImport\wn_comm.dll";

        [DllImport(DllFilename, CharSet = CharSet.Unicode, EntryPoint = "WN_OpenCommPortW")]
        private static extern IntPtr WN_OpenCommPortW(string comPort, string comSettings);

        [DllImport(DllFilename, CharSet = CharSet.Unicode, EntryPoint = "WN_CloseCommPortW")]
        private static extern uint WN_CloseCommPortW(IntPtr handle);

        [DllImport(DllFilename, CharSet = CharSet.Unicode, EntryPoint = "WN_SendFileW")]
        private static extern uint WN_SendFileW(IntPtr handle, string protocol, string srcFilename, string dstPath,
            bool forceFiletransfert, WN_FileTransferProgressProc progress);

        [DllImport(DllFilename, CharSet = CharSet.Unicode, EntryPoint = "WN_ReceiveFile")]
        private static extern uint WN_ReceiveFile(IntPtr handle, string protocol, string srcFilename, string dstPath,
            bool forceFiletransfert, WN_FileTransferProgressProc progress);

        [DllImport(DllFilename, CharSet = CharSet.Unicode, EntryPoint = "WN_ExeCommandW")]
        private static extern uint WN_ExeCommandW(IntPtr handle, string command, StringBuilder answer, uint maxLen, uint timeOutMs, ref int answerType);

        public IntPtr Handle;

        /// <summary>
        /// 通过LAN连接WriteNow烧写器
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static IntPtr ConnectWriteNowLan(string ip)
        {
            return WN_OpenCommPortW("LAN", ip);
        }

        /// <summary>
        /// 断开与WriteNow烧写器的连接
        /// </summary>
        /// <param name="handle"></param>
        public static void DisConnectWriteNow(IntPtr handle) => WN_CloseCommPortW(handle);

        public static void SendFile(IntPtr handle, string filePath)
        {
            var result = WN_SendFileW(handle, "ymodem", filePath, @"\images", true, TransferCallback);
        }

        //public static void SendFile2(IntPtr handle, string filePath)
        //{
        //    WN_SendFileW(handle, "ymodem", filePath, @"\projects", true, TransferCallback);
        //}

        public static void ReceiveFile(IntPtr handle, string imgaeFileName, string saveFilePath)=> WN_ReceiveFile(handle, "ymodem", @"\images\" + imgaeFileName, saveFilePath, true, TransferCallback);

        public static string ExeCommand(IntPtr handle, string cmd, uint timeOut = 2000)
        {
            var answer = new StringBuilder(10000);
            var answerType = -1;

            WN_ExeCommandW(handle, cmd, answer, (uint)answer.Capacity, timeOut, ref answerType);
            return answerType == 0 ? answer.ToString() : string.Empty;
        }

        private static uint TransferCallback(uint fileSize, uint filePos)
        {
            return 0;
        }
    }
}
