#if UNITY_EDITOR

using UnityEditor;
#endif

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

namespace RayNeo
{

    [InputControlLayout(displayName = "Ring Device", stateType = typeof(RingDeviceState))]
#if UNITY_EDITOR
    [InitializeOnLoad]
#endif
    public class RingInputDevice : UnityEngine.InputSystem.InputDevice, IInputStateCallbackReceiver
    {
        public ButtonControl button { get; private set; }
        public ButtonControl buttonHeavy { get; private set; }//触控板按下
        public QuaternionControl devicePose { get; private set; }
        public Vector3Control touchPosition { get; private set; }


#if UNITY_EDITOR

        static RingInputDevice()
        {
            Initialize();


        }
        ////戒指插入时,调用该接口.
        //[MenuItem("Tools/RayNeo Devices/Create Ring Device")]
        //public static void CreateDevice()
        //{
        //    // This is the code that you would normally run at the point where
        //    // you discover devices of your custom type.

        //    InputSystem.AddDevice<RingInputDevice>();
        //}

        ////戒指拔出时,调用该接口.
        //[MenuItem("Tools/RayNeo Devices/Remove Ring Device")]
        //public static void RemoveDevice()
        //{
        //    var customDevice = InputSystem.devices.FirstOrDefault(x => x is RingInputDevice);
        //    if (customDevice != null)
        //        InputSystem.RemoveDevice(customDevice);
        //}

#endif
        public static RingInputDevice current { get; private set; }


        public override void MakeCurrent()
        {
            base.MakeCurrent();
        }
        protected override void OnRemoved()
        {
            base.OnRemoved();
            if (current == this)
                current = null;
        }
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            InputSystem.RegisterLayout<RingInputDevice>(
                      matches: new InputDeviceMatcher()
                          .WithInterface("Ring"));
        }

        protected override void FinishSetup()
        {
            base.FinishSetup();
            button = GetChildControl<ButtonControl>("button_touch");
            buttonHeavy = GetChildControl<ButtonControl>("button_heavy");
            devicePose = GetChildControl<QuaternionControl>("devicePose");
            touchPosition = GetChildControl<Vector3Control>("touchPosition");
            current = this;

        }
        private RingDeviceState m_state = new RingDeviceState();
        public void OnNextUpdate()
        {
            InputSystem.QueueStateEvent(this, m_state);
        }




        public void UpdatePos(float x, float y, float z, bool touchBtnState)
        {
            if (!enabled)
            {
                return;
            }
            m_state.touchPosition = new Vector3(x, y, z);
            if (touchBtnState)
            {
                m_state.buttons |= 1 << 0;
            }
            else
            {
                short key = ~(1);
                m_state.buttons &= (ushort)key;
            }
        }
        public void UpdateQuaternion(Quaternion r)
        {
            if (!enabled)
            {
                return;
            }
            m_state.devicePose = r;
            //m_state.rotation = r;
            //m_state.devicePose.rotation = r;
            //var p = m_state.devicePose;
            //p.rotation = r;
            //m_state.devicePose = p;
        }
        public void OnHeavyPressStateChagne(bool press)
        {
            if (!enabled)
            {
                return;
            }

            if (press)
            {
                m_state.buttons |= 1 << 1;
            }
            else
            {
                short key = ~(1 << 1);
                m_state.buttons &= (ushort)key;
            }

        }

        public void OnStateEvent(InputEventPtr eventPtr)
        {
            InputState.Change(this, eventPtr);
        }

        public bool GetStateOffsetForEvent(InputControl control, InputEventPtr eventPtr, ref uint offset)
        {
            return false;
        }
    }

    public struct RingDeviceState : IInputStateTypeInfo
    {
        // In the case of a HID (which we assume for the sake of this demonstration),
        // the format will be "HID". In practice, the format will depend on how your
        // particular device is connected and fed into the input system.
        // The format is a simple FourCC code that "tags" state memory blocks for the
        // device to give a base level of safety checks on memory operations.
        public FourCC format => new FourCC('R', 'I', 'N', 'G');

        // InputControlAttributes on fields tell the input system to create controls
        // for the public fields found in the struct.

        // Assume a 16bit field of buttons. Create one button that is tied to
        // bit #3 (zero-based). Note that buttons do not need to be stored as bits.
        // They can also be stored as floats or shorts, for example.
        [InputControl(name = "button_touch", layout = "Button", bit = 0)]
        [InputControl(name = "button_heavy", layout = "Button", bit = 1)]
        public ushort buttons;

        // Create a floating-point axis. The name, if not supplied, is taken from
        // the field.
        [InputControl( name = "devicePose")]
        public Quaternion devicePose;
        [InputControl(name = "touchPosition")]
        public Vector3 touchPosition;
    }
}
