// calibration of Mag
// 磁力计校准

namespace com.rayneo.xr.extensions
{
    using System;
    using System.Runtime.InteropServices;

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

    public abstract class XRCalibratorEvent
    {
        //检测出磁力计异常, 需要开始校准
        public abstract void onNeedCalibrate();
        //正在标定
        public abstract void onDoingCalibration(XRCalibratorHeadInfo info);
        //校准成功
        public abstract void onCalibrateSuccess();
        //校准失败
        public abstract void onCalibrateFailed(); 
        
    }

    public class XRMagCalibrator
    {
        private XRCalibratorEvent mEventNotifier = null;

        public XRMagCalibrator(XRCalibratorEvent EventNotifier)
        {
            this.mEventNotifier = EventNotifier;
            XRInterfaces.RegisterStateEventCallback(DelegatedMethod);
        }

        ~XRMagCalibrator()
        {
            XRInterfaces.UnregisterStateEventCallback(DelegatedMethod);
        }

        //开始校准
        //Start()/ Stop()必须配套使用
        public bool Start()
        {
            XRInterfaces.SendCommand((int)XRControlUnit.kUnitHeadTracker, (int)FXRControlCommand.kCtlCmdStartMagCalibration);
            return true;
        }

        //结束校准
        //Start()/ Stop()必须配套使用
        public bool Stop()
        {
            XRInterfaces.SendCommand((int)XRControlUnit.kUnitHeadTracker, (int)FXRControlCommand.kCtlCmdStopMagCalibration);
            return true;
        }

        private void DelegatedMethod(UInt32 state, UInt64 timestamp, uint length, IntPtr data)
        {
            switch (state)
            {
                case ((UInt32)XRStateEvent.kStateMagNeedCalibrate):
                    mEventNotifier.onNeedCalibrate();
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
                        mEventNotifier.onDoingCalibration(info);
                        break;
                    }
                case ((UInt32)XRStateEvent.kStateMagCalibrateSuccess):
                    mEventNotifier.onCalibrateSuccess();
                    break;
                case ((UInt32)XRStateEvent.kStateMagCalibrateFailed):
                    mEventNotifier.onCalibrateFailed();
                    break;
                default:
                    break;
            }
        }
    }
}