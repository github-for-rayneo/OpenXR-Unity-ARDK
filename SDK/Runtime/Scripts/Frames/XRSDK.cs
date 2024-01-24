using UnityEngine;
using FfalconXR;

namespace RayNeo
{
    public class XRSDK
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Load()
        {
            Initialize();
        }

        private static void Initialize()
        {
            Log.Level = RayNeoXRGeneralSettings.Instance.baseConfig.LogLevel;
            Screen.sleepTimeout = RayNeoXRGeneralSettings.Instance.baseConfig.SleepTimeOut;

            Log.Init();
        }
    }
}
