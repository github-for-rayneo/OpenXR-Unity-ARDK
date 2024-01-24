using System.Collections.Generic;
using UnityEngine.Scripting;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.XR;
using UnityEngine.InputSystem;

#if UNITY_EDITOR
using UnityEditor;
#endif
#if USE_INPUT_SYSTEM_POSE_CONTROL
#else
using PoseControl = UnityEngine.XR.OpenXR.Input.PoseControl;
#endif

namespace UnityEngine.XR.OpenXR.Features.Interactions
{
    /// <summary>
    /// This <see cref="OpenXRInteractionFeature"/> enables the use of RayNeo controller interaction profiles in OpenXR.
    /// </summary>
#if UNITY_EDITOR
    [UnityEditor.XR.OpenXR.Features.OpenXRFeature(UiName = "RayNeo Controller Profile",
        BuildTargetGroups = new[] { BuildTargetGroup.Android },
        Company = "RayNeo",
        Desc = "Allows for mapping input to the RayNeo Controller interaction profile.",
        OpenxrExtensionStrings = extensionString,
        Version = "0.0.1",
        Category = UnityEditor.XR.OpenXR.Features.FeatureCategory.Interaction,
        FeatureId = featureId
        )]
#endif
    public class RayNeoControllerProfile : OpenXRInteractionFeature
    {
        protected internal enum RayNeoActionType
        {
            UNKNOW_ACTION_TYPE,
            Thumbstick_Touch,
            Thumbstick_Click,
            Thumbstick_X,
            Thumbstick_Y,
            Thumbstick_Vector2f, // 此处为打样，理论上触控板只保留Vector2f或者XY即可，无需两个定义
            Temple_Tap,
            Head_Pose,
            Ring_Pose,
            Eye_Pose
        }
        /// <summary>
        /// The feature id string. This is used to give the feature a well known id for reference.
        /// </summary>
        public const string featureId = "com.unity.openxr.feature.input.rayneo";

        [Preserve, InputControlLayout(displayName = "CellPhone")]

        public class RayNeoCellPhoneController : RayNeoController
        {
            [Preserve, InputControl(offset = 0, aliases = new[] { "device", "gripPose" }, usage = "Device")]
            public PoseControl devicePose { get; private set; }
            protected override void FinishSetup()
            {
                base.FinishSetup();
                devicePose = GetChildControl<PoseControl>("devicePose");
            }
        }

        [Preserve, InputControlLayout(displayName = "Ring")]

        public class RayNeoRingController : RayNeoController
        {
            [Preserve, InputControl(aliases = new[] { "HomeButton" }, usage = "Home")]
            public ButtonControl homeButton { get; private set; }

            [Preserve, InputControl(offset = 0, aliases = new[] { "device", "gripPose" }, usage = "Device")]
            public PoseControl devicePose { get; private set; }
            protected override void FinishSetup()
            {
                base.FinishSetup();
                homeButton = GetChildControl<ButtonControl>("homeButton");
                devicePose = GetChildControl<PoseControl>("devicePose");

            }
        }

        [Preserve, InputControlLayout(displayName = "Eye")]

        public class RayNeoEyeController : RayNeoController
        {
            [Preserve, InputControl(aliases = new[] { "EyePos" }, usage = "eyePos")]
            public PoseControl eyePos { get; private set; }
            protected override void FinishSetup()
            {
                base.FinishSetup();
                eyePos = GetChildControl<PoseControl>("eyePos");

            }
        }

        /// <summary>
        /// An Input System device based on the hand interaction profile in the RayNeo controller</a>.
        /// </summary>
        [Preserve, InputControlLayout(isGenericTypeOfDevice = true, displayName = "RayNeo (OpenXR)")]
        public class RayNeoController : UnityEngine.InputSystem.InputDevice
        {
            //[Preserve, InputControl(aliases = new[] { "HomeButton" }, usage = "Ring")]
            //public ButtonControl homeButton { get; private set; }


            //public RayNeoRingController ring = InputSystem.InputSystem.GetDevice<RayNeoRingController>("Ring");

