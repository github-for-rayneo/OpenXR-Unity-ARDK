using System.Runtime.InteropServices;
using UnityEngine.UIElements;

namespace com.rayneo.xr.extensions
{
    public static class XRMathUtils
    {
        /// <summary>
        /// runtime右手坐标系结果转换成Unity左手系结果
        /// </summary>
        /// <param name="rotation">旋转, 四元数, [x,y,z,w]</param>
        /// <param name="position">平移</param>
        static public void RightHand2UnityLeftHand(float[] rotation, float[] position)
        {
            rotation[0] = -rotation[0];
            rotation[1] = -rotation[1];
            rotation[2] = rotation[2];
            rotation[3] = rotation[3];

            position[0] = position[0];
            position[1] = position[1];
            position[2] = -position[2];
        }

        /// <summary>
        /// runtime右手坐标系结果转换成Unity左手系结果
        /// </summary>
        /// <param name="rotation">旋转, 四元数, [x,y,z,w]</param>
        static public void RightHand2UnityLeftHand(float[] rotation)
        {
            rotation[0] = -rotation[0];
            rotation[1] = -rotation[1];
            rotation[2] = rotation[2];
            rotation[3] = rotation[3];
        }

        /// <summary>
        /// runtime右手坐标系结果转换成Unity左手系结果
        /// </summary>
        /// <param name="rotation">旋转, 四元数</param>
        /// <param name="position">平移</param>
        static public void RightHand2UnityLeftHand(XRRotation rotation, XRPosition position)
        {
            rotation.x = -rotation.x;
            rotation.y = -rotation.y;

            position.z = -position.z;
        }

        /// <summary>
        /// runtime右手坐标系结果转换成Unity左手系结果
        /// </summary>
        /// <param name="rotation"></param>
        static public void RightHand2UnityLeftHand(XRRotation rotation)
        {
            rotation.x = -rotation.x;
            rotation.y = -rotation.y;
        }

    }
}