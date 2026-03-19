using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace CommonUtility.FileOperator
{
    public static class XmlHelper
    {
        /// <summary>
        /// 将XML反序列化为一个对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        public static T Deserialize<T>(string filePath)
        {
            var type = typeof(T);

            object targetObj;

            var serializer = new XmlSerializer(type);

            using (var fs = new FileStream(filePath, FileMode.Open))
            {
                using (var reader = XmlReader.Create(fs))
                {
                    targetObj =
                        (T)serializer.Deserialize(reader);
                }
            }

            return (T)targetObj;
        }

        /// <summary>
        /// 将一个对象按XML序列化的方式写入到一个文件
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="path">路径</param>
        /// <param name="encoding">编码类型</param>
        public static void SerializeToFile(object obj, string path, Encoding encoding)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException("path");

            if (obj is null)
                return;

            // 这里加一段备份的方法
            using (var file = new FileStream(path, FileMode.Create, FileAccess.Write))
                XmlSerializeInternal(file, obj, encoding);
        }

        private static void XmlSerializeInternal(Stream stream, object o, Encoding encoding)
        {
            if (o == null)
                throw new ArgumentNullException("o");
            if (encoding == null)
                throw new ArgumentNullException("encoding");

            var serializer = new XmlSerializer(o.GetType());

            var settings = new XmlWriterSettings
            {
                Indent = true,
                NewLineChars = "\r\n",
                Encoding = encoding,
                IndentChars = "    "
            };

            using (var writer = XmlWriter.Create(stream, settings))
            {
                serializer.Serialize(writer, o);
                writer.Close();
            }
        }
    }
}