            //public UnityEngine.InputSystem.InputDevice ring = InputSystem.InputSystem.GetDevice<UnityEngine.InputSystem.InputDevice>("Ring");
            //public RayNeoController ring=> UnityEngine.InputSystem.GetDevice<XRController>(CommonUsages.LeftHand);

            //[Preserve, InputControl(aliases = new[] { "HomeButton" }, usage = "Home")]
            //public ButtonControl homeButton { get; private set; }

            //[Preserve, InputControl(aliases = new[] { "RingPos" }, usage = "RingPos")]
            //public QuaternionControl ringPos { get; private set; }

            /*[Preserve, InputControl(aliases = new[] { "Pad Button" }, usage = "PadButton")]
            public ButtonControl padButton { get; private set; }//触控板按下
            [Preserve, InputControl(aliases = new[] { "TouchPad" }, usage = "TouchPad")]
            public Vector2Control position { get; private set; }

            [Preserve, InputControl(aliases = new[] { "Ring Pos" }, usage = "RingPos")]
            public QuaternionControl rotation { get; private set; } */


            /// <summary>
            /// A [Vector2Control](xref:UnityEngine.InputSystem.Controls.Vector2Control) that represents the <see cref="RayNeoControllerProfile.thumbstick"/> OpenXR binding.
            /// </summary>
            //[Preserve, InputControl(aliases = new[] { "Primary2DAxis", "Joystick" }, usage = "Primary2DAxis")]
            //public Vector2Control thumbstick { get; private set; }

            ///// <summary>
            ///// A [AxisControl](xref:UnityEngine.InputSystem.Controls.AxisControl) that represents the <see cref="RayNeoControllerProfile.squeezeValue"/> OpenXR binding.
            ///// </summary>
            //[Preserve, InputControl(aliases = new[] { "GripAxis", "squeeze" }, usage = "Grip")]
            //public AxisControl grip { get; private set; }

            ///// <summary>
            ///// A [ButtonControl](xref:UnityEngine.InputSystem.Controls.ButtonControl) that represents the <see cref="RayNeoControllerProfile.squeezeClick"/> OpenXR binding.
            ///// </summary>
            //[Preserve, InputControl(aliases = new[] { "GripButton", "squeezeClicked" }, usage = "GripButton")]
            //public ButtonControl gripPressed { get; private set; }

            ///// <summary>
            ///// A [ButtonControl](xref:UnityEngine.InputSystem.Controls.ButtonControl) that represents the <see cref="RayNeoControllerProfile.menu"/> OpenXR bindings.
            ///// </summary>
            //[Preserve, InputControl(aliases = new[] { "Primary", "menuButton" }, usage = "Menu")]
            //public ButtonControl menu { get; private set; }

            ///// <summary>
            ///// A [ButtonControl](xref:UnityEngine.InputSystem.Controls.ButtonControl) that represents the <see cref="RayNeoControllerProfile.system"/> OpenXR bindings.
            ///// </summary>
            //[Preserve, InputControl(aliases = new[] { "systemButton" }, usage = "system")]
            //public ButtonControl system { get; private set; }

            ///// <summary>
            ///// A [ButtonControl](xref:UnityEngine.InputSystem.Controls.ButtonControl) that represents the <see cref="RayNeoControllerProfile.buttonA"/> <see cref="RayNeoControllerProfile.buttonX"/> OpenXR bindings, depending on handedness.
            ///// </summary>
            //[Preserve, InputControl(aliases = new[] { "A", "X", "buttonA", "buttonX" }, usage = "PrimaryButton")]
            //public ButtonControl primaryButton { get; private set; }

            ///// <summary>
            ///// A [ButtonControl](xref:UnityEngine.InputSystem.Controls.ButtonControl) that represents the <see cref="RayNeoControllerProfile.buttonATouch"/> <see cref="RayNeoControllerProfile.buttonYTouch"/> OpenXR bindings, depending on handedness.
            ///// </summary>
            //[Preserve, InputControl(aliases = new[] { "ATouched", "XTouched", "ATouch", "XTouch", "buttonATouched", "buttonXTouched" }, usage = "PrimaryTouch")]
            //public ButtonControl primaryTouched { get; private set; }

