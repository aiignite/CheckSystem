using System;
using System.Runtime.InteropServices;

namespace CommonUtility
{
    public class CycloneControlApi
    {
        public const int MaxNumberOfCyclones = 100;

        /* CATEGORY NAMES AND THEIR PROPERTIES
        A list of properties supported by the Cyclone can
        be retrieved with the getPropertyList() routine */
        public const string CycloneProperties = "cycloneProperties";
        public const string SelectCyclonePropertyType = "cycloneType";
        public const string SelectCyclonePropertyFirmwareVersion = "cycloneFirmwareVersion";
        public const string SelectCyclonePropertyCycloneLogicVersion = "cycloneLogicVersion";
        public const string SelectCyclonePropertyName = "cycloneName";
        public const string SelectCyclonePropertyNumberOfExternalImages = "numberOfExternalImages";
        public const string SelectCyclonePropertyNumberOfInternalImages = "numberOfInternalImages";
        public const string SelectCyclonePropertyNumberOfImages = "totalnumberofimages";
        public const string CurrentImageSelected = "currentImageSelected";
        public const string NetworkProperties = "networkProperties";
        public const string SelectNetworkPropertyCycloneIpAddress = "cycloneIPAddress";
        public const string SelectNetworkPropertyCycloneNetmask = "cycloneNetworkMask";
        public const string SelectNetworkPropertyCycloneGateway = "cycloneNetworkGateway";
        public const string SelectNetworkPropertyCycloneDns = "cycloneDNSAddress";
        public const string ImageProperties = "imageProperties";
        public const string SelectImagePropertyName = "imageName";
        public const string SelectImagePropertyMediaType = "mediaType";
        public const string SelectImagePropertyUniqueId = "imageUniqueId";
        public const string SelectImagePropertyCrc32 = "imageCRC32";
        public const string SelectImagePropertyVoltageSettings = "imageVoltageSettings";
        public const string SelectImagePropertyFirstObjectCrc = "imageFirstObjectCrc";
        public const string SelectImagePropertyFirstDeviceCrc = "imageFirstDeviceCrc";
        public const string SelectImagePropertySerialNumberCount = "imageSerialNumberCount";
        public const string SelectImagePropertyGetSerialNumber = "imageSerialNumber"; /* Append index in decimal format, e.g. 'imageSerialNumber1', 'imageSerialNumber2' */

        //MEDIA TYPE VALUES
        public const byte MediaInternal = 1;
        public const byte MediaExternal = 2;

        //CYCLONE SPECIAL FEATURES 
        public const uint PeSetFirmwareUpdatePrintfCallback = 0x58006001;
        public const uint PeCycloneSdkSetFirmwareUpdateMode = 0x58006002;
        public const uint PeCycloneSdkEnableDebugOutFile = 0x58006006;

        public const uint CycloneGetImageDescriptionFromFile = 0xA5001001;
        public const uint CycloneGetImageCrc32FromFile = 0xA5001002;
        public const uint CycloneGetImageSettingsFromFile = 0xA5001003;
        public const uint CycloneGetImageCommmandLineParamsFromFile = 0xA5001004;
        public const uint CycloneGetImageScriptFileFromFile = 0xA5001005;

        public const uint PeCycloneGetCycloneScreenBitmapBuffer = 0xA5001101;
        public const uint PeCycloneSendDisplayTouch = 0xA5001102;
        public const uint PeCycloneDoesDisplayNeedUpdate = 0xA5001103;

        public const uint CycloneTogglePowerNoDebug = 0xA5002001;
        public const uint CycloneSetActiveSecurityCode = 0xA5002002;

        //CYCLONE PORT TYPE AND IDENTIFIER TYPES
        public const uint CyclonePortTypeUsb = 5;
        public const uint CyclonePortTypeEthernet = 6;
        public const uint CyclonePortTypeSerial = 7;

        public const uint CycloneInformationIpAddress = 1;
        public const uint CycloneInformationName = 2;
        public const uint CycloneInformationGenericPortNumber = 3;
        public const uint CycloneInformationCycloneTypeString = 4;

        public const uint FirmwareUpdateAuto = 0;
        public const uint FirmwareUpdateForce = 1;
        public const uint FirmwareUpdateNever = 2;

