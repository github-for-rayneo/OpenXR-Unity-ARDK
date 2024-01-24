namespace com.rayneo.xr.extensions
{
    using AOT;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using UnityEditor;
    using UnityEngine;
    using UnityEngine.XR.OpenXR;

    public static class XRInterfaces
    {

        public delegate void FXRStateEventCallback(UInt32 state, UInt64 timestamp, uint length, IntPtr data);
        private static List<FXRStateEventCallback> mStateEventCallbackLists = new List<FXRStateEventCallback>();
        private static float[] m_rotOrientation = new float[4];

        //render & OpenXR Settings
        public static void SetBasicXRConfigs(Dictionary<string, int> settings)
        {
            foreach(var item in settings)
            {
                if(item.Key == "ATW" && item.Value == 1)
                {
                   SendCommand((int)XRControlUnit.kUnitConfiguration, (int)FXRControlCommand.kCtlCmdUseATW);
                }else if(item.Key == "renderMode" && item.Value == 1)
                {
                    SendCommand((int)XRControlUnit.kUnitConfiguration, (int)FXRControlCommand.kCtlCmdUseSinglePass);
                }
                else if(item.Key == "depthSubmissionMode")
                {
                    int[] prop = new int[1];
                    prop[0] = item.Value;
                    IntPtr ptr = Marshal.AllocHGlobal(sizeof(int));
                    Marshal.Copy(prop, 0, ptr, sizeof(int));
                    XRInterfaces.SetProp("depthSubmissionMode", ptr, sizeof(int));
                }
                else
                {
                    Debug.LogError("Unknown command " + item.Key);
                }
            }
        }

        public static void Recenter()
        {
            RayNeoApi_RecenterHeadTracker();
        }

        public static void EnableSlamHeadTracker()
        {
            RayNeoApi_EnableSlamHeadTracker();
        }

        public static void DisableSlamHeadTracker()
        {
            RayNeoApi_DisableSlamHeadTracker();
        }

        public static XRRotation GetNineAxisOrientation()
        {
            RayNeoApi_NineAxisOrientation(m_rotOrientation);
            XRRotation r = new(m_rotOrientation);
            XRMathUtils.RightHand2UnityLeftHand(r);
            return r;
        }

        public static int GetHeadTrackerStatus()
        {
            return RayNeoApi_GetHeadTrackerStatus();
        }

        [MonoPInvokeCallback(typeof(FXRStateEventCallback))]
        private static void StateEventDispatcher(UInt32 state, UInt64 timestamp, uint length, IntPtr data)
        {
            foreach (FXRStateEventCallback item in mStateEventCallbackLists)
            {
                item(state, timestamp, length, data);
            }
        }

        public static bool RegisterStateEventCallback(FXRStateEventCallback callback)
        {
            if (mStateEventCallbackLists.Contains(callback)) return false;
            mStateEventCallbackLists.Add(callback);
            if (mStateEventCallbackLists.Count == 1)
            {
                RayNeoApi_RegisterStateEventCallback(Marshal.GetFunctionPointerForDelegate<FXRStateEventCallback>(StateEventDispatcher));
            }
            return true;
        }

        public static bool UnregisterStateEventCallback(FXRStateEventCallback callback)
        {
            if (!mStateEventCallbackLists.Contains(callback)) return false;
            mStateEventCallbackLists.Remove(callback);
            if (mStateEventCallbackLists.Count == 0)
            {
                RayNeoApi_UnregisterStateEventCallback(Marshal.GetFunctionPointerForDelegate<FXRStateEventCallback>(StateEventDispatcher));
            }
            return true;
        }

        public static int SendCommand(int unit, int command)
        {
            return RayNeoApi_SendCommand(unit, command);
        }

        public static void EnablePlaneDetection()
        {
            RayNeoApi_EnablePlaneDetection();
        }

        public static void DisablePlaneDetection()
        {
            RayNeoApi_DisablePlaneDetection();
        }

        public static int GetPlaneInfo(XRPlaneInfo[] info, int arraySize)
        {
            int len = RayNeoApi_GetPlaneInfo(info, arraySize);
            for (int i = 0; i < len; i++)
            {
                XRMathUtils.RightHand2UnityLeftHand(info[i].pose.rotation, info[i].pose.position);
            }
            return len;
        }

        public static int SetProp(string item, IntPtr value, int len)
        {
            return RayNeoApi_SetProp(item, value, len);
        }

        public static int GetProp(string item, [In, Out] IntPtr value, ref int len)
        {
            return RayNeoApi_GetProp(item, value, ref len);
        }


        public static int DeviceType()
        {
            return 0x1000;
        }

        public static int HostType()
        {
            return -1;
        }




        /*
         *  Symbols import from libFFalconXRInterfaces.so
         */
        [DllImport(XRConstants.XRInterfaces)]
        private static extern void RayNeoApi_RecenterHeadTracker();

        [DllImport(XRConstants.XRInterfaces)]
        private static extern void RayNeoApi_EnableSlamHeadTracker();

        [DllImport(XRConstants.XRInterfaces)]
        private static extern void RayNeoApi_DisableSlamHeadTracker();

        [DllImport(XRConstants.XRInterfaces)]
        private static extern void RayNeoApi_NineAxisOrientation(float[] orientation);

        [DllImport(XRConstants.XRInterfaces)]
        public static extern float RayNeoApi_NineAxisAzimuth();

        [DllImport(XRConstants.XRInterfaces)]
        private static extern int RayNeoApi_GetHeadTrackerStatus();

        [DllImport(XRConstants.XRInterfaces)]
        private static extern void RayNeoApi_RegisterStateEventCallback(IntPtr callback);

        [DllImport(XRConstants.XRInterfaces)]
        private static extern void RayNeoApi_UnregisterStateEventCallback(IntPtr callback);

        [DllImport(XRConstants.XRInterfaces)]
        private static extern int RayNeoApi_SendCommand(int unit, int command);

        [DllImport(XRConstants.XRInterfaces)]
        private static extern void RayNeoApi_EnablePlaneDetection();

        [DllImport(XRConstants.XRInterfaces)]
        private static extern void RayNeoApi_DisablePlaneDetection();

        [DllImport(XRConstants.XRInterfaces, CallingConvention = CallingConvention.Cdecl)]
        private static extern int RayNeoApi_GetPlaneInfo([In, Out] XRPlaneInfo[] info, int arraySize);

        [DllImport(XRConstants.XRInterfaces, CallingConvention = CallingConvention.Cdecl)]
        private static extern int RayNeoApi_SetProp(string item, IntPtr value, int len);

        [DllImport(XRConstants.XRInterfaces, CallingConvention = CallingConvention.Cdecl)]
        private static extern int RayNeoApi_GetProp(string item, [In, Out] IntPtr value, ref int len);


        #region sharecamera
        [DllImport(XRConstants.XRInterfaces)]
        public static extern void RayneoApi_OpenCamera();

        [DllImport(XRConstants.XRInterfaces)]

        public static extern void RayNeoApi_CloseCamera();

        [DllImport(XRConstants.XRInterfaces)]

        public static extern void RayneoApi_GetLatestFrame(byte[] data, int w, int h);
        #endregion
    }


}