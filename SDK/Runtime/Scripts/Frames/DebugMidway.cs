#if UNITY_EDITOR

using UnityEngine;
using System;
using System.IO;
namespace RayNeo
{

    public class DebugMidway
    {
        static DebugMidway()
        {
            Init();
        }

        //ÊÇ·ñÔÚÄ£Äâ.
        public static bool IsInSimulator
        {
            get
            {
                return m_cfg.m_IsInSimulator;
            }
            set
            {
                m_cfg.m_IsInSimulator = value;
                OnSimulatorChange?.Invoke(m_cfg.m_IsInSimulator);
                ChangeConfig();
            }
        }

        public static RayNeo.API.DeviceType GetDeviceType()
        {
            return (RayNeo.API.DeviceType)m_cfg.CurrentDeviceType;
        }
        public static void SetDeviceType(API.DeviceType value)
        {
            m_cfg.CurrentDeviceType = (int)value;
            OnDeviceTypeChange?.Invoke(value);
            ChangeConfig();
        }

        //public static bool IsInSimulator
        //{= fa
        //    get
        //    {
        //        return m_cfg.m_IsInSimulator;
        //    }
        //    set
        //    {
        //        m_cfg.m_IsInSimulator = value;
        //        OnSimulatorChange?.Invoke(m_cfg.m_IsInSimulator);
        //        ChangeConfig();
        //    }
        //}

        public static Action<bool> OnSimulatorChange;
        public static Action<API.DeviceType> OnDeviceTypeChange;

        private static DebugConfig m_cfg;

        private static string CFG_PATH = Application.temporaryCachePath + "/debugMiday.json";
        public static void Init()
        {
            if (m_cfg != null)
            {
                return;
            }

            if (File.Exists(CFG_PATH))
            {
                m_cfg = JsonUtility.FromJson<DebugConfig>(File.ReadAllText(CFG_PATH));
            }
            else
            {
                m_cfg = new DebugConfig();
                ChangeConfig();
            }
        }

        public static void ChangeConfig()
        {
            File.WriteAllText(CFG_PATH, JsonUtility.ToJson(m_cfg));
        }

    }

    public class DebugConfig
    {
        public bool m_IsInSimulator = false;
        //public bool m_Is
        public int CurrentDeviceType = (int)RayNeo.API.DeviceType.NONE;
    }


}
#endif