        public const string DllFilename = @"\DllImport\CycloneControlSDK.dll";

        /* The following imported methods have wrapper methods to properly handle the
           char* and char** C data types. 4 byte C# style bool is also converted into 
           a 1 byte C-style bool. Your application should call the wrapper method. */

        /* Private Methods */
        [DllImport(DllFilename, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "version")]
        private static extern IntPtr unmanaged_version();

        [DllImport(DllFilename, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "queryInformationOfAutodetectedCyclone")]
        private static extern IntPtr unmanaged_queryInformationOfAutodetectedCyclone(int autodetectIndex, int informationType);

        [DllImport(DllFilename, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "connectToCyclone")]
        private static extern uint unmanaged_connectToCyclone(IntPtr nameIpOrPortIdentifier);

        [DllImport(DllFilename, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "setLocalMachineIpNumber")]
        private static extern void unmanaged_setLocalMachineIpNumber(IntPtr ipNumber);

        [DllImport(DllFilename, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "startDynamicDataProgram")]
        private static extern byte unmanaged_startDynamicDataProgram(uint cycloneHandle, uint targetAddress, uint dataLength, IntPtr unmanagedCharArrayPtr);

        [DllImport(DllFilename, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "dynamicReadBytes")]
        private static extern byte unmanaged_dynamicReadBytes(uint cycloneHandle, uint targetAddress, uint dataLength, IntPtr unmanagedCharArrayPtr);

        [DllImport(DllFilename, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "getDescriptionOfErrorCode")]
        private static extern IntPtr unmanaged_getDescriptionOfErrorCode(uint cycloneHandle, uint errorCode);

        [DllImport(DllFilename, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "getImageDescription")]
        private static extern IntPtr unmanaged_getImageDescription(uint cycloneHandle, uint imageId);

        [DllImport(DllFilename, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "compareImageInCycloneWithFile")]
        private static extern byte unmanaged_compareImageInCycloneWithFile(uint cycloneHandle, IntPtr aFile, uint imageId);

        [DllImport(DllFilename, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "addCycloneImage")]
        private static extern uint unmanaged_addCycloneImage(uint cycloneHandle, uint selectedMediaType, byte replaceImageOfSameDescription, IntPtr aFile);

        [DllImport(DllFilename, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "getPropertyValue")]
        private static extern IntPtr unmanaged_getPropertyValue(uint cycloneHandle, uint resourceOrImageId, IntPtr categoryName, IntPtr propertyName);

        [DllImport(DllFilename, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "setPropertyValue")]
        private static extern byte unmanaged_setPropertyValue(uint cycloneHandle, uint resourceOrImageId, IntPtr categoryName, IntPtr propertyName, IntPtr newValue);

        [DllImport(DllFilename, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "getPropertyList")]
        private static extern IntPtr unmanaged_getPropertyList(uint cycloneHandle, uint resourceOrImageId, IntPtr categoryName);

        [DllImport(DllFilename, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "getFirmwareVersion")]
        private static extern IntPtr unmanaged_getFirmwareVersion(uint cycloneHandle);

        [DllImport(DllFilename, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "startImageExecution")]
        private static extern byte unmanaged_startImageExecution(uint cycloneHandle, byte imageId);

        [DllImport(DllFilename, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "resetCyclone")]
        private static extern byte unmanaged_resetCyclone(uint cycloneHandle, uint resetDelayInMs);

        [DllImport(DllFilename, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "formatCycloneMemorySpace")]
        private static extern byte unmanaged_formatCycloneMemorySpace(uint cycloneHandle, uint selectedMediaType);

        [DllImport(DllFilename, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "eraseCycloneImage")]
        private static extern byte unmanaged_eraseCycloneImage(uint cycloneHandle, uint imageId);

        /* Public Methods */
        [DllImport(DllFilename, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "enumerateAllPorts")]
        public static extern void enumerateAllPorts();

        [DllImport(DllFilename, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "disconnectFromAllCyclones")]
        public static extern void disconnectFromAllCyclones();

        [DllImport(DllFilename, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "queryNumberOfAutodetectedCyclones")]
        public static extern uint queryNumberOfAutodetectedCyclones();