            ///// <summary>
            ///// A [ButtonControl](xref:UnityEngine.InputSystem.Controls.ButtonControl) that represents the <see cref="RayNeoControllerProfile.buttonB"/> <see cref="RayNeoControllerProfile.buttonY"/> OpenXR bindings, depending on handedness.
            ///// </summary>
            //[Preserve, InputControl(aliases = new[] { "B", "Y", "buttonB", "buttonY" }, usage = "SecondaryButton")]
            //public ButtonControl secondaryButton { get; private set; }

            ///// <summary>
            ///// A [ButtonControl](xref:UnityEngine.InputSystem.Controls.ButtonControl) that represents the <see cref="RayNeoControllerProfile.buttonBTouch"/> <see cref="RayNeoControllerProfile.buttonYTouch"/> OpenXR bindings, depending on handedness.
            ///// </summary>
            //[Preserve, InputControl(aliases = new[] { "BTouched", "YTouched", "BTouch", "YTouch", "buttonBTouched", "buttonYTouched" }, usage = "SecondaryTouch")]
            //public ButtonControl secondaryTouched { get; private set; }

            ///// <summary>
            ///// A [AxisControl](xref:UnityEngine.InputSystem.Controls.AxisControl) that represents the <see cref="RayNeoControllerProfile.trigger"/> OpenXR binding.
            ///// </summary>
            //[Preserve, InputControl(usage = "Trigger")]
            //public AxisControl trigger { get; private set; }

            ///// <summary>
            ///// A [ButtonControl](xref:UnityEngine.InputSystem.Controls.ButtonControl) that represents the <see cref="RayNeoControllerProfile.triggerClick"/> OpenXR binding.
            ///// </summary>
            //[Preserve, InputControl(aliases = new[] { "indexButton", "indexTouched", "triggerbutton" }, usage = "TriggerButton")]
            //public ButtonControl triggerPressed { get; private set; }

            ///// <summary>
            ///// A [ButtonControl](xref:UnityEngine.InputSystem.Controls.ButtonControl) that represents the <see cref="RayNeoControllerProfile.triggerTouch"/> OpenXR binding.
            ///// </summary>
            //[Preserve, InputControl(aliases = new[] { "indexTouch", "indexNearTouched" }, usage = "TriggerTouch")]
            //public ButtonControl triggerTouched { get; private set; }

            ///// <summary>
            ///// A [ButtonControl](xref:UnityEngine.InputSystem.Controls.ButtonControl) that represents the <see cref="RayNeoControllerProfile.thumbstickClick"/> OpenXR binding.
            ///// </summary>
            //[Preserve, InputControl(aliases = new[] { "JoystickOrPadPressed", "thumbstickClick", "joystickClicked" }, usage = "Primary2DAxisClick")]
            //public ButtonControl thumbstickClicked { get; private set; }

            ///// <summary>
            ///// A [ButtonControl](xref:UnityEngine.InputSystem.Controls.ButtonControl) that represents the <see cref="RayNeoControllerProfile.thumbstickTouch"/> OpenXR binding.
            ///// </summary>
            //[Preserve, InputControl(aliases = new[] { "JoystickOrPadTouched", "thumbstickTouch", "joystickTouched" }, usage = "Primary2DAxisTouch")]
            //public ButtonControl thumbstickTouched { get; private set; }

            /// <summary>
            /// A <see cref="PoseControl"/> that represents the <see cref="RayNeoControllerProfile.grip"/> OpenXR binding.
            /// </summary>


            ///// <summary>
            ///// A <see cref="PoseControl"/> that represents the <see cref="RayNeoControllerProfile.aim"/> OpenXR binding.
            ///// </summary>
            //[Preserve, InputControl(offset = 0, alias = "aimPose", usage = "Pointer")]
            //public PoseControl pointer { get; private set; }

            /// <summary>
            /// A [ButtonControl](xref:UnityEngine.InputSystem.Controls.ButtonControl) required for backwards compatibility with the XRSDK layouts. This represents the overall tracking state of the device. This value is equivalent to mapping devicePose/isTracked.
            /// </summary>
            //[Preserve, InputControl(offset = 28, usage = "IsTracked")]
            //public ButtonControl isTracked { get; private set; }

