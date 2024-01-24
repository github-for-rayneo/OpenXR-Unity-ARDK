using com.rayneo.xr.extensions;
using System;
using UnityEngine;
namespace RayNeo.API
{

    /// <summary>
    /// 获取当前Activity
    /// </summary>
    public static class PlatformAndroid
    {
        public const string PackageRootPath = "com.rayneo.openxradapter";

        public const string SupportPackagePath = PackageRootPath + ".support";
        private const string SystemMonitoringManagerClass = SupportPackagePath + ".systemmonitoring.SystemMonitoringManager";
        private const string RayNeoHelperClass = SupportPackagePath + ".RayNeoHelper";
        private static AndroidJavaObject m_CurAct;
        private static AndroidJavaObject m_Moitoring;
        private static AndroidJavaClass m_Helper;
        private static AndroidJavaObject m_ExternalSrcInterface = null;

        /// <summary>
        /// 获取当前运行的activity
        /// </summary>
        public static AndroidJavaObject CurActivity
        {
            get
            {
                if (m_CurAct == null)
                {
                    AndroidJavaClass unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                    m_CurAct = unityClass.GetStatic<AndroidJavaObject>("currentActivity");
                }

                return m_CurAct;
            }
        }
        public static AndroidJavaObject MonitoringManager
        {
            get
            {
                if (m_Moitoring == null)
                {
                    AndroidJavaClass unityClass = new AndroidJavaClass(SystemMonitoringManagerClass);
                    m_Moitoring = unityClass.CallStatic<AndroidJavaObject>("ins");
                }

                return m_Moitoring;
            }
        }

        public static AndroidJavaClass JavaHelper
        {
            get
            {
                if (m_Helper == null)
                {
                    m_Helper = new AndroidJavaClass(RayNeoHelperClass);
                }
                return m_Helper;
            }
        }
        /// <summary>
        /// 获取当前ApplicationContext
        /// </summary>

        public static AndroidJavaObject ApplicationContext
        {
            get
            {
                return CurActivity.Call<AndroidJavaObject>("getApplicationContext");
            }
        }
        public static AndroidJavaObject ExternalSrcInterface
        {
            get
            {
                if (m_ExternalSrcInterface == null)
                {
                    m_ExternalSrcInterface = new AndroidJavaObject("com.ffalcon.externalSrcMgr.ExternalSrcHelper");
                }
                return m_ExternalSrcInterface;
            }
        }


        public static void OpenSystemMonitoring()
        {
            MonitoringManager.Call("openSystemMonitoring", 0L);
        }

        public static void CloseSystemMonitoring()
        {
            MonitoringManager.Call("closeSystemMonitoring");
        }

        public static float GetScreenBrightness()
        {
            return JavaHelper.CallStatic<float>("getScreenBrightness");
        }

        public static void SetScreenBrightness(float percent)
        {
            JavaHelper.CallStatic("setScreenBrightness", percent);
        }

        public static float GetGlobalCpuTemperature()
        {
            return JavaHelper.CallStatic<float>("getGlobalCpuTemperature");
        }

        /// <summary>
        /// 系统监控信息开启的枚举
        /// </summary>
        public class SystemMonitoringInfoType
        {

            //显示全部.
            public const long FULL = 0;

            //电流
            public const long ELECTRIC = 1 << 0;
            //平均电流
            public const long AVERAGE_ELECTRIC = 1 << 1;
            //当前电量
            public const long BATTERY_CAPACTIY = 1 << 2;

            //CPU热量
            public const long CPU_TEMPERATURE = 1 << 3;

            //CPU使用率
            public const long CPU_USAGE = 1 << 4;

            //内存使用
            public const long MEM_USAGE = 1 << 5;

            //当前顶层应用CPU使用
            public const long CUR_TOP_USAGE = 1 << 6;
        }
    }
}
