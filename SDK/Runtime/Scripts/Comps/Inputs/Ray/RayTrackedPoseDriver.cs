using FfalconXR.InputModule;
using FfalconXR.Native;
using RayNeo.API;
using UnityEngine;

namespace RayNeo
{

    public enum RayControllerType
    {
        CenterFocus,//中心点不移动
        CellPhone,
        Eye,
        Ring,
        Custom,
    }
    public class RayTrackedPoseDriver : MonoBehaviour
    {

        public RayControllerType m_RayControllerType;
        private RayNeoInput m_RayInput;
        public GameObject m_BeamLine;
        public bool m_VisbileLine = false;
        private RayInteractionBase m_InteractionBase;

        private void Awake()
        {
            SetLineVisibleState(m_VisbileLine);
            SetupInputKeyHandler();
            m_RayInput = new RayNeoInput();
            m_RayInput.Enable();
            switch (m_RayControllerType)
            {
                case RayControllerType.CenterFocus:
                    SetRayInteractionProxy<RayDefault>();
                    break;
                case RayControllerType.CellPhone:
                    SetRayInteractionProxy<RayCellPhone>();
                    break;
                case RayControllerType.Eye:
                    break;
                case RayControllerType.Ring:
                    SetRayInteractionProxy<RayRing>();
                    break;
                default:
                    SetRayInteractionProxy<RayDefault>();
                    break;
            }
        }

        public void SetRayInteractionProxy<T>() where T : RayInteractionBase
        {
            m_InteractionBase = gameObject.AddComponent<T>();
            m_InteractionBase.SetRayInput(m_RayInput);
        }
        public void SetLineVisibleState(bool visible)
        {
            if (m_BeamLine != null)
            {
                m_BeamLine.SetActive(visible);
            }
        }

        private void OnDestroy()
        {
            if (m_RayInput != null)
            {
                m_RayInput.Disable();
            }
        }
        private void Update()
        {

#if UNITY_EDITOR
            return;
#endif


            if (m_InteractionBase == null)
            {
                return;
            }
            m_InteractionBase.Update();
            //if (!RingDriver.RingOpened)
            //{
            //    return;
            //}
            //if (RingDriver.GetTouchType() == RingTouchType.UnityTouchEvent)
            //{
            //    return;//unity不执行.
            //}
            //Debug.LogError("CCCCCCCCCCCCCCCCCC2:" + m_ring.RingAction.Quaternion.ReadValue<Quaternion>());

            //transform.rotation = m_ring.RingAction.Quaternion.ReadValue<Quaternion>();
        }

        private void SetupInputKeyHandler()
        {
            //#if ENABLE_INPUT_SYSTEM
            //            if (!GameObject.Find("NewInputSystemKeyHandler"))
            //            {
            //                NewInputSystemKeyHandler keyHandler = new GameObject("NewInputSystemKeyHandler").AddComponent<NewInputSystemKeyHandler>();
            //            }
            //#elif ENABLE_LEGACY_INPUT_MANAGER
            //            if (!GameObject.Find("UnityInputKeyHandler"))
            //            {
            //                UnityInputKeyHandler keyHandler = new GameObject("UnityInputKeyHandler").AddComponent<UnityInputKeyHandler>();
            //            }
            //#endif
        }
    }
}
