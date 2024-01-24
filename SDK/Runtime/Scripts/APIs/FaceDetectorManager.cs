using com.rayneo.xr.extensions;
using UnityEngine;
namespace RayNeo
{

    public class FaceDetectorManager
    {
        private static FaceDetectorManager ins = new FaceDetectorManager();
        public static FaceDetectorManager Ins
        {
            get
            {
                return ins;
            }
        }
#if UNITY_EDITOR
#else

        float[] m_position = new float[3];
        Vector3 m_posVec3 = Vector3.zero;
        private long m_faceHandle = 0;
#endif

        /// <summary>
        /// ��ȡ����λ��. 
        /// ���ü������ʼ��.��Ҫ���ʵ�ʱ������StopFaceDectector
        /// </summary>
        /// <param name="suc">������û�л�ȡ������.</param>
        /// <returns>�����Vector3.zero����û�л�ȡ��.</returns>

        public Vector3 GetFacePosition(out bool suc)
        {
#if UNITY_EDITOR
            //�༭����ִ��. �������Կ��Ǽ���debug
            suc = false;
            return Vector3.zero;
#else
            if (m_faceHandle == 0)
            {
                m_faceHandle = XRFaceDetector.CreateFaceDetector();
                if (m_faceHandle != 0)
                {
                    XRFaceDetector.InitFaceDetector();
                }
            }
            if (m_faceHandle != 0)
            {
                XRFaceDetector.GetFaceInCamera(m_position);
                if (m_position[0] == 0 && m_position[1] == 0 && m_position[2] == 0)
                {
                    suc = false;
                    return Vector3.zero;
                }
                m_posVec3.Set(m_position[0], m_position[1], m_position[2]);
            }
            else
            {
                suc = false;
                return Vector3.zero;
            }
            suc = true;
            return m_posVec3;
#endif

        }

        public void StopFaceDectector()
        {
#if UNITY_EDITOR
            return;
#else

            XRFaceDetector.DestroyFaceDetector();
            m_faceHandle = 0;
#endif
        }

    }
}
