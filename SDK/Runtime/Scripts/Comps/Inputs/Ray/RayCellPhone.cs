using FfalconXR.InputModule;
using FfalconXR.Native;
using RayNeo.API;
using UnityEngine;

namespace RayNeo
{
    public class RayCellPhone : RayInteractionBase
    {

        public override void SetRayInput(RayNeoInput input)
        {
            base.SetRayInput(input);
        }

        public override void Update()
        {
            Debug.LogError("YYYYYYYYYYYYYYYYYYYYYYYY:"+ m_Input.CellPhone.Pose.ReadValue<Pose>().rotation);
            transform.rotation = m_Input.CellPhone.Pose.ReadValue<Pose>().rotation;
        }
    }
}
