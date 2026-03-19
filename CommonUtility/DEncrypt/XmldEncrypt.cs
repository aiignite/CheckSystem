using CommonUtility.FileOperator;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Xml;

namespace CommonUtility.DEncrypt
{
    public static class XmldEncrypt
    {
        public static string KeyString = "B6BA446E5C8DCC58E03B3C1BDCE1498A054272317A1B104782F12C53380ADF45";
        public static string IvString = "FE3E64E484592B8C04A5FCCC4B54571B";

        /// <summary>
        /// XML 文件加密
        /// </summary>
        /// <param name="inputFilePath"></param>
        /// <param name="outputFilePath"></param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        public static bool EncryptXmlFile(string inputFilePath, string outputFilePath, byte[] key, byte[] iv, bool isNeedBk = true)
        {
            using (var aes = Aes.Create())
            {
                {
                    var originFileInfo = new FileInfo(inputFilePath);
                    var originFileName = originFileInfo.FullName;
                    var extension = originFileInfo.Extension;
                    var tempFile = originFileName.Substring(0, originFileName.Length - extension.Length) + "~temp" + extension;

                    try
                    {
                        aes.Key = key;
                        aes.IV = iv;

                        using (var fileStream = new FileStream(tempFile, FileMode.Create))
                        using (var cryptoStream = new CryptoStream(fileStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                        using (var writer = new StreamWriter(cryptoStream))
                        {
                            var xmlDoc = new XmlDocument();
                            xmlDoc.Load(inputFilePath);
                            xmlDoc.Save(writer);

                            //Console.WriteLine("XML 文件加密完成！");
                        }

                        // 备份原文件以防止意外的数据丢失
                        if (isNeedBk)
                            File.Copy(inputFilePath, inputFilePath + ".beforeEncryptBak~" + Guid.NewGuid(), true);

                        if (File.Exists(outputFilePath))
                            File.Delete(outputFilePath);

                        File.Move(tempFile, outputFilePath);
                        File.Delete(tempFile);
                        return true;
                    }
                    catch (Exception e)
                    {
                        if (File.Exists(tempFile))
                            File.Delete(tempFile);
                        Console.WriteLine(e);
                        return false;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// XML 文件加密
        /// </summary>
        /// <param name="inputFilePath"></param>
        /// <param name="outputFilePath"></param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        public static bool EncryptXmlFile(string inputFilePath, string outputFilePath, string key, string iv, bool isNeedBk = true)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(iv) ||
                key.Length % 2 != 0 || iv.Length % 2 != 0)
                return false;

            try
            {
                var keyBytes = new List<byte>();
                var ivBytes = new List<byte>();

                for (var i = 0; i < key.Length; i = i + 2)
                    keyBytes.Add(Convert.ToByte(key.Substring(i, 2), 16));
                for (var i = 0; i < iv.Length; i = i + 2)
                    ivBytes.Add(Convert.ToByte(iv.Substring(i, 2), 16));

                return EncryptXmlFile(inputFilePath, outputFilePath, keyBytes.ToArray(), ivBytes.ToArray(), isNeedBk);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }

            return false;
        }

        /// <summary>
        /// XML 文件解密
        /// </summary>
        /// <param name="inputFilePath"></param>
        /// <param name="outputFilePath"></param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <param name="isNeedBk"></param>
        public static bool DecryptXmlFile(string inputFilePath, string outputFilePath, byte[] key, byte[] iv, bool isNeedBk = true)
        {
            using (var aes = Aes.Create())
            {
                var originFileInfo = new FileInfo(inputFilePath);
                var originFileName = originFileInfo.FullName;
                var extension = originFileInfo.Extension;
                var tempFile = originFileName.Substring(0, originFileName.Length - extension.Length) + "~temp" + extension;

                try
                {
                    aes.Key = key;
                    aes.IV = iv;

                    using (var fileStream = new FileStream(inputFilePath, FileMode.Open))
                    using (var cryptoStream = new CryptoStream(fileStream, aes.CreateDecryptor(), CryptoStreamMode.Read))
                    using (var reader = new StreamReader(cryptoStream))
                    {
                        var xmlDoc = new XmlDocument();
                        xmlDoc.Load(reader);
                        xmlDoc.Save(tempFile);

                        //Console.WriteLine("XML 文件解密完成！");
                    }

                    // 备份原文件以防止意外的数据丢失
                    if (isNeedBk)
                        File.Copy(inputFilePath, inputFilePath + ".beforeDecryptBak~" + Guid.NewGuid(), true);

                    if (File.Exists(outputFilePath))
                        File.Delete(outputFilePath);

                    File.Move(tempFile, outputFilePath);
                    File.Delete(tempFile);
                    return true;
                }
                catch (Exception e)
                {
                    if (File.Exists(tempFile))
                        File.Delete(tempFile);
                    Console.WriteLine(e);
                    return false;
                }
            }

            return false;
        }

        /// <summary>
        /// XML 文件解密
        /// </summary>
        /// <param name="inputFilePath"></param>
        /// <param name="outputFilePath"></param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <param name="isNeedBk"></param>
        /// <returns></returns>
        public static bool DecryptXmlFile(string inputFilePath, string outputFilePath, string key, string iv, bool isNeedBk = true)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(iv) ||
                key.Length % 2 != 0 || iv.Length % 2 != 0)
                return false;

            try
            {
                var keyBytes = new List<byte>();
                var ivBytes = new List<byte>();

                for (var i = 0; i < key.Length; i = i + 2)
                    keyBytes.Add(Convert.ToByte(key.Substring(i, 2), 16));
                for (var i = 0; i < iv.Length; i = i + 2)
                    ivBytes.Add(Convert.ToByte(iv.Substring(i, 2), 16));

                return DecryptXmlFile(inputFilePath, outputFilePath, keyBytes.ToArray(), ivBytes.ToArray(), isNeedBk);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }

            return false;
        }

        public static T DeserializeEncryptXmlFile<T>(string filePath)
        {
            var inputFileInfo = new FileInfo(filePath);
            var tempFile =
                inputFileInfo.FullName.Substring(0, inputFileInfo.FullName.Length - inputFileInfo.Extension.Length);

            var temp = tempFile + "~tempEncrypt~" + Guid.NewGuid() + ".xml";

            try
            {
                DecryptXmlFile(filePath, temp, KeyString, IvString, false);
                return XmlHelper.Deserialize<T>(temp);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                return default(T);
            }
            finally
            {
                if (File.Exists(temp))
                    File.Delete(temp);
            }
        }
    }
}
