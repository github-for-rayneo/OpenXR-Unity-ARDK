#if UNITY_EDITOR

using UnityEditor;
using UnityEditor.XR.OpenXR.Features;
#endif

using com.rayneo.xr.extensions;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR.OpenXR;
using UnityEngine.XR.OpenXR.Features;

namespace Unity.XR.RayNeo.OpenXR.ARDK
{
#if UNITY_EDITOR
    [OpenXRFeature(UiName = featureName,
        BuildTargetGroups = new[] { BuildTargetGroup.Android, /*BuildTargetGroup.Standalone*/ },
        CustomRuntimeLoaderBuildTargets = new[] { BuildTarget.Android },
        Company = "RayNeo",
        Desc = "Enable support for RayNeo AR glasses",
        DocumentationLink = "",
        FeatureId = featureId)]
#endif
    public class RayNeoSupportFeature : RayNeoOpenXRFeatureBase
    {
        public const string featureName = "RayNeo Support";
        public const string featureId = "com.unity.openxr.feature.rayneo.support";


        public bool OpenSLAMOnStart = false;
        public bool ATWSupport = false;

        // Called after xrCreateInstance
        protected override bool OnInstanceCreate(ulong xrInstanceHandle)
        {            
            base.OnInstanceCreate(xrInstanceHandle);

            Dictionary<string, int> settings = new Dictionary<string, int>();
            if (ATWSupport)
            {
                settings.Add("ATW", 1);
            }

            var renderMode = OpenXRSettings.Instance.renderMode;
            if (renderMode == OpenXRSettings.RenderMode.SinglePassInstanced)
            {
                settings.Add("renderMode", 1);
            }

            var depthMode = OpenXRSettings.Instance.depthSubmissionMode;
            if (depthMode == OpenXRSettings.DepthSubmissionMode.Depth16Bit)
            {
                settings.Add("depthSubmissionMode", 16);
            }
            else if (depthMode == OpenXRSettings.DepthSubmissionMode.Depth24Bit) 
            {
                settings.Add("depthSubmissionMode", 24);
            }


            XRInterfaces.SetBasicXRConfigs(settings);
            if (OpenSLAMOnStart)
            {
                com.rayneo.xr.extensions.XRInterfaces.EnableSlamHeadTracker();
            }
            return true;
        }

        // OpenXR feature is a ScriptableObject, called when loaded
        protected override void OnEnable()
        {
            //Debug.Log("ONENABLE!!!");
            base.OnEnable();
        }


        // Subsystem-related callbacks
        protected override void OnSubsystemCreate()
        {
            //Debug.Log("ONSUBSYSTEMCREATE");
        }

        protected override void OnSubsystemStart()
        {
            //Debug.Log("ONSUBSYSTEMSTART");
        }

        protected override void OnSubsystemStop()
        {
            //Debug.Log("ONSUBSYSTEMSTOP");
        }

        protected override void OnSubsystemDestroy()
        {
            //Debug.Log("ONSUBSYSTEMDESTROY");
        }
#if UNITY_EDITOR

        protected override void GetValidationChecks(List<ValidationRule> rules, BuildTargetGroup targetGroup)
        {
            base.GetValidationChecks(rules, targetGroup);

            //opengl 3 限制.  今后有vulkan 再取消限制
            rules.Add(new ValidationRule(this)
            {
                message = "The graphicsAPIs should be set to OpenGLES.",
                checkPredicate = () =>
                {
                    if (!PlayerSettings.GetUseDefaultGraphicsAPIs(BuildTarget.Android))
                    {
                        return true;
                    }
                    var graphics = PlayerSettings.GetGraphicsAPIs(BuildTarget.Android);
                    if (graphics != null && graphics.Length >= 1 && graphics[0] == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES3)
                    {
                        return true;
                    }
                    return false;
                },
                error = true,
                fixIt = () =>
                {
                    PlayerSettings.SetUseDefaultGraphicsAPIs(BuildTarget.Android, false);
                    PlayerSettings.SetGraphicsAPIs(BuildTarget.Android, new GraphicsDeviceType[1] { GraphicsDeviceType.OpenGLES3 });
                },
                fixItAutomatic = true,
                fixItMessage = "choose OpenGLES3."
            });
            AndroidSdkVersions minSdkVersion = AndroidSdkVersions.AndroidApiLevel30;


            //min api 30
            rules.Add(new ValidationRule(this)
            {
                message = "The minSdkVersion need to select " + minSdkVersion.ToString(),
                checkPredicate = () =>
                {
                    return PlayerSettings.Android.minSdkVersion >= minSdkVersion;
                },
                error = true,
                fixIt = () =>
                {
                    PlayerSettings.Android.minSdkVersion = minSdkVersion;
                },
                fixItAutomatic = true,
                fixItMessage = "choose minSdkVersion to " + minSdkVersion.ToString() + "."
            });
            AndroidSdkVersions targetSdkVersion = AndroidSdkVersions.AndroidApiLevel30;

            //target api 30
            rules.Add(new ValidationRule(this)
            {
                message = "The targetSdkVersion need to select " + minSdkVersion.ToString(),
                checkPredicate = () =>
                {
                    return PlayerSettings.Android.targetSdkVersion >= targetSdkVersion;
                },
                error = true,
                fixIt = () =>
                {
                    PlayerSettings.Android.targetSdkVersion = targetSdkVersion;
                },
                fixItAutomatic = true,
                fixItMessage = "choose targetSdkVersion to " + minSdkVersion.ToString() + "."
            });


            //landscape
            rules.Add(new ValidationRule(this)
            {
                message = "In order to display correct on mobile devices, the orientation should be set to LandscapeLeft. ",
                checkPredicate = () =>
                {
                    return PlayerSettings.defaultInterfaceOrientation == UIOrientation.LandscapeLeft;
                },
                error = false,
                fixIt = () =>
                {
                    PlayerSettings.defaultInterfaceOrientation = UIOrientation.LandscapeLeft;
                },
                fixItAutomatic = true,
                fixItMessage = "choose targetSdkVersion to " + minSdkVersion.ToString() + "."
            });

        }
#endif

    }
}