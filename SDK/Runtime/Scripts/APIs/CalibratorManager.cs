
using com.rayneo.xr.extensions;
using System;
using System.Runtime.InteropServices;

namespace RayNeo
{
    public class XRCalibratorHeadInfo
    {
        //Euler
        public float HeadYawMax;
        public float HeadYawMin;
        public float HeadPitchMax;
        public float HeadPitchMin;
        public float HeadRollMax;
        public float HeadRollMin;
    }

    /// <summary>
    /// ¥≈¡¶º∆.
    /// </summary>
    public static class CalibratorManager
    {
        public static Action<XRCalibratorHeadInfo> OnDoingCalibration;

        public static Action OnNeedCalibrate;
        public static Action OnCalibrateSuccess;
        public static Action OnCalibrateFailed;
        public static void RegisterState()
        {
            XRInterfaces.RegisterStateEventCallback(DelegatedMethod);
        }

        public static void UnregisterState()
        {
            XRInterfaces.UnregisterStateEventCallback(DelegatedMethod);
        }

        public static void Start()
        {
            XRInterfaces.SendCommand((int)XRControlUnit.kUnitHeadTracker, (int)FXRControlCommand.kCtlCmdStartMagCalibration);
        }
        public static void Stop()
        {
            XRInterfaces.SendCommand((int)XRControlUnit.kUnitHeadTracker, (int)FXRControlCommand.kCtlCmdStopMagCalibration);
        }

        private static void DelegatedMethod(UInt32 state, UInt64 timestamp, uint length, IntPtr data)
        {
            switch (state)
            {
                case ((UInt32)XRStateEvent.kStateMagNeedCalibrate):
                    OnNeedCalibrate?.Invoke();
                    break;
                case ((UInt32)XRStateEvent.kStateMagDoingCalibrate):
                    {
                        XRCalibratorHeadInfo info = new XRCalibratorHeadInfo();
                        byte[] byteArray = new byte[length];
                        Marshal.Copy(data, byteArray, 0, (int)length);
                        info.HeadYawMax = BitConverter.ToSingle(byteArray, 0);
                        info.HeadYawMin = BitConverter.ToSingle(byteArray, 4);
                        info.HeadPitchMax = BitConverter.ToSingle(byteArray, 8);
                        info.HeadPitchMin = BitConverter.ToSingle(byteArray, 12);
                        info.HeadRollMax = BitConverter.ToSingle(byteArray, 16);
                        info.HeadRollMin = BitConverter.ToSingle(byteArray, 20);
                        OnDoingCalibration?.Invoke(info);
                        break;
                    }
                case ((UInt32)XRStateEvent.kStateMagCalibrateSuccess):
                    OnCalibrateSuccess?.Invoke();
                    break;
                case ((UInt32)XRStateEvent.kStateMagCalibrateFailed):
                    OnCalibrateFailed?.Invoke();
                    break;
                default:
                    break;
            }
        }
    }

}
