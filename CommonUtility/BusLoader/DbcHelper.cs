using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace CommonUtility.BusLoader
{
    public class DbcHelper
    {
        public Dictionary<string, DbcFile> MyDbcFile = new Dictionary<string, DbcFile>();
        public string Path = string.Empty;
        private string _fileBuffer = string.Empty;

        public int Parse(string path)
        {
            Path = path;
            FileLoader.Load(path, ref _fileBuffer);

            var fileInfo = new FileInfo(path);
            var err = StrToDbeFile(fileInfo.Name.Remove(fileInfo.Name.Length - 4, 4));
            return err;
        }

        private int StrToDbeFile(string fileName)
        {
            const int err = 0;

            if (MyDbcFile.ContainsKey(fileName))
                return -1;

            MyDbcFile.Add(fileName, new DbcFile());

            if (_fileBuffer == null)
            {
                if (_fileBuffer == string.Empty)
                    ExceptionHandler.Report("Dbc文件为空");
            }

            if (_fileBuffer == null)
                return err;

            var bufferAry = _fileBuffer.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            if (bufferAry.Length < 3)
                return ExceptionHandler.Report("Dbc文件格式错误");

            var lineNum = bufferAry.Length;
            var isMessageValid = false;

            for (var i = 0; i < lineNum; i++)
            {
                var lineAry = bufferAry[i].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (lineAry.Length < 1)
                    return ExceptionHandler.Report("Dbc文件行格式错误");

                switch (lineAry[0])
                {
                    case "VAL_:":
                        {

                            break;
                        }

                    case "CM_:":
                        {
                            var cmt = new Comment { Cmt = bufferAry[i] };
                            MyDbcFile[fileName].Comments.Add(cmt);
                            break;
                        }

                    case "BU_:":
                        {
                            for (var j = 1; j < lineAry.Length; j++)
                                MyDbcFile[fileName].Nodes.Add(lineAry[j]);
                            break;
                        }

                    case "BO_":
                        {
                            var message = new Message();
                            var id = Convert.ToUInt32(lineAry[1]);

                            // 跳过默认的消息
                            if (id == 0xC0000000)
                            {
                                isMessageValid = false;
                                break;
                            }

                            isMessageValid = true;
                            //最高位为1的为扩展帧
                            if ((id & 0x80000000) != 0)
                            {
                                id &= 0x7FFFFFFF;
                                message.IsExternId = true;
                            }
                            else
                            {
                                message.IsExternId = false;
                            }
                            message.MessgeId = id;
                            
                            if (lineAry[3]==":")
                            {
                                message.MessageSize = Convert.ToUInt32(lineAry[4]);
                                message.Transmitter = lineAry[5];
                                message.MessageName = lineAry[2].Substring(0, lineAry[2].Length);
                            }
                            else
                            {
                                message.MessageSize = Convert.ToUInt32(lineAry[3]);
                                message.Transmitter = lineAry[4];
                                message.MessageName = lineAry[2].Substring(0, lineAry[2].Length - 1);
                            }

                            MyDbcFile[fileName].Messages.Add(message);
                            break;
                        }

                    case "SG_":
                        {
                            if (isMessageValid)
                            {
                                uint byteOffset;
                                var signal = new Signal { SignalName = lineAry[1] };

                                if (lineAry[2] == ":")
                                {
                                    signal.MultiplexerIndicator = -2;
                                    byteOffset = 0;
                                }
                                else
                                {
                                    byteOffset = 1;
                                    if (lineAry[2][0] == 'M')
                                    {
                                        signal.MultiplexerIndicator = -1;
                                    }
                                    else if (lineAry[2][0] == 'm')
                                    {
                                        signal.MultiplexerIndicator = Convert.ToInt32(lineAry[2].Substring(1, lineAry[2].Length - 1));
                                    }
                                    else
                                    {
                                        return ExceptionHandler.Report("Dbc信号格式错误");
                                    }
                                }

                                var sp = lineAry[3 + byteOffset].Split(new[] { '|', '@' }, StringSplitOptions.RemoveEmptyEntries);

                                signal.StartBit = Convert.ToUInt32(sp[0]);
                                signal.SignalSize = Convert.ToUInt32(sp[1]);

                                if (sp[2][0] == '0')
                                {
                                    signal.ByteOrder = 0;
                                    var startBit = (int)signal.StartBit;
                                    var bitLen = (int)signal.SignalSize;

                                    // 有bug，跨多字节还有错误，20210625
                                    // e.g. if (startBit == 7 && bitLen == 64)
                                    if (bitLen > 1)
                                    {
                                        if (bitLen > 8 * 2)
                                            break;

                                        if (startBit - bitLen + 1 < startBit / 8 * 8)
                                        {
                                            var rest = startBit / 8 * 8 - (startBit - bitLen + 1);
                                            signal.StartBit = (uint)((startBit / 8 + 1) * 8 + 7 - rest + 1);
                                        }
                                        else
                                        {
                                            signal.StartBit = (uint)(startBit - bitLen + 1);
                                        }
                                    }
                                }
                                else if (sp[2][0] == '1')
                                    signal.ByteOrder = 1;

                                if (sp[2][1] == '+')
                                    signal.ValueType = 0;
                                else if (sp[2][1] == '-')
                                    signal.ValueType = 1;

                                var sp1 = lineAry[4 + byteOffset].Split(new[] { '(', ',', ')' }, StringSplitOptions.RemoveEmptyEntries);
                                signal.Factor = Convert.ToDouble(sp1[0]);
                                signal.Offset = Convert.ToDouble(sp1[1]);

                                var sp2 = lineAry[5 + byteOffset].Split(new[] { '[', '|', ']' }, StringSplitOptions.RemoveEmptyEntries);
                                signal.Minimum = Convert.ToDouble(sp2[0]);
                                signal.Maximum = Convert.ToDouble(sp2[1]);

                                signal.UintStr = lineAry[6 + byteOffset];

                                signal.Receivers = lineAry[7 + byteOffset].Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                                MyDbcFile[fileName].Messages[MyDbcFile[fileName].Messages.Count - 1].Signals.Add(signal);
                            }
                            break;
                        }
                }
            }
            return err;
        }

        public class DbcFile
        {
            public List<string> Nodes = new List<string>();
            public List<Message> Messages = new List<Message>();
            public List<Comment> Comments = new List<Comment>();
        }

        public class Message
        {
            public uint MessgeId;
            public bool IsExternId;
            public string MessageName = string.Empty;
            public uint MessageSize;
            public string Transmitter = string.Empty;
            public List<Signal> Signals = new List<Signal>();
        }

        public class Signal
        {
            public string SignalName = string.Empty;

            /// <summary>
            /// -2为普通信号
            /// -1为复用选择信号
            /// 0~N为复用信号
            /// </summary>
            public int MultiplexerIndicator = -2;
            public uint StartBit;
            public uint SignalSize;

            /// <summary>
            /// 0为Motorola
            /// 1为Intel
            /// </summary>
            public uint ByteOrder;

            /// <summary>
            /// 0为unsigned
            /// 1为signed
            /// </summary>
            public uint ValueType;
            public double Factor;
            public double Offset;
            public double Minimum;
            public double Maximum;
            public string UintStr = string.Empty;
            public string[] Receivers;
        }

        public class Comment
        {
            public string Cmt = string.Empty;
        }

        public static class FileLoader
        {
            #region 方法
            /// <summary>
            /// 加载文本文件到字符串
            /// </summary>
            /// <param name="path">文件路径</param>
            /// <param name="stringOut">输出的字符串</param>
            /// <returns></returns>
            public static int Load(string path, ref string stringOut)
            {
                FileStream fs = null;
                StreamReader sr = null;

                try
                {
                    fs = new FileStream(path, FileMode.Open, FileAccess.Read);
                    sr = new StreamReader(fs);

                    stringOut = sr.ReadToEnd();
                }
                catch (Exception en)
                {
                    if (sr != null)
                        sr.Close();
                    if (fs != null)
                        fs.Close();
                    return ExceptionHandler.Report("加载文件失败" + en.Message);
                }

                sr.Close();
                fs.Close();

                return 0;
            }
            #endregion
        }

        public static class ExceptionHandler
        {
            public static int Report(string str)
            {
                var en = new Exception(str);
                throw en;
            }

            public static void Handle(Exception en)
            {
                MessageBox.Show(en.Message);
            }
        }
    }
}