            ///// <summary>
            ///// A [IntegerControl](xref:UnityEngine.InputSystem.Controls.IntegerControl) required for backwards compatibility with the XRSDK layouts. This represents the bit flag set to indicate what data is valid. This value is equivalent to mapping devicePose/trackingState.
            ///// </summary>
            //[Preserve, InputControl(offset = 32, usage = "TrackingState")]
            //public IntegerControl trackingState { get; private set; }

            ///// <summary>
            ///// A [Vector3Control](xref:UnityEngine.InputSystem.Controls.Vector3Control) required for backwards compatibility with the XRSDK layouts. This is the device position. For the RayNeo device, this is both the grip and the pointer position. This value is equivalent to mapping devicePose/position.
            ///// </summary>


            ///// <summary>
            ///// A [Vector3Control](xref:UnityEngine.InputSystem.Controls.Vector3Control) required for back compatibility with the XRSDK layouts. This is the pointer position. This value is equivalent to mapping pointerPose/position.
            ///// </summary>
            //[Preserve, InputControl(offset = 96)]
            //public Vector3Control pointerPosition { get; private set; }

            ///// <summary>
            ///// A [QuaternionControl](xref:UnityEngine.InputSystem.Controls.QuaternionControl) required for backwards compatibility with the XRSDK layouts. This is the pointer rotation. This value is equivalent to mapping pointerPose/rotation.
            ///// </summary>
            //[Preserve, InputControl(offset = 108, alias = "pointerOrientation")]
            //public QuaternionControl pointerRotation { get; private set; }

            ///// <summary>
            ///// A <see cref="HapticControl"/> that represents the <see cref="RayNeoControllerProfile.haptic"/> binding.
            ///// </summary>
            //[Preserve, InputControl(usage = "Haptic")]
            //public HapticControl haptic { get; private set; }

            /// <summary>
            /// Internal call used to assign controls to the the correct element.
            /// </summary>
            protected override void FinishSetup()
            {
                base.FinishSetup();

                //homeButton = GetChildControl<ButtonControl>("homeButton");


                //padButton = GetChildControl<ButtonControl>("padButton");
                // position = GetChildControl<Vector2Control>("touchPad");
                // rotation = GetChildControl<QuaternionControl>("ringPos");
                //trigger = GetChildControl<AxisControl>("trigger");

                //triggerPressed = GetChildControl<ButtonControl>("triggerPressed");
                //triggerTouched = GetChildControl<ButtonControl>("triggerTouched");
                //grip = GetChildControl<AxisControl>("grip");
                //gripPressed = GetChildControl<ButtonControl>("gripPressed");
                //menu = GetChildControl<ButtonControl>("menu");
                //primaryButton = GetChildControl<ButtonControl>("primaryButton");
                //primaryTouched = GetChildControl<ButtonControl>("primaryTouched");
                //secondaryButton = GetChildControl<ButtonControl>("secondaryButton");
                //secondaryTouched = GetChildControl<ButtonControl>("secondaryTouched");
                //thumbstickClicked = GetChildControl<ButtonControl>("thumbstickClicked");
                //thumbstickTouched = GetChildControl<ButtonControl>("thumbstickTouched");

                //pointer = GetChildControl<PoseControl>("pointer");

                //isTracked = GetChildControl<ButtonControl>("isTracked");
                //trackingState = GetChildControl<IntegerControl>("trackingState");
                //pointerPosition = GetChildControl<Vector3Control>("pointerPosition");
                //pointerRotation = GetChildControl<QuaternionControl>("pointerRotation");

                //haptic = GetChildControl<HapticControl>("haptic");
            }
        }

        public const string profile = "/interaction_profiles/rayneo/rayneo_controller";

        // Available Bindings
        // Left Hand Only
        /// <summary>
        /// Constant for a boolean interaction binding '.../input/x/click' OpenXR Input Binding. Used by input subsystem to bind actions to physical inputs. This binding is only available for the <see cref="OpenXRInteractionFeature.UserPaths.leftHand"/> user path.
        /// </summary>
        //  public const string buttonX = "/input/x/click";

        public const string homeButton = "/input/thumbstick/click";