        [DllImport(DllFilename, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "checkCycloneExecutionStatus")]
        public static extern uint checkCycloneExecutionStatus(uint cycloneHandle);

        [DllImport(DllFilename, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "getNumberOfErrors")]
        public static extern uint getNumberOfErrors(uint cycloneHandle);

        [DllImport(DllFilename, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "getErrorCode")]
        public static extern int getErrorCode(uint cycloneHandle, uint errorNum);

        [DllImport(DllFilename, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "getLastErrorAddr")]
        public static extern uint getLastErrorAddr(uint cycloneHandle);

        [DllImport(DllFilename, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, EntryPoint = "countCycloneImages")]
        public static extern uint countCycloneImages(uint cycloneHandle);

        // It is recommended to use the provided wrapper methods for executing special features that deal with unmanaged data types
        [DllImport(DllFilename, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "cycloneSpecialFeatures")]
        public static extern byte unmanaged_cycloneSpecialFeatures(uint featureNum, byte setFeature, uint paramValue1,
            uint paramValue2, uint paramValue3, IntPtr paramReference1, IntPtr paramReference2);

        public static string Version()
        {
            return Marshal.PtrToStringAnsi(unmanaged_version());
        }

        public static string QueryInformationOfAutodetectedCyclone(int autodetectIndex, int informationType)
        {
            return Marshal.PtrToStringAnsi(unmanaged_queryInformationOfAutodetectedCyclone(autodetectIndex, informationType));
        }

