//using FFalcon.XR.Runtime;
using com.rayneo.xr.extensions;
using FfalconXR;
using System;
using UnityEngine;
namespace RayNeo.API
{

    /// <summary>
    /// 算法功能集合API
    /// </summary>
    public class Algorithm
    {

        private const int width = 640;
        private const int height = 480;

        private byte[] m_CameraCpuImage = new byte[(int)(width * height * 1.5f)];
        /// <summary>
        /// 打开slam
        /// </summary>
        public static void EnableSlamHeadTracker()
        {


#if UNITY_EDITOR
#else

            XRInterfaces.EnableSlamHeadTracker();
#endif

        }

        public static void DisableSlamHeadTracker()
        {
#if UNITY_EDITOR
#else

            XRInterfaces.DisableSlamHeadTracker();
#endif

        }

        public static SlamState GetSlamStatus()
        {
#if UNITY_EDITOR
            return SlamState.FFVINS_TRACKING_FAIL;
#else
            return (SlamState)XRInterfaces.GetHeadTrackerStatus();
#endif

        }
        /// <summary>
        /// slam当前状态.
        /// </summary>
        public enum SlamState
        {
            //正在初始化
            FFVINS_INITIALIZING = 0,
            //初始化成功
            FFVINS_TRACKING_SUCCESS = 1,
            //失败
            FFVINS_TRACKING_FAIL = 2,
        }

        /// <summary>
        /// 开启平面检测
        /// </summary>
        public static void EnablePlaneDetection()
        {
#if UNITY_EDITOR
#else

            XRInterfaces.EnablePlaneDetection();
#endif

        }
        /// <summary>
        /// 关闭平面检测
        /// </summary>
        public static void DisablePlaneDetection()
        {
#if UNITY_EDITOR
#else

            XRInterfaces.DisablePlaneDetection();
#endif


        }

        public void OpenShareCamera()
        {
#if UNITY_EDITOR
#else

             XRInterfaces.RayneoApi_OpenCamera();

#endif
        }

        public void CloseShareCamera()
        {
#if UNITY_EDITOR
#else
             XRInterfaces.RayNeoApi_CloseCamera();
#endif
        }
        public void GetCameraColors(Action<int, int, Color32> call)
        {
#if UNITY_EDITOR
#else
            XRInterfaces.RayneoApi_GetLatestFrame(m_CameraCpuImage,width,height);
            YUV420SP_RGB(m_CameraCpuImage, width, height, call);
#endif
        }
        /// <summary>
        /// 获取当前平面检测结果.创建XRPlaneInfo并维护.
        /// </summary>
        /// <param name="info">输出平面信息</param>
        /// <param name="arraySize">需要检测的平面数量</param>
        /// <returns></returns>
        public static int GetPlaneInfo(XRPlaneInfo[] info)
        {
            return XRInterfaces.GetPlaneInfo(info, info.Length);
        }

        /// <summary>
        /// 平面结果坐标转换
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static Vector3 ConvertPlanePosition(XRPlaneInfo info)
        {

            return new Vector3(info.pose.position.x, info.pose.position.y, info.pose.position.z);
        }
        /// <summary>
        /// 平面结果旋转转换.
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static Quaternion ConvertPlaneRotation(XRPlaneInfo info)
        {
            return new Quaternion(info.pose.rotation.x, info.pose.rotation.y, info.pose.rotation.z, info.pose.rotation.w);
        }

