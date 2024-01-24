using UnityEngine;
using FfalconXR;
using System.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using RayNeo.API;
#if UNITY_EDITOR

using UnityEditor;
#endif

using Unity.Collections;

namespace RayNeo
{

    //[CreateAssetMenu(fileName = "RayNeoGeneralSettings", menuName = "RayNeo/RayNeo")]
    public class RayNeoXRGeneralSettings : ScriptableObject
    {

        internal static RayNeoXRGeneralSettings s_RuntimeSettingsInstance = null;
        public static RayNeoXRGeneralSettings Instance
        {
            get
            {
#if UNITY_EDITOR
                if (s_RuntimeSettingsInstance == null)
                {
                    return null;
                }
#endif
                if (!s_RuntimeSettingsInstance.m_Inited)
                {
                    s_RuntimeSettingsInstance.Init();
                    s_RuntimeSettingsInstance.m_Inited = true;
                }
                return s_RuntimeSettingsInstance;
            }
#if UNITY_EDITOR
            set
            {
                s_RuntimeSettingsInstance = value;
                s_RuntimeSettingsInstance.Init();
                s_RuntimeSettingsInstance.m_Inited = true;
            }
#endif
        }
        [SerializeField]
        public List<TouchConfig> touchConfigs;
        [SerializeField]
        public BaseConfig baseConfig;
        [SerializeField]
        private List<DeviceInfo> m_devices = new List<DeviceInfo>();
        [NonSerialized]
        private bool m_Inited = false;
        public RayNeoXRGeneralSettings()
        {
            s_RuntimeSettingsInstance = this;
        }

        #region runtime
        [NonSerialized]
        private Dictionary<RayNeo.API.DeviceType, TouchConfig> m_touchConfigDict = new Dictionary<RayNeo.API.DeviceType, TouchConfig>();

        public TouchConfig SimpleTouchConfig
        {
            get
            {
                return m_touchConfigDict[RayNeoInfo.CurrentDeviceType()];
            }
        }
        public List<DeviceInfo> Devices
        {
            get => m_devices;
        }

        private void Init()
        {
            for (int i = 0; i < touchConfigs.Count; i++)
            {
                m_touchConfigDict[touchConfigs[i].PlatformType] = touchConfigs[i];
            }
        }
        public TouchConfig GetSimpleTouchConfig(RayNeo.API.DeviceType t)
        {
            return m_touchConfigDict[t];
        }
        #endregion


#if UNITY_EDITOR


        public void CreateDefaultData()
        {
            if (touchConfigs == null)
            {
                touchConfigs = new List<TouchConfig>();
            }
            //TouchConfigs = new List<TouchConfig>();
            Array ev = Enum.GetValues(typeof(RayNeo.API.DeviceType));
            for (int i = 0; i < ev.Length; i++)
            {
                bool digout = false;
                foreach (var item in touchConfigs)
                {
                    if (item.PlatformType == (RayNeo.API.DeviceType)ev.GetValue(i))
                    {
                        digout = true;
                        break;
                    }
                }

                if (!digout)
                {
                    touchConfigs.Add(new TouchConfig()
                    {
                        PlatformType = (RayNeo.API.DeviceType)ev.GetValue(i)
                    }); ;
                }
            }

            m_devices.Add(new DeviceInfo { type = RayNeo.API.DeviceType.NextViewPro, DeviceName = "NextViewPro", FOV = 12.9f + 8.7f });
            m_devices.Add(new DeviceInfo { type = RayNeo.API.DeviceType.AIR_PLUS_Normal, DeviceName = "Air Plus", FOV = 12.9f + 8.7f });
            m_devices.Add(new DeviceInfo { type = RayNeo.API.DeviceType.AIR_PLUS_15_Seeya, DeviceName = "Air Plus(Seeya)", FOV = 12.9f + 8.7f });
            m_devices.Add(new DeviceInfo { type = RayNeo.API.DeviceType.AIR_PLUS_15_Sony, DeviceName = "Air Plus(Sony)", FOV = 12.9f + 8.7f });
            m_devices.Add(new DeviceInfo { type = RayNeo.API.DeviceType.AIR_PLUS_18, DeviceName = "Air Plus 1.8", FOV = 11.9f + 11.9f });
            m_devices.Add(new DeviceInfo { type = RayNeo.API.DeviceType.AIR2_Taurus, DeviceName = "Air Plus 2", FOV = 11.2f + 11.2f });
            m_devices.Add(new DeviceInfo { type = RayNeo.API.DeviceType.X2_Normal, DeviceName = "X2", FOV = 8.33972f * 2, Width = 640, Height = 480 });

        }


        //private string TempConfigPath = Application.temporaryCachePath + "/debugConfig.json";
        //public class TempConfigInfo
        //{
        //    public int deviceType;
        //}

        //public TempConfigInfo tempInfo { get; private set; }
        //public TempConfigInfo ReadTempConfig()
        //{
        //    if (tempInfo == null && File.Exists(TempConfigPath))
        //    {
        //        tempInfo = JsonUtility.FromJson<TempConfigInfo>(File.ReadAllText(TempConfigPath));
        //    }
        //    if (tempInfo == null)
        //    {
        //        tempInfo = new TempConfigInfo();
        //        WriteTempConfig(tempInfo);
        //    }
        //    return tempInfo;
        //}

        //public void WriteTempConfig(TempConfigInfo info)
        //{
        //    File.WriteAllText(TempConfigPath, JsonUtility.ToJson(info));
        //}
#endif

    }



    [Serializable]
    public class TouchConfig
    {
        //[Disable]
        [SerializeField]
        public RayNeo.API.DeviceType PlatformType = RayNeo.API.DeviceType.X2_Normal;
        //双击.   间隔.
        [Range(0, 1)]
        public float MulitTapSpacing = 0.4f;//走配置.

        //按下到抬起.如果产生了超过该像素的滑动.那么判定为滑动, 不执行点击.
        public uint MovingThreshold = 10;//走配置
        public uint SwipeEndThreshold = 100;//调用swipeEnd的阈值.
        public bool ReleaseSimpleTapImmediately = false;//如果为true.双击则会先有个单击回调.再有双击
        public bool ReleaseLastTapOnChangeToMove = true;//单击后.手指放下滑动.  立刻执行单击事件.
    }
    [Serializable]
    public class BaseConfig
    {
        public LogLevel LogLevel = LogLevel.NoLog;

        //public int TargetFrameRate = 60;

        public int SleepTimeOut = SleepTimeout.NeverSleep;
    }
    [Serializable]
    public class DeviceInfo
    {
        public string DeviceName;
        public RayNeo.API.DeviceType type;
        public int Width = 1920;
        public int Height = 1080;
        public float FOV = 15.1f;
    }
    //public class DisableAttribute : PropertyAttribute
    //{
    //}
}