        /// <summary>
        /// 根据名称连接烧写器
        /// </summary>
        /// <param name="nameIpOrPortIdentifier">名称</param>
        /// <returns></returns>
        public static uint ConnectToCyclone(string nameIpOrPortIdentifier)
        {
            var ptr = Marshal.StringToHGlobalAnsi(nameIpOrPortIdentifier);
            try
            {
                return unmanaged_connectToCyclone(ptr);
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
        }

        public static void SetLocalMachineIpNumber(string ipNumber)
        {
            var ptr = Marshal.StringToHGlobalAnsi(ipNumber);
            try
            {
                unmanaged_setLocalMachineIpNumber(ptr);
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
        }

        public static bool StartDynamicDataProgram(
            uint cycloneHandle, uint targetAddress, uint dataLength, byte[] managedByteArray)
        {
            var size = managedByteArray.Length;
            var unmanagedByteArrayPtr = Marshal.AllocHGlobal(size);
            Marshal.Copy(managedByteArray, 0, unmanagedByteArrayPtr, managedByteArray.Length);
            try
            {
                var unmanagedBool = unmanaged_startDynamicDataProgram(cycloneHandle, targetAddress, dataLength, unmanagedByteArrayPtr);
                return !unmanagedBool.Equals(0);
            }
            finally
            {
                Marshal.FreeHGlobal(unmanagedByteArrayPtr);
            }
        }

        public static byte[] DynamicReadBytes(
            uint cycloneHandle, uint targetAddress, uint dataLength)
        {
            //This function returns a managed byte array of size dataLength with data read from memory starting at targetAddress
            //If there is any error while reading memory, return a null.
            var unmanagedByteArrayPtr = Marshal.AllocHGlobal((int)dataLength);
            var managedByteArray = new byte[dataLength];
            try
            {
                var unmanagedBool = unmanaged_dynamicReadBytes(cycloneHandle, targetAddress, dataLength, unmanagedByteArrayPtr);
                if (unmanagedBool.Equals(0))
                {
                    return null;
                }
                else
                {
                    Marshal.Copy(unmanagedByteArrayPtr, managedByteArray, 0, (int)dataLength);
                    return managedByteArray;
                }
            }
            finally
            {
                Marshal.FreeHGlobal(unmanagedByteArrayPtr);
            }
        }

        public static string GetDescriptionOfErrorCode(uint cycloneHandle, uint errorCode)
        {
            return Marshal.PtrToStringAnsi(unmanaged_getDescriptionOfErrorCode(cycloneHandle, errorCode));
        }

        public static string GetImageDescription(uint cycloneHandle, uint imageId)
        {
            return Marshal.PtrToStringAnsi(unmanaged_getImageDescription(cycloneHandle, imageId));
        }

        public static bool CompareImageInCycloneWithFile(uint cycloneHandle, string aFile, uint imageId)
        {
            var aFilePtr = Marshal.StringToHGlobalAnsi(aFile);
            try
            {
                var unmanagedBool = Convert.ToByte(unmanaged_compareImageInCycloneWithFile(cycloneHandle, aFilePtr, imageId));
                return !unmanagedBool.Equals(0);
            }
            finally
            {
                Marshal.FreeHGlobal(aFilePtr);
            }
        }

        public static uint AddCycloneImage(uint cycloneHandle, uint selectedMediaType, bool replaceImageOfSameDescription, string aFile)
        {
            var aFilePtr = Marshal.StringToHGlobalAnsi(aFile);
            try
            {
                var unmanagedBool = (byte) (!replaceImageOfSameDescription ? 0 : 1);
                return unmanaged_addCycloneImage(cycloneHandle, selectedMediaType, unmanagedBool, aFilePtr);
            }
            finally
            {
                Marshal.FreeHGlobal(aFilePtr);
            }
        }

        public static string GetPropertyValue(uint cycloneHandle, uint resourceOrImageId, string categoryName, string propertyName)
        {
            var categoryNamePtr = Marshal.StringToHGlobalAnsi(categoryName);
            var propertyNamePtr = Marshal.StringToHGlobalAnsi(propertyName);
            try
            {
                return Marshal.PtrToStringAnsi(unmanaged_getPropertyValue(cycloneHandle, resourceOrImageId, categoryNamePtr, propertyNamePtr));
            }
            finally
            {
                Marshal.FreeHGlobal(categoryNamePtr);
                Marshal.FreeHGlobal(propertyNamePtr);
            }
        }

        public static bool SetPropertyValue(uint cycloneHandle, uint resourceOrImageId, string categoryName, string propertyName, string newValue)
        {
            var categoryNamePtr = Marshal.StringToHGlobalAnsi(categoryName);
            var propertyNamePtr = Marshal.StringToHGlobalAnsi(propertyName);
            var newValuePtr = Marshal.StringToHGlobalAnsi(newValue);
            try
            {
                var unmanagedBool = unmanaged_setPropertyValue(cycloneHandle, resourceOrImageId, categoryNamePtr, propertyNamePtr, newValuePtr);
                return !unmanagedBool.Equals(0);
            }
            finally
            {
                Marshal.FreeHGlobal(categoryNamePtr);
                Marshal.FreeHGlobal(propertyNamePtr);
                Marshal.FreeHGlobal(newValuePtr);
            }
        }

        public static string GetPropertyList(uint cycloneHandle, uint resourceOrImageId, string categoryName)
        {
            var categoryNamePtr = Marshal.StringToHGlobalAnsi(categoryName);
            try
            {
                return Marshal.PtrToStringAnsi(unmanaged_getPropertyList(cycloneHandle, resourceOrImageId, categoryNamePtr));
            }
            finally
            {
                Marshal.FreeHGlobal(categoryNamePtr);
            }
        }

        public static string GetFirmwareVersion(uint cycloneHandle)
        {
            return Marshal.PtrToStringAnsi(unmanaged_getFirmwareVersion(cycloneHandle));
        }

        public static bool StartImageExecution(uint cycloneHandle, byte imageId)
        {
            var unmanagedBool = unmanaged_startImageExecution(cycloneHandle, imageId);
            return !unmanagedBool.Equals(0);
        }

        public static bool ResetCyclone(uint cycloneHandle, uint resetDelayInMs)
        {
            var unmanagedBool = unmanaged_resetCyclone(cycloneHandle, resetDelayInMs);
            return !unmanagedBool.Equals(0);
        }

        public static bool FormatCycloneMemorySpace(uint cycloneHandle, uint selectedMediaType)
        {
            var unmanagedBool = unmanaged_formatCycloneMemorySpace(cycloneHandle, selectedMediaType);
            return !unmanagedBool.Equals(0);
        }

        public static bool EraseCycloneImage(uint cycloneHandle, uint imageId)
        {
            var unmanagedBool = unmanaged_eraseCycloneImage(cycloneHandle, imageId);
            return !unmanagedBool.Equals(0);
        }

        public static bool CycloneSpecialFeaturesGetImageDescriptionFromImageFile(string imageFileName, ref string resultString)
        {
            var tmpPtrToPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(IntPtr)));
            var filePtr = Marshal.StringToHGlobalAnsi(imageFileName);
            try
            {
                const byte falseValue = 0;
                var unmanagedBool = unmanaged_cycloneSpecialFeatures(CycloneGetImageDescriptionFromFile, falseValue, 0, 0, 0, tmpPtrToPtr, filePtr);
                resultString = Marshal.PtrToStringAnsi((IntPtr)Marshal.PtrToStructure(tmpPtrToPtr, typeof(IntPtr)));
                return !unmanagedBool.Equals(0);
            }
            finally
            {
                Marshal.FreeHGlobal(tmpPtrToPtr);
                Marshal.FreeHGlobal(filePtr);
            }
        }

