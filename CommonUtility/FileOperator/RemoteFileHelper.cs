using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace CommonUtility.FileOperator
{
    public static class RemoteFileHelper
    {
        [DllImport("mpr.dll")]
        private static extern int WNetAddConnection2(NetResource netResource,
            string password, string username, int flags);

        [DllImport("mpr.dll")]
        private static extern int WNetCancelConnection2(string name, int flags, bool force);

        private const int RESOURCETYPE_DISK = 0x1;
        private const int CONNECT_UPDATE_PROFILE = 0x1;
        private const int DISCONNECT_UPDATE_PROFILE = 0x2;

        [StructLayout(LayoutKind.Sequential)]
        private class NetResource
        {
            public int ResourceScope;
            public int ResourceType;
            public int DisplayType;
            public int Usage;
            public string LocalName;
            public string RemoteName;
            public string Comment;
            public string Provider;
        }

        public static List<FileInfo> GetRemoteFileList(string remotePath, string username, string password)
        {
            var list = new List<FileInfo>();

            // 连接到远程共享文件夹
            var result = ConnectToRemoteDirectory(remotePath, username, password);
            if (result == 0)
            {
                Console.WriteLine("Connected to remote directory successfully.");

                // 访问远程共享文件夹
                try
                {
                    var files = Directory.GetFiles(remotePath);
                    foreach (var file in files)
                    {
                        var fileInfo = new FileInfo(file);
                        list.Add(fileInfo);
                        Console.WriteLine(file);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error accessing remote directory: {ex.Message}");
                }
                finally
                {
                    // 断开与远程共享文件夹的连接
                    DisconectFromRemoteDirectory(remotePath);
                    Console.WriteLine("Disconnected from remote directory.");
                }
            }
            else
            {
                Console.WriteLine($"Failed to connect to remote directory. Error code: {result}");
            }

            return list;
        }

        /// <summary>
        /// 连接到远程共享文件夹
        /// </summary>
        /// <param name="remotePath"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static int ConnectToRemoteDirectory(string remotePath, string username, string password)
        {
            // 创建网络资源对象
            var netResource = new NetResource
            {
                ResourceType = RESOURCETYPE_DISK,
                RemoteName = remotePath
            };
            try
            {
                var result = WNetAddConnection2(netResource, password, username, CONNECT_UPDATE_PROFILE);
                return result;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        /// <summary>
        /// 断开与远程共享文件夹的连接
        /// </summary>
        /// <param name="remotePath"></param>
        public static void DisconectFromRemoteDirectory(string remotePath)
        {
            try
            {
                WNetCancelConnection2(remotePath, DISCONNECT_UPDATE_PROFILE, false);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
