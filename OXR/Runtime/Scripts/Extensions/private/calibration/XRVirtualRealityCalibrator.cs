// 虚实标定

namespace com.rayneo.xr.extensions
{
    using System;
    using System.Runtime.InteropServices;
    using UnityEngine;
    using UnityEngine.UIElements;
    using System.Collections;
    using System.Drawing;
    using UnityEngine.XR;

    public class XRVirtualRealityCalibrator
    {
        /// <summary>
        /// 设置渲染相机投影矩阵，
        /// 此接口会直接修改渲染相机投影矩阵并存储，
        /// 调用请慎重
        /// </summary>
        /// <param name="eye">left or right</param>
        /// <param name="projection">列主序投影矩阵</param>
        /// <returns></returns>
        public static int SetRenderProjectionMatrix(FXREye eye, Matrix4x4 projection)
        {
            float[] array = new float[16];
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    array[i * 4 + j] = projection[i, j];
                }
            }

            IntPtr ptr = Marshal.AllocHGlobal(sizeof(float) * 16);
            Marshal.Copy(array, 0, ptr, sizeof(float) * 16);
            string item = (eye == FXREye.kEyeLeft ? "LeftRenderProjectionMatrix" : "RightRenderProjectionMatrix");
            int ret = XRInterfaces.SetProp(item, ptr, sizeof(float) * 16);
            Marshal.FreeHGlobal(ptr);
            return ret;
        }

        /// <summary>
        ///  获取渲染相机投影矩阵,未标定时, 由工装FOV计算得到
        /// </summary>
        /// <param name="eye">left or right</param>
        /// <returns>列主序投影矩阵</returns>
        public static Matrix4x4 GetRenderProjectionMatrix(FXREye eye)
        {
            string item = (eye == 0 ? "LeftRenderProjectionMatrix" : "RightRenderProjectionMatrix");

            float[] array = new float[16];
            Matrix4x4 projection = Matrix4x4.identity;
            //Pin一下 不然Marshal结合gc管理的 array会出现问题
            GCHandle handle = GCHandle.Alloc(array, GCHandleType.Pinned);
            try
            {
                IntPtr ptr = Marshal.AllocHGlobal(sizeof(float) * 16);
                
                int len = 0;
                XRInterfaces.GetProp(item, ptr, ref len);

                if (len == 16 * sizeof(float))
                {
                    Marshal.Copy(ptr, array, 0, len);
                    for (int i = 0; i < 4; i++)
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            projection[i, j] = array[i * 4 + j];
                        }
                    }
                }
                Marshal.FreeHGlobal(ptr);
            }
            finally
            {
                if (handle.IsAllocated)
                    handle.Free();
            }
            Debug.Log("Prj " + projection);
            return projection;
        }

        /// <summary>
        /// 设置渲染相机外参
        /// </summary>
        /// <param name="eye"></param>
        /// <param name="rotation"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public static int SetRenderExtrinsicParameters(FXREye eye, Quaternion rotation, Vector3 position)
        {
            Matrix4x4 extrinsic = Matrix4x4.TRS(position, rotation, Vector3.one);
            Matrix4x4 _ext = AdjustCoordinates(extrinsic);
            Quaternion rotation_gl = _ext.ExtractRotation();
            Vector3 position_gl = _ext.ExtractPosition();
            float[] array = { rotation_gl.x, rotation_gl.y, rotation_gl.z, rotation_gl.w, position_gl.x, position_gl.y, position_gl.z };
            
            IntPtr ptr = Marshal.AllocHGlobal(sizeof(float) * 7);
            Marshal.Copy(array, 0, ptr, sizeof(float) * 7);
            string item = (eye == FXREye.kEyeLeft ? "LeftRenderExtrinsicParameters" : "RightRenderExtrinsicParameters");
            int ret = XRInterfaces.SetProp(item, ptr, sizeof(float) * 7);
            Marshal.FreeHGlobal(ptr);
            return ret;
        }

        /// <summary>
        /// 获取渲染相机外参
        /// 未标定时由IPD计算得到
        /// </summary>
        /// <param name="eye"></param>
        /// <returns>列主序外参矩阵</returns>
        public static Matrix4x4 GetRenderExtrinsicParameters(FXREye eye)
        {
            Matrix4x4 extrinsic = Matrix4x4.identity;
            string item = (eye == 0 ? "LeftRenderExtrinsicParameters" : "RightRenderExtrinsicParameters");
            float[] array = new float[16];  // TODO 应该7，但是7会crash， 原因未知

            //Pin一下 不然Marshal结合gc管理的 array会出现问题
            GCHandle handle = GCHandle.Alloc(array, GCHandleType.Pinned);
            try
            {
                IntPtr ptr = Marshal.AllocHGlobal(sizeof(float) * 7);  
                int len = 0;
                XRInterfaces.GetProp(item, ptr, ref len);
                Marshal.Copy(ptr, array, 0, len);
                Quaternion rot = new Quaternion(array[0], array[1], array[2], array[3]);
                Vector3 pos = new Vector3(array[4], array[5], array[6]);
                extrinsic = Matrix4x4.TRS(pos, rot, Vector3.one);
                Marshal.FreeHGlobal(ptr);
            }
            finally
            {
                if (handle.IsAllocated)
                    handle.Free();
            }

            return extrinsic;
        }

        /// <summary>
        /// 设置camera外参
        /// </summary>
        /// <param name="rotation"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public static int SetCameraExtrinsicParameters(Quaternion rotation, Vector3 position)
        {
            float[] array = { rotation.x, rotation.y, rotation.z, rotation.w, position.x, position.y, position.z };
            IntPtr ptr = Marshal.AllocHGlobal(sizeof(float) * 7);
            Marshal.Copy(array, 0, ptr, sizeof(float) * 7);
            int ret = XRInterfaces.SetProp("Camera0ExtrinsicParameters", ptr, sizeof(float) * 7);
            Marshal.FreeHGlobal(ptr);
            return ret;
        }

        /// <summary>
        /// 是否执行过标定(是否存储了标定参数)
        /// </summary>
        public static bool HasConductedCalibration()
        {

            int[] res = new int[1];
            //Pin一下 不然Marshal结合gc管理的 array会出现问题
            GCHandle handle = GCHandle.Alloc(res, GCHandleType.Pinned);
            try
            {
                IntPtr ptr = Marshal.AllocHGlobal(sizeof(int) * 1);

                int len = 0;
                XRInterfaces.GetProp("HasConductedCalibration", ptr, ref len);
                Marshal.Copy(ptr, res, 0, len);

                Marshal.FreeHGlobal(ptr);
            }
            finally
            {
                if (handle.IsAllocated)
                    handle.Free();
            }
            return (res[0] != 0);
        }

        /**
         * @brief used to changing coordinates from opencv frame to opengl frame
         *        i.e. T_render_camera is defined in opencv frame. So, 
         *        if we need to change render coordinate from opencv to opengl, then T_gl_cv * T_render_camera
         *        if we need to change camera coordinate from opencv to opengl, then T_render_camera * T_gl_cv.inverse()
         */
        private static Matrix4x4 AdjustCoordinates(Matrix4x4 origin)
        {
            Matrix4x4 T = Matrix4x4.identity;
            T[1, 1] = -1;
            T[2, 2] = -1;
            return (T * origin);
        }
    }
}