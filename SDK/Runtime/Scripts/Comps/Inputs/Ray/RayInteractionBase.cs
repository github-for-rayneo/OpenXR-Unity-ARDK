using FfalconXR.InputModule;
using FfalconXR.Native;
using RayNeo.API;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RayNeo
{
    public abstract class RayInteractionBase : BaseInputKeyHandler
    {
        protected RayNeoInput m_Input;
        public virtual void SetRayInput(RayNeoInput input)
        {
            m_Input = input;
        }

        public override void InputUpdate(out bool pointerDown, out bool pointerUp)
        {
            pointerDown = m_Input.SimpleTouch.Tap.WasPressedThisFrame();
            pointerUp = m_Input.SimpleTouch.Tap.WasReleasedThisFrame();
        }
        public abstract void Update();
    }
}
