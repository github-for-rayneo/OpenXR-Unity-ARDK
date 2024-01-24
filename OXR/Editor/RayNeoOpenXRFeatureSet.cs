using UnityEditor;
using UnityEditor.XR.OpenXR.Features;


namespace Unity.XR.RayNeo.OpenXR.ARDK.Editor
{
    [OpenXRFeatureSet(
        FeatureIds = new[]
        {
            RayNeoSupportFeature.featureId,
            //EmptyFeatureTemplate.featureId,
            //SLAMFeature.featureId
        },
        RequiredFeatureIds = new[]
        {
            RayNeoSupportFeature.featureId,

        },
        UiName = "RayNeo XR",
        Description = "Feature group that implements RayNeo's capabilities.",
        FeatureSetId = "com.unity.xr.rayneo.openxr",
        SupportedBuildTargets = new[]
        {
            BuildTargetGroup.Android
        }
    )]
    internal class RayNeoOpenXRFeatureSet
    {
    }
}