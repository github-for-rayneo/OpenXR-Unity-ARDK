using com.rayneo.xr.extensions;
using System;
using UnityEngine;
namespace RayNeo.API
{
    public class RayNeoInfo
    {
        public static RayNeo.API.DeviceType CurrentDeviceType()
        {
#if UNITY_EDITOR
            if (DebugMidway.IsInSimulator)
            {
                return DebugMidway.GetDeviceType();
            }
#endif

            return (RayNeo.API.DeviceType)XRInterfaces.DeviceType();
        }

        /// <summary>
        /// 眼镜是否分体
        /// </summary>
        /// <returns></returns>
        public static bool DeviceIsSplitType()
        {
            int devType = (int)CurrentDeviceType();
            if (devType <= (int)BB_END && devType >= (int)BB_START)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 眼镜是否一体
        /// </summary>
        /// <returns></returns>
        public static bool DeviceIsIntegratedType()
        {
            int devType = (int)CurrentDeviceType();
            if (devType <= X_END && devType >= X_START)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 是否插入设备
        /// </summary>
        /// <returns></returns>
        public static bool HasDevice()
        {
            return CurrentDeviceType() >= 0;
        }

        /// <summary>
        /// 魔盒等设备.
        /// </summary>
        /// <returns></returns>
        public static bool HasBox()
        {
            int boxType = (int)CurrentBoxType();
            if (boxType <= (int)PlatformType.BOX_END && boxType >= (int)PlatformType.BOX_START)
            {
                return true;
            }
            return false;
        }
        public static PlatformType CurrentBoxType()
        {
            return (PlatformType)XRInterfaces.HostType();
        }

        private const int BB_START = 0x0020;//start
        private const int BB_END = 0x00FF;//end
        private const int X_START = 0x1000;//start
        private const int X_END = 0x10FF;//end
    }

    public enum DeviceType
    {
        NONE = -0x0001,
        //BB
        NextViewPro = 0x0020,  //一代眼镜nextviewpro
        AIR_PLUS_Normal = 0x0021,  //二代眼镜
        AIR_PLUS_15_Seeya = 0x0022, //1.5 Seeya屏
        AIR_PLUS_15_Sony = 0x0023,  //1.5 Sony屏
        AIR_PLUS_18 = 0x0024,      //1.8
        AIR2_Taurus = 0x0030,        //金牛

        //一体
        X2_Normal = 0x1000,//
    }

    public enum PlatformType
    {
        NONE = -0x0001,
        BOX_START = 0xF0FF,//魔盒 开始
        BOX_Normal = 0xF020,//
        BOX_END = 0xF020,//
    }


}
