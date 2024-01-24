using RayNeo;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Rayneo
{

    public abstract class RayNeoProjectSettingGUIBase
    {
        public abstract void GUI(RayNeo.API.DeviceType dType, GUIStyle titleStyle);
    }


}