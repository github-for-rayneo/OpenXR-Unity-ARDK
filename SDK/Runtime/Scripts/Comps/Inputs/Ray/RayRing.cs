using FfalconXR.InputModule;
using FfalconXR.Native;
using RayNeo.API;
using UnityEngine;
using UnityEngine.InputSystem.XR;

namespace RayNeo
{
    public class RayRing : RayInteractionBase
    {

        public override void SetRayInput(RayNeoInput input)
        {
            base.SetRayInput(input);
        }

        public override void Update()
        {
            if (!RingManager.RingOpened)
            {
                return;
            }
            Quaternion p = m_Input.Ring.Rotation.ReadValue<Quaternion>();
            transform.rotation = p;
        }

        public override void InputUpdate(out bool pointerDown, out bool pointerUp)
        {
            if (RingManager.RingOpened)
            {
                pointerDown = m_Input.Ring.TouchPadClick.WasPressedThisFrame();
                pointerUp = m_Input.Ring.TouchPadClick.WasReleasedThisFrame();
            }
            else
            {
                base.InputUpdate(out pointerDown, out pointerUp);

            }
        }
    }
}