        /// <summary>
        /// 将现有的GameObject赋予平面
        /// </summary>
        /// <param name="onePlaneMeshInfo"></param>
        /// <param name="obj"></param>
        /// <param name="includeMaterial"></param>
        /// <returns></returns>
        public static GameObject CreatePlaneMesh(XRPlaneInfo onePlaneMeshInfo, GameObject obj, bool invertYZ = false, Material m = null)
        {
            if (onePlaneMeshInfo.local_polygon_size < 3)
            {
                Log.Error("FfalconApi.CreatePlaneMesh error,XRPlaneInfo error.local_polygon_size:", onePlaneMeshInfo.local_polygon_size);
                return obj;//不成立.
            }
            MeshFilter mf = obj.GetComponent<MeshFilter>();

            if (mf == null)
            {
                mf = obj.AddComponent<MeshFilter>();
            }

            Mesh mesh;

            if (mf.mesh == null)
            {
                mesh = new Mesh();
                mf.mesh = mesh;
            }
            else
            {
                mesh = mf.mesh;
            }



            Vector3[] vertices = new Vector3[onePlaneMeshInfo.local_polygon_size];
            for (int i = 0; i < onePlaneMeshInfo.local_polygon_size; i++)
            {
                int index = i * 2;
                if (invertYZ)
                {
                    vertices[i] = new Vector3(onePlaneMeshInfo.local_polygon[index], onePlaneMeshInfo.local_polygon[index + 1], 0);

                }
                else
                {
                    vertices[i] = new Vector3(onePlaneMeshInfo.local_polygon[index], 0, onePlaneMeshInfo.local_polygon[index + 1]);
                }


            }

            int trianglesCount = (onePlaneMeshInfo.local_polygon_size - 2);
            int[] triangles = new int[trianglesCount * 3];

            for (int i = 0; i < trianglesCount; i++)
            {
                int tranglesPos = i * 3;
                triangles[tranglesPos] = 0;
                triangles[tranglesPos + 2] = i + 1;
                triangles[tranglesPos + 1] = i + 2;

            }
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            if (m != null)
            {
                MeshRenderer mr = obj.GetComponent<MeshRenderer>();
                if (mr == null)
                {
                    mr = obj.AddComponent<MeshRenderer>();
                    mr.material = m;
                }
                //if (mr.material == null)
                //{
                //    mr.material = new Material(Shader.Find("Standard"));
                //    mr.material.color = Color.green;
                //}
            }

            return obj;
        }


        public static void SetUnityCameraToPhysicalCameraFOV(Camera camera)
        {
            float fx = 376.686F;
            float fy = 376.1188F;
            float cx = 319.3743F;
            float cy = 241.355F;
            float skew = 0f;
            var projectM = PerspectiveOffCenter(fx, fy, cx, cy, skew, camera.nearClipPlane, camera.farClipPlane);
            camera.projectionMatrix = projectM;


        }

        private static Matrix4x4 PerspectiveOffCenter(float fx, float fy, float cx, float cy, float skew, float near, float far)
        {
            float width = 640f;
            float height = 480f;
            float x = 2.0f * fx / width;
            float y = 2.0f * fy / height;
            float a = (-2f * cx + width) / width;
            float b = (2f * cy - height) / height;
            float c = -(far + near) / (far - near);
            float d = -2.0F * (far * near) / (far - near);
            float e = -1.0F;
            Matrix4x4 m = new Matrix4x4();
            m[0] = x;
            m[4] = -2 * skew / width;
            m[5] = y;
            m[8] = a;
            m[9] = b;
            m[10] = c;
            m[11] = e;
            m[14] = d;
            return m;
        }


        /// <summary>
        /// 获取偏航角
        /// </summary>
        /// <returns></returns>
        public static float GetAzimuth()
        {
#if UNITY_EDITOR
            return 0;
#else
            return XRInterfaces.RayNeoApi_NineAxisAzimuth();

#endif
        }


        /// <summary>
        /// 获取带磁力计的3dof旋转信息
        /// </summary>
        /// <returns></returns>
        public static Quaternion GetNineAxisOrientation()
        {
#if UNITY_EDITOR
            XRRotation rot = new XRRotation();

#else
            XRRotation rot = XRInterfaces.GetNineAxisOrientation();
#endif

            return new Quaternion(rot.x, rot.y, rot.z, rot.w);
        }

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