        ///// <summary>
        ///// Constant for a boolean interaction binding '.../input/x/touch' OpenXR Input Binding. Used by input subsystem to bind actions to physical inputs. This binding is only available for the <see cref="OpenXRInteractionFeature.UserPaths.leftHand"/> user path.
        ///// </summary>
        //public const string buttonXTouch = "/input/x/touch";
        ///// <summary>
        ///// Constant for a boolean interaction binding '.../input/y/click' OpenXR Input Binding. Used by input subsystem to bind actions to physical inputs. This binding is only available for the <see cref="OpenXRInteractionFeature.UserPaths.leftHand"/> user path.
        ///// </summary>
        //public const string buttonY = "/input/y/click";
        ///// <summary>
        ///// Constant for a boolean interaction binding '.../input/y/touch' OpenXR Input Binding. Used by input subsystem to bind actions to physical inputs. This binding is only available for the <see cref="OpenXRInteractionFeature.UserPaths.leftHand"/> user path.
        ///// </summary>
        //public const string buttonYTouch = "/input/y/touch";

        //// Right Hand Only
        ///// <summary>
        ///// Constant for a boolean interaction binding '.../input/a/click' OpenXR Input Binding. Used by input subsystem to bind actions to physical inputs. This binding is only available for the <see cref="OpenXRInteractionFeature.UserPaths.rightHand"/> user path.
        ///// </summary>
        //public const string buttonA = "/input/a/click";
        ///// <summary>
        ///// Constant for a boolean interaction binding '.../input/a/touch' OpenXR Input Binding. Used by input subsystem to bind actions to physical inputs. This binding is only available for the <see cref="OpenXRInteractionFeature.UserPaths.rightHand"/> user path.
        ///// </summary>
        //public const string buttonATouch = "/input/a/touch";
        ///// <summary>
        ///// Constant for a boolean interaction binding '..."/input/b/click' OpenXR Input Binding. Used by input subsystem to bind actions to physical inputs. This binding is only available for the <see cref="OpenXRInteractionFeature.UserPaths.rightHand"/> user path.
        ///// </summary>
        //public const string buttonB = "/input/b/click";
        ///// <summary>
        ///// Constant for a boolean interaction binding '.../input/b/touch' OpenXR Input Binding. Used by input subsystem to bind actions to physical inputs. This binding is only available for the <see cref="OpenXRInteractionFeature.UserPaths.rightHand"/> user path.
        ///// </summary>
        //public const string buttonBTouch = "/input/b/touch";

        //// Both Hands
        ///// <summary>
        ///// Constant for a boolean interaction binding '.../input/menu/click' OpenXR Input Binding. Used by input subsystem to bind actions to physical inputs.
        ///// </summary>
        //public const string menu = "/input/menu/click";
        ///// <summary>
        ///// Constant for a boolean interaction binding '.../input/system/click' OpenXR Input Binding. Used by input subsystem to bind actions to physical inputs.(may not be available for application use)
        ///// </summary>
        //public const string system = "/input/system/click";
        ///// <summary>
        ///// Constant for a float interaction binding '.../input/trigger/click' OpenXR Input Binding. Used by input subsystem to bind actions to physical inputs.
        ///// </summary>
        //public const string triggerClick = "/input/trigger/click";
        ///// <summary>
        ///// Constant for a float interaction binding '.../input/trigger/value' OpenXR Input Binding. Used by input subsystem to bind actions to physical inputs.
        ///// </summary>
        //public const string trigger = "/input/trigger/value";
        ///// <summary>
        ///// Constant for a boolean interaction binding '.../input/trigger/touch' OpenXR Input Binding. Used by input subsystem to bind actions to physical inputs.
        ///// </summary>
        //public const string triggerTouch = "/input/trigger/touch";
        ///// <summary>
        ///// Constant for a Vector2 interaction binding '.../input/thumbstick' OpenXR Input Binding. Used by input subsystem to bind actions to physical inputs.
        ///// </summary>
        //public const string thumbstick = "/input/thumbstick";
        ///// <summary>
        ///// Constant for a boolean interaction binding '.../input/thumbstick/click' OpenXR Input Binding. Used by input subsystem to bind actions to physical inputs.
        ///// </summary>
        //public const string thumbstickClick = "/input/thumbstick/click";
        ///// <summary>
        ///// Constant for a boolean interaction binding '.../input/thumbstick/touch' OpenXR Input Binding. Used by input subsystem to bind actions to physical inputs.
        ///// </summary>
        //public const string thumbstickTouch = "/input/thumbstick/touch";
        ///// <summary>
        ///// Constant for a float interaction binding '.../input/squeeze/click' OpenXR Input Binding. Used by input subsystem to bind actions to physical inputs.
        ///// </summary>
        //public const string squeezeClick = "/input/squeeze/click";
        ///// <summary>
        ///// Constant for a float interaction binding '.../input/squeeze/value' OpenXR Input Binding. Used by input subsystem to bind actions to physical inputs.
        ///// </summary>
        //public const string squeezeValue = "/input/squeeze/value";
        /// <summary>
        /// Constant for a pose interaction binding '.../input/grip/pose' OpenXR Input Binding. Used by input subsystem to bind actions to physical inputs.
        /// </summary>
        public const string grip = "/input/grip/pose";
        public const string anim = "/input/aim/pose";
        public const string gazePos = "/input/gaze_ext/pose";

