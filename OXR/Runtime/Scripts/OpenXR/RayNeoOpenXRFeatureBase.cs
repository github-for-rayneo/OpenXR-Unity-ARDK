using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine.XR.OpenXR;
using UnityEngine.XR.OpenXR.Features;
using System.Diagnostics;
using System.Collections.Specialized;
using UnityEngine.XR.OpenXR.Features.Interactions;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Unity.XR.RayNeo.OpenXR.ARDK
{
    // Base class for all RayNeo OpenXR features.
    // Can be removed if it's not necessary to hold common logic for multiple features.
    public abstract class RayNeoOpenXRFeatureBase : OpenXRFeature
    {
        // public handles
        public ulong InstanceHandle { get; private set; }
        public ulong SessionHandle { get; private set; }
        public int SessionState { get; private set; }
        public ulong SystemIDHandle { get; private set; }
        public ulong SpaceHandle { get; private set; }

        protected override IntPtr HookGetInstanceProcAddr(IntPtr func)
        {
            return base.HookGetInstanceProcAddr(func);
        }

        protected override bool OnInstanceCreate(ulong instanceHandle)
        {
            InstanceHandle = instanceHandle;
            //GetInstanceProcAddrPtr = (GetInstanceProcAddrDelegate)Marshal.GetDelegateForFunctionPointer(xrGetInstanceProcAddr, typeof(GetInstanceProcAddrDelegate));
            //OnHookMethods();
            return true;
        }

        protected override void OnInstanceDestroy(ulong instanceHandle)
        {
            SystemIDHandle = 0;
            InstanceHandle = 0;
        }

        protected override void OnSystemChange(ulong systemIDHandle)
        {
            SystemIDHandle = systemIDHandle;
        }

        protected override void OnSessionCreate(ulong sessionHandle)
        {
            SessionHandle = sessionHandle;
        }

        protected override void OnSessionBegin(ulong sessionHandle)
        {
            //IsSessionRunning = true;
        }

        protected override void OnSessionStateChange(int oldState, int newState)
        {
            SessionState = newState;
        }

        protected override void OnSessionEnd(ulong sessionHandle)
        {
            //IsSessionRunning = false;
        }

        protected override void OnSessionDestroy(ulong sessionHandle)
        {
            SessionHandle = 0;
        }

        protected override void OnAppSpaceChange(ulong spaceHandle)
        {
            SpaceHandle = spaceHandle;
        }
#if UNITY_EDITOR

        protected override void GetValidationChecks(List<ValidationRule> rules, BuildTargetGroup targetGroup)
        {
            rules.Add(new ValidationRule(this)
            {
                message = "Please enable RayNeo interaction profile.",
                checkPredicate = () =>
                {
                    var settings = OpenXRSettings.GetSettingsForBuildTargetGroup(targetGroup);
                    if (null == settings)
                        return false;

                    bool rayneoInteractionProfileEnabled = false;
                    //bool otherInteractionProfileEnabled = false;
                    var f = settings.GetFeature(typeof(RayNeoControllerProfile));
                    if (f != null && f.enabled)
                    {
                        rayneoInteractionProfileEnabled = true;
                    }
                    return rayneoInteractionProfileEnabled;
                },
                error = true,
                fixIt = () =>
                {
                    var settings = OpenXRSettings.GetSettingsForBuildTargetGroup(targetGroup);
                    var f = settings.GetFeature(typeof(RayNeoControllerProfile));
                    f.enabled = true;
                    //UnityEngine.Resources.FindObjectsOfTypeAll(UnityEditor.ProjectSettingsWindow);
                    //var windows = UnityEngine.Resources.FindObjectsOfTypeAll<UnityEditor.EditorWindow>();
                    //for (int i = 0; i < windows.Length; i++)
                    //{
                    //    //if (windows[i].titleContent.text.Contains("OpenXR"))
                    //    //{
                    //        windows[i].Repaint();
                    //    //}
                    //}
                    SettingsService.NotifySettingsProviderChanged();
                    //EditorApplication.RepaintProjectWindow();
                    //SettingsService.OpenProjectSettings("Project/XR Plug-in Management/OpenXR");
                },
                fixItAutomatic = true,
                fixItMessage = "Auto add RayNeo interaction profile."
            });
        }
#endif

    }  // class RayNeoOpenXRFeatureBase
}  // namespace