using RayNeo.API;
using System;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem.XR;

namespace RayNeo
{


    public class HeadTrackedPoseDriver : TrackedPoseDriver
    {
        private static HeadTrackedPoseParams m_params = new HeadTrackedPoseParams();

        public Camera CenterCamera;
        ///// <summary>
        ///// 使用眼镜上的相机为姿态.
        ///// </summary>
        //public bool m_useActualCameraAttitude = false;
        public Pose RGBEyePose;
        public static event Action<Pose> OnPostUpdate;
        public XROrigin m_Origin;

        public GameObject Origin => m_Origin.gameObject;

        public static void ResetQuaternion()
        {
            m_params.ResetQuaternionAction?.Invoke();
        }

        override protected void Awake()
        {
            CenterCamera = GetComponent<Camera>();
            RGBEyePose = new Pose();

            m_params.AwakeDriver(this, ResetTrackedPos);
            base.Awake();
        }

        protected override void OnDestroy()
        {
            m_params.DestroyDriver(this, ResetTrackedPos);
            base.OnDestroy();
        }

        protected override void SetLocalTransform(Vector3 newPosition, Quaternion newRotation)
        {
            var rot = m_params.GetRotation(newRotation);
            base.SetLocalTransform(newPosition, rot);
            RGBEyePose.position = newPosition;
            RGBEyePose.rotation = rot;
            OnPostUpdate?.Invoke(RGBEyePose);
        }
        //public Vector3 ActualCameraPositionOffset()
        //{
        //    return new Vector3(-0.00768132f, 0.0040311f, 0.00137355f);
        //}

        //public Quaternion ActualCameraRotationOffset()
        //{
        //    return new Quaternion(0, 0, 0, 1);

        //}



        public void ResetTrackedPos()
        {
            m_params.ResetRotation();
        }



#if UNITY_EDITOR

        private Quaternion m_debugQuaternion = Quaternion.Euler(0, 0, 0);
        private Vector3 m_debugTrans = Vector3.zero;

        public static void SetQuaternion(Quaternion q)
        {
            foreach (var item in m_params.DriverInstanceSet)
            {
                item.m_debugQuaternion = q;
            }
        }
        public static void SetPosition(Vector3 t)
        {
            foreach (var item in m_params.DriverInstanceSet)
            {
                item.m_debugTrans = t;
            }
        }
        protected override void PerformUpdate()
        {
            SetLocalTransform(m_debugTrans, m_debugQuaternion);
        }
#else
#endif


    }

    internal class HeadTrackedPoseParams
    {
        public Quaternion CameraInverseQuaternion = Quaternion.identity;
        public Quaternion RotationRawData = Quaternion.identity;
        public Action ResetQuaternionAction;

        public HashSet<HeadTrackedPoseDriver> DriverInstanceSet = new HashSet<HeadTrackedPoseDriver>();

        public void AwakeDriver(HeadTrackedPoseDriver hdk, Action resetAction)
        {
            DriverInstanceSet.Add(hdk);
            ResetQuaternionAction += resetAction;
        }
        public void DestroyDriver(HeadTrackedPoseDriver hdk, Action resetAction)
        {
            DriverInstanceSet.Remove(hdk);
            ResetQuaternionAction -= resetAction;
        }

        public Quaternion GetRotation(Quaternion rawQuat)
        {
#if UNITY_EDITOR

#else
          if (  !RayNeoInfo.HasDevice())

            {
                Vector3 euler = rawQuat.eulerAngles;
                euler = new Vector3(euler.x, euler.y, euler.z - 90);
                rawQuat = Quaternion.Euler(euler);
            }
           
#endif
            //var imu2Camera = new Quaternion(-0.999359f, -0.0238991f, -0.0247418f, -0.00988973f);
            //RotationRawData = rawQuat * imu2Camera;
            RotationRawData = rawQuat;
            return CameraInverseQuaternion * RotationRawData;
        }

        public void ResetRotation()
        {
            CameraInverseQuaternion = Quaternion.Inverse(RotationRawData);
        }
    }

}
