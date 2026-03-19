using CommonUtility.MyCameraSdk.Common.Enum;
using CommonUtility.MyCameraSdk.Mode;
using System.Collections.Generic;

namespace CommonUtility.MyCameraSdk
{
    public class CamFactory
    {
        private static List<ICamera> CameraList { get; set; } = new List<ICamera>();


        public CamFactory()
        {
            if (CameraList == null)
            {
                CameraList = new List<ICamera>();
            }
        }

        public static List<string> GetDeviceEnum(CameraBrand brand)
        {
            ICamera camera = null;
            switch (brand)
            {
                case CameraBrand.DaHeng:
                    camera = new DHCamera();
                    break;
                case CameraBrand.HIK:
                    camera = new HKCamera();
                    break;
            }

            return camera?.GetListEnum();
        }

        public static ICamera CreateCamera(CameraBrand brand)
        {
            ICamera camera = null;
            switch (brand)
            {
                case CameraBrand.DaHeng:
                    camera = new DHCamera();
                    break;
                case CameraBrand.HIK:
                    camera = new HKCamera();
                    break;
            }

            CameraList.Add(camera);
            return camera;
        }

        public static ICamera GetItem(string CamSN)
        {
            ICamera result = null;
            if (CameraList.Count < 1)
            {
                return result;
            }

            foreach (ICamera camera in CameraList)
            {
                if ((camera as BaseCamera).SN.Equals(CamSN))
                {
                    result = camera;
                    break;
                }
            }

            return result;
        }

        public static void DestroyCamera(ICamera decamera)
        {
            CameraList?.Remove(decamera);
            decamera?.CloseDevice();
        }

        public static void DestroyAll()
        {
            if (CameraList.Count < 1)
            {
                return;
            }

            foreach (ICamera camera in CameraList)
            {
                camera?.CloseDevice();
            }

            CameraList?.Clear();
        }
    }
}