        public static bool CycloneSpecialFeaturesGetImageCrc32FromFile(string imageFileName, ref uint numCrc32)
        {
            var tmpPtrToPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(uint)));
            var filePtr = Marshal.StringToHGlobalAnsi(imageFileName);
            const byte falseValue = 0;
            try
            {
                var unmanagedBool = unmanaged_cycloneSpecialFeatures(CycloneGetImageCrc32FromFile, falseValue, 0, 0, 0, tmpPtrToPtr, filePtr);
                numCrc32 = (uint)Marshal.ReadInt32(tmpPtrToPtr); //read the CRC32 value from the unmanaged pointer
                //stringCRC32 = numCRC32.ToString("X8"); //convert to hex string
                return !unmanagedBool.Equals(0);
            }
            finally
            {
                Marshal.FreeHGlobal(tmpPtrToPtr);
                Marshal.FreeHGlobal(filePtr);
            }
        }

        public static bool CycloneSpecialFeaturesGetImageScriptFileFromImageFile(string imageFileName, string scriptFile)
        {
            var scriptFilePtr = Marshal.StringToHGlobalAnsi(scriptFile);
            var imageFileNamePtr = Marshal.StringToHGlobalAnsi(imageFileName);
            try
            {
                const byte falseValue = 0;
                var unmanagedBool = unmanaged_cycloneSpecialFeatures(CycloneGetImageScriptFileFromFile, falseValue, 0, 0, 0, scriptFilePtr, imageFileNamePtr);
                return !unmanagedBool.Equals(0);
            }
            finally
            {
                Marshal.FreeHGlobal(scriptFilePtr);
                Marshal.FreeHGlobal(imageFileNamePtr);
            }
        }

        public static bool CycloneSpecialFeaturesTogglePowerNoDebug()
        {
            const byte falseValue = 0;
            var unmanagedBool = unmanaged_cycloneSpecialFeatures(CycloneTogglePowerNoDebug, falseValue, 0, 0, 0, IntPtr.Zero, IntPtr.Zero);
            return !unmanagedBool.Equals(0);
        }

        public static bool CycloneSpecialFeaturesSetActiveSecurityCode(uint length, byte[] securityBytesArray, string securityCodeType)
        {
            var size = securityBytesArray.Length;
            var unmanagedSecurityBytesArrayPtr = Marshal.AllocHGlobal(size);
            Marshal.Copy(securityBytesArray, 0, unmanagedSecurityBytesArrayPtr, securityBytesArray.Length);
            var securityCodeTypePtr = Marshal.StringToHGlobalAnsi(securityCodeType);
            try
            {
                const byte trueValue = 1;
                var unmanagedBool = unmanaged_cycloneSpecialFeatures(CycloneSetActiveSecurityCode, trueValue, 0, length,
                    0, unmanagedSecurityBytesArrayPtr, securityCodeTypePtr);
                return !unmanagedBool.Equals(0);
            }
            finally
            {
                Marshal.FreeHGlobal(unmanagedSecurityBytesArrayPtr);
                Marshal.FreeHGlobal(securityCodeTypePtr);
            }
        }
    }
}
