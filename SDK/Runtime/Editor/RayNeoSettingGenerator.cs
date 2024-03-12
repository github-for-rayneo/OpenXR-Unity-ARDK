using UnityEditor;
using Rayneo;

namespace RayNeo.Editor
{
    [InitializeOnLoad]
    public class RayNeoSettingGenerator
    {

        static RayNeoSettingGenerator()
        {
            EditorApplication.update += Update;
        }

        static void Update()
        {
            RayNeoSettingsProvider.CreateGeneralSettings();
            EditorApplication.update -= Update;

        }
    }
}

