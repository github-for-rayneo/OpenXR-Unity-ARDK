using RayNeo.API;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.XR.OpenXR.Features.Interactions.RayNeoControllerProfile;

namespace RayNeo
{

    /// <summary>
    /// 戒指
    /// </summary>
    public static class RingManager
    {
        private static AndroidJavaObject m_ringManager;

        private static AndroidJavaObject Ins
        {
            get
            {
                if (m_ringManager == null)
                {

                    var ajc = new AndroidJavaClass(PlatformAndroid.SupportPackagePath + ".ipc.ring.RingManager");
                    m_ringManager = ajc.CallStatic<AndroidJavaObject>("ins");
                }

                return m_ringManager;
            }
        }
        private static bool m_ringOpened = false;
        private static RingInputDevice m_device;
        private static RingListener m_listener;

        public static bool RingOpened => m_ringOpened;

        public static void OpenRing()
        {
            if (m_ringOpened)
            {
                return;
            }
            m_ringOpened = true;
            m_device = InputSystem.AddDevice<RingInputDevice>();
            m_listener = new RingListener();
            if (!Application.isEditor)
            {
                Ins.Call("RegCallBack", m_listener);
                Ins.Call("OpenRing");
            }


        }
        public static void OpenRing(RingTouchType type, bool defaultLongClickEnable)
        {
            OpenRing();
            SwitchTouchType(type);
            SetRingLongClickEnalbe(defaultLongClickEnable);
        }

        //切换类型
        public static void SwitchTouchType(RingTouchType type)
        {
            if (Application.isEditor || !m_ringOpened)
            {
                return;
            }
            Ins.Call("SwitchTouchType", (int)type);
        }
        /// <summary>
        /// long click enable
        /// </summary>
        /// <param name="enable"></param>
        public static void SetRingLongClickEnalbe(bool enable)
        {
            if (Application.isEditor || !m_ringOpened)
            {
                return;
            }
            Ins.Call("SetRingLongClickEnalbe", enable);
        }

        public static RingTouchType GetTouchType()
        {
            if (Application.isEditor || !m_ringOpened)
            {
                return RingTouchType.CustomTouchEvent;
            }
            return (RingTouchType)Ins.Call<int>("GetTouchType");
        }

        public static void CloseRing()
        {

            if (!Application.isEditor && m_ringOpened)
            {
                Ins.Call("closeRing");
                Ins.Call("RegCallBack", null);

                var customDevice = InputSystem.devices.FirstOrDefault(x => x is RingInputDevice);
                if (customDevice != null)
                    InputSystem.RemoveDevice(customDevice);
                m_device = null;
            }
            m_ringOpened = false;
        }


#if UNITY_EDITOR
        public static void OnTouchPadHeavyPressChange(bool pressed)
        {
            if (m_listener == null)
            {
                return;
            }
            m_listener.OnTouchPadPressChange(pressed);
        }
        public static void OnTouch(int eventType, float x, float y)
        {
            if (m_listener == null)
            {
                return;
            }
            m_listener.OnTouch(eventType, x, y);
        }
        public static void OnRotation(Quaternion q)
        {
            if (m_listener == null)
            {
                return;
            }
            m_listener.OnRotation(q.x, q.y, q.z, q.w);

        }
#endif


        public class RingListener : AndroidJavaProxy
        {


            private bool m_ringBtnDown = false;//有touch了就默认true了.
            private bool m_ringHeavyBtnDown = false;//重击
            private RingTouchMotionEventType m_lastEvent = RingTouchMotionEventType.NONE;

            public RingListener() : base(PlatformAndroid.SupportPackagePath + ".ipc.ring.RingListener")
            {
            }
            public void OnTouch(int eventType, float x, float y)
            {
                RingTouchMotionEventType et = (RingTouchMotionEventType)eventType;
                if (et == RingTouchMotionEventType.ACTION_DOWN || et == RingTouchMotionEventType.ACTION_POINTER_DOWN)
                {
                    m_ringBtnDown = true;

                }
                else if (et != RingTouchMotionEventType.ACTION_MOVE)
                {
                    //m_ringBtnDown = false;
                    //if (m_ringHeavyBtnDown)
                    //{
                    //    m_ringHeavyBtnDown = false;
                    //    m_device.OnHeavyPressStateChagne(false);
                    //}
                    m_ringBtnDown = false;
                }

                m_device.UpdatePos(x, y, 0, m_ringBtnDown);
                m_lastEvent = et;
            }



            public void OnTouchPadPressChange(bool press)
            {
                m_device.OnHeavyPressStateChagne(press);

            }
            public void OnRotation(float x, float y, float z, float w)
            {
                if (m_device == null)
                {
                    return;
                }
                var rot = new Quaternion(x, y, z, w);
                m_device.UpdateQuaternion(rot);
            }

        }
    }


    public enum RingTouchMotionEventType
    {
        NONE = -1,
        ACTION_CANCEL = 3,
        ACTION_DOWN = 0,
        ACTION_MOVE = 2,
        ACTION_UP = 1,
        ACTION_POINTER_DOWN = 5,
        ACTION_POINTER_UP = 6


    }

    public enum RingTouchType
    {
        UnityTouchEvent = 0,//走unity默认的事件
        CustomTouchEvent = 1,//自定义touch事件
        BothTouchEvent = 2,//两个事件都走.
    }


}
