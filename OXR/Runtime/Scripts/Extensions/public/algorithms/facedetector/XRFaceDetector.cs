namespace com.rayneo.xr.extensions
{
    using AOT;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public static class XRFaceDetector
    {
        static XRFaceDetector()
        {
#if UNITY_ANDROID
            var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            var activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            var context = activity.Call<AndroidJavaObject>("getApplicationContext");

            FXRUnity_InitializeAndroid(activity.GetRawObject());
            var algorithm = new AndroidJavaClass("com.ffalcon.xr.extension.FxrAlgorithm");
            algorithm.CallStatic<bool>("InitAndroid", context);
#endif
        }

        public static long CreateFaceDetector()
        {
            IntPtr pHandle;
            if (FXRFeature_CreateFaceDetector(out pHandle) == 0)
            {
                return pHandle.ToInt64();
            }
            return 0;
        }

        public static void InitFaceDetector()
        {
            FXRFeature_InitFaceDetector();
        }

        public static void DestroyFaceDetector()
        {
            FXRFeature_DestroyFaceDetector();
        }

        public static void SetCameraFrameRate(int fps) {
            FXRFeature_SetFrameRate(fps);
        }

        public static float[] GetFacePosition(float[] position)
        {
            FXRFeature_GetFacePosition(position, position.Length);
            return position;
        }

        public static float[] GetFaceInCamera(float[] position)
        {
            FXRFeature_GetFaceInCamera(position, position.Length);
            return position;
        }

        public static float[] GetFaceLookAt()
        {
            float[] lookAt = new float[3];
            FXRFeature_GetFaceLookAt(lookAt, lookAt.Length);
            return lookAt;
        }

        public static bool CheckFaceState()
        {
            return FXRFeature_CheckFaceState();
        }





        /*
         *  Symbols import from libFFalconXR_algorithm.so
         *  For face detector
         */
#if UNITY_ANDROID
        [DllImport(XRConstants.XRFaceDetector)]
        private static extern void FXRUnity_InitializeAndroid(IntPtr context);
#endif

        [DllImport(XRConstants.XRFaceDetector)]
        private static extern int FXRFeature_CreateFaceDetector(out IntPtr handle);

        [DllImport(XRConstants.XRFaceDetector)]
        private static extern void FXRFeature_InitFaceDetector();

        [DllImport(XRConstants.XRFaceDetector)]
        private static extern void FXRFeature_DestroyFaceDetector();

        [DllImport(XRConstants.XRFaceDetector)]
        private static extern void FXRFeature_GetFacePosition(float[] position, int size);

        [DllImport(XRConstants.XRFaceDetector)]
        private static extern void FXRFeature_GetFaceInCamera(float[] position, int size);

        [DllImport(XRConstants.XRFaceDetector)]
        private static extern void FXRFeature_GetFaceLookAt(float[] lookAt, int size);

        [DllImport(XRConstants.XRFaceDetector)]
        private static extern void FXRFeature_SetFrameRate(int fps);

        [DllImport(XRConstants.XRFaceDetector)]
        private static extern bool FXRFeature_CheckFaceState();


    }
}