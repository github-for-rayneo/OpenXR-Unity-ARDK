#if UNITY_EDITOR

using UnityEditor;
using UnityEditor.XR.OpenXR.Features;
#endif

using UnityEngine;
using UnityEngine.XR.OpenXR;

namespace Unity.XR.RayNeo.OpenXR.ARDK
{
#if UNITY_EDITOR
    [OpenXRFeature(UiName = featureName,
        BuildTargetGroups = new[] { BuildTargetGroup.Android, /*BuildTargetGroup.Standalone */},
        CustomRuntimeLoaderBuildTargets = new[] { BuildTarget.Android },
        Company = "RayNeo",
        Desc = "Sample empty feature template",
        DocumentationLink = "",
        FeatureId = featureId)]
#endif
    public class EmptyFeatureTemplate : RayNeoOpenXRFeatureBase
    {
        public const string featureName = "Empty Feature Template";
        public const string featureId = "com.unity.openxr.feature.empty.feature.template";
    }
}