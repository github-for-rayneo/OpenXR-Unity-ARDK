using System;
using UnityEngine;
namespace RayNeo.API
{

    public static class IPC
    {

        #region gps
        public static Action<PhoneGPSResultType, string> GpsStateChagneCallBack;
        public static Action<long, double, double, double, double, double, double> GPSPushCallBack;

        private static AndroidJavaObject m_phoneMamanager;
        private static AndroidJavaObject PhoneManager
        {
            get
            {
                if (m_phoneMamanager == null)
                {

                    var m = new AndroidJavaClass(PlatformAndroid.SupportPackagePath + ".ipc.phonegps.PhoneGPSManager");
                    m_phoneMamanager = m.CallStatic<AndroidJavaObject>("ins");

                }
                return m_phoneMamanager;
            }
        }
        public static void OpenPhoneGPS()
        {
            if (Application.isEditor)
            {
                return;
            }
            PhoneManager.Call("RegCallBack", new PhoneGPSListener());
            PhoneManager.Call("OpenPhoneGPS");
        }

        public static void ClosePhoneGPS()
        {
            if (Application.isEditor)
            {
                return;
            }
            PhoneManager.Call("RegCallBack", null);
            PhoneManager.Call("close");
        }
        //public void PushStateChange(int code, String msg);
        //public void OnGPSPush(long time, double latitude, double longitude, double altitude, double speed,
        //                       double horizontalAccuracyMeters, double AccuracyMeters);

    }

    class PhoneGPSListener : AndroidJavaProxy
    {
        public PhoneGPSListener() : base(PlatformAndroid.SupportPackagePath + ".ipc.phonegps.PhoneGPSListener")
        {
        }

        public void PushStateChange(int code, String msg)
        {
            IPC.GpsStateChagneCallBack((PhoneGPSResultType)code, msg);
        }
        public void OnGPSPush(long time, double latitude, double longitude, double altitude, double speed,
                               double horizontalAccuracyMeters, double AccuracyMeters)
        {
            IPC.GPSPushCallBack?.Invoke(time, latitude, longitude, altitude, speed, horizontalAccuracyMeters, AccuracyMeters);
        }
    }
    #endregion


    public enum PhoneGPSResultType
    {
        //0�ֻ������� 1 �ֻ�δ���� ���ֻ�δ���ӷ��� 2���û�����֮��������ע������ 3.������ʱ�������ֻ������������
        UNKNOW = -1,

        PHONE_CONNECTED = 0,
        PHONE_NOT_CONNECTED = 1,
        PHONE_REGITST_PUSH = 2,
        PHONE_PUSH_TIME_OUT = 3,
    }
}