        ///// <summary>
        ///// Constant for a pose interaction binding '.../input/aim/pose' OpenXR Input Binding. Used by input subsystem to bind actions to physical inputs.
        ///// </summary>
        //public const string aim = "/input/aim/pose";
        ///// <summary>
        ///// Constant for a haptic interaction binding '.../output/haptic' OpenXR Input Binding. Used by input subsystem to bind actions to physical inputs.
        ///// </summary>
        //public const string haptic = "/output/haptic";

        private const string kDeviceLocalizedName = "RayNeo Controller OpenXR";
        private const string kDeviceLocalizedRingName = "RayNeo Controller OpenXR Ring";
        private const string kDeviceLocalizedCellPhoneName = "RayNeo Controller OpenXR CellPhone";
        private const string kDeviceLocalizedEyeName = "RayNeo Controller OpenXR Eye";
        private const string kDeviceLocalizedHeadName = "RayNeo Controller OpenXR Head";


        /// <summary>
        /// The OpenXR Extension string. This extension defines the interaction profile for RayNeo controllers.
        /// /// </summary>
        public const string extensionString = "";

        /// <inheritdoc/>
        protected override void RegisterDeviceLayout()
        {

            InputSystem.InputSystem.RegisterLayout(typeof(RayNeoController),
                        matches: new InputDeviceMatcher()
                        .WithInterface(XRUtilities.InterfaceMatchAnyVersion)
                        .WithProduct(kDeviceLocalizedName));

            InputSystem.InputSystem.RegisterLayout(typeof(RayNeoRingController),
    matches: new InputDeviceMatcher()
    .WithInterface(XRUtilities.InterfaceMatchAnyVersion)
    .WithProduct(kDeviceLocalizedRingName));


            InputSystem.InputSystem.RegisterLayout(typeof(RayNeoCellPhoneController),
    matches: new InputDeviceMatcher()
    .WithInterface(XRUtilities.InterfaceMatchAnyVersion)
    .WithProduct(kDeviceLocalizedCellPhoneName));

            InputSystem.InputSystem.RegisterLayout(typeof(RayNeoEyeController),
matches: new InputDeviceMatcher()
.WithInterface(XRUtilities.InterfaceMatchAnyVersion)
.WithProduct(kDeviceLocalizedEyeName));

        }

        /// <inheritdoc/>
        protected override void UnregisterDeviceLayout()
        {
            InputSystem.InputSystem.RemoveLayout(nameof(RayNeoController));
            InputSystem.InputSystem.RemoveLayout(nameof(RayNeoRingController));
            InputSystem.InputSystem.RemoveLayout(nameof(RayNeoCellPhoneController));
            InputSystem.InputSystem.RemoveLayout(nameof(RayNeoEyeController));

        }

        /// <inheritdoc/>
        protected override void RegisterActionMapsWithRuntime()
        {
            AddRingActionMap();
            AddEyeActionMap();
            AddCellPhoneActionMap();
            //AddHeadActionMap();
        }

