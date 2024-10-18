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
        public static void CreateFaceDetector()
        {
            RayneoApi_CreateFaceDetector();
        }

        public static void DestroyFaceDetector()
        {
            RayneoApi_DestroyFaceDetector();
        }

        public static float[] GetFacePosition(float[] position)
        {
            RayneoApi_GetFaceInCamera(position);
            return position;
        }

        public static int CheckFaceState()
        {
            return RayneoApi_CheckFaceState();
        }


        [DllImport(XRConstants.XRInterfaces)]
        private static extern void RayneoApi_CreateFaceDetector();

        [DllImport(XRConstants.XRInterfaces)]
        private static extern void RayneoApi_DestroyFaceDetector();

        [DllImport(XRConstants.XRInterfaces)]
        private static extern int RayneoApi_CheckFaceState();

        [DllImport(XRConstants.XRInterfaces)]
        private static extern void RayneoApi_GetFaceInCamera(float[] position);


    }
}