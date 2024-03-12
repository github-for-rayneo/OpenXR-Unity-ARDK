using com.rayneo.xr.extensions;
using System;
using UnityEngine;
namespace RayNeo.API
{

    public class ShareCamera
    {
        public static int CameraWidth = 640;
        public static int CameraHeight = 480;

        private static int CameraFrames = 25;
        private static byte[] m_CameraCpuImage = new byte[(int)(CameraWidth * CameraHeight * 1.5f)];
        private static bool m_ShareCameraOpenState = false;
        public bool ShareCameraOpened => m_ShareCameraOpenState;
        public int YUVSize => m_CameraCpuImage.Length;
        //private IEnumerator _UpdateFrame()
        //{
        //    while (enabled)
        //    {
        //        yield return new WaitForSeconds(1000 / CameraFrames);

        //    }
        //}
//        public static void Open()
//        {
//            m_ShareCameraOpenState = true;
//#if UNITY_EDITOR
//#else

//             XRInterfaces.RayneoApi_OpenCamera();
//#endif
//        }

//        public static void Close()
//        {
//#if UNITY_EDITOR
//#else
//             XRInterfaces.RayNeoApi_CloseCamera();
//#endif
//            m_ShareCameraOpenState = false;
//        }
//        public static byte[] GetYUVFrame()
//        {
//            XRInterfaces.RayneoApi_GetLatestFrame(m_CameraCpuImage, CameraWidth, CameraHeight);
//            return m_CameraCpuImage;
//        }
//        public static void GetCameraColors(Action<int, int, Color32> call)
//        {
//#if UNITY_EDITOR
//#else
//            GetYUVFrame();
//            YUV420SP_RGB(m_CameraCpuImage, CameraWidth, CameraHeight, call);
//#endif
//        }


        public static void YUV420SP_RGB(byte[] yuvs, int w, int h, Action<int, int, Color32> call)
        {
            int chromaOffset = w * h;
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    int Y = yuvs[y * w + x];
                    int V = yuvs[chromaOffset + (y / 2) * w + 2 * (x / 2)];
                    int U = yuvs[chromaOffset + (y / 2) * w + 2 * (x / 2) + 1];

                    int C = Y - 16;
                    int D = U - 128;
                    int E = V - 128;

                    int R = (298 * C + 409 * E + 128) >> 8;
                    int G = (298 * C - 100 * D - 208 * E + 128) >> 8;
                    int B = (298 * C + 516 * D + 128) >> 8;

                    R = Math.Min(255, Math.Max(0, R));
                    G = Math.Min(255, Math.Max(0, G));
                    B = Math.Min(255, Math.Max(0, B));
                    call(x, y, new Color32((byte)R, (byte)G, (byte)B, 1));
                    //int offset = y * bmpStride + x * 3;
                    //Marshal.WriteByte(bmpPtr + offset, (byte)B);
                    //Marshal.WriteByte(bmpPtr + offset + 1, (byte)G);
                    //Marshal.WriteByte(bmpPtr + offset + 2, (byte)R);
                }
            }

        }

    }
}