        private void AddCellPhoneActionMap()
        {
            ActionMapConfig actionMap = new ActionMapConfig()
            {
                name = "RayNeoCellPhoneController",
                localizedName = kDeviceLocalizedCellPhoneName,
                desiredInteractionProfile = profile,
                manufacturer = "RayNeo",
                serialNumber = "",
                deviceInfos = new List<DeviceConfig>()
                {
                    new DeviceConfig()
                    {
                        characteristics = (InputDeviceCharacteristics)( InputDeviceCharacteristics.TrackedDevice | InputDeviceCharacteristics.Controller ),
                        userPath = RayNeoUserPaths.cellphone,

                    },
                },
                actions = new List<ActionConfig>()
                {

                       new ActionConfig()
                    {
                        name = "devicePose",
                        localizedName = "Device Pose",
                        type = ActionType.Pose,
                        usages = new List<string>()
                        {
                            "Device"
                        },
                        bindings = new List<ActionBinding>()
                        {
                            new ActionBinding()
                            {
                                interactionPath = grip,
                                interactionProfileName = profile,
                            }
                        }
                    } ,
                }
            };

            AddActionMap(actionMap);
        }
        private void AddEyeActionMap()
        {
            ActionMapConfig actionMap = new ActionMapConfig()
            {
                name = "RayNeoEyeController",
                localizedName = kDeviceLocalizedEyeName,
                desiredInteractionProfile = profile,
                manufacturer = "RayNeo",
                serialNumber = "",
                deviceInfos = new List<DeviceConfig>()
                {
                    new DeviceConfig()
                    {
                        characteristics = (InputDeviceCharacteristics)( InputDeviceCharacteristics.TrackedDevice | InputDeviceCharacteristics.Controller ),
                        userPath = RayNeoUserPaths.eye_gaze,

                    },
                },
                actions = new List<ActionConfig>()
                {

                       new ActionConfig()
                    {
                        name = "eyePos",
                        localizedName = "EyePos",
                        type = ActionType.Pose,
                        usages = new List<string>()
                        {
                            "eye"
                        },
                        bindings = new List<ActionBinding>()
                        {
                            new ActionBinding()
                            {
                                interactionPath = gazePos,
                                interactionProfileName = profile,
                            }
                        }
                    } ,
                }
            };

            AddActionMap(actionMap);
        }

        private void AddRingActionMap()
        {
            ActionMapConfig actionMap = new ActionMapConfig()
            {
                name = "RayNeoRingController",
                localizedName = kDeviceLocalizedRingName,
                desiredInteractionProfile = profile,
                manufacturer = "RayNeo",
                serialNumber = "",
                deviceInfos = new List<DeviceConfig>()
                {
                    new DeviceConfig()
                    {
                        characteristics = (InputDeviceCharacteristics)( InputDeviceCharacteristics.TrackedDevice | InputDeviceCharacteristics.Controller ),
                        userPath = RayNeoUserPaths.ring,

                    },
                },
                actions = new List<ActionConfig>()
                {

                       new ActionConfig()
                    {
                        name = "homeButton",
                        localizedName = "HomeButton",
                        type = ActionType.Binary,
                        usages = new List<string>()
                        {
                            "Home"
                        },
                        bindings = new List<ActionBinding>()
                        {
                            new ActionBinding()
                            {
                                interactionPath = homeButton,
                                interactionProfileName = profile,
                            }
                        }
                    } ,
                    new ActionConfig()
                    {
                        name = "devicePose",
                        localizedName = "Device Pose",
                        type = ActionType.Pose,
                        usages = new List<string>()
                        {
                            "Device"
                        },
                        bindings = new List<ActionBinding>()
                        {
                            new ActionBinding()
                            {
                                interactionPath = grip,
                                interactionProfileName = profile,
                            }
                        }
                    },
                }
            };
            AddActionMap(actionMap);
        }

    }

    public static class RayNeoUserPaths
    {
        public const string ring = "/user/ring";
        public const string cellphone = "/user/cellphone";
        public const string eye_gaze = "/user/eyes_ext";

    }



}
