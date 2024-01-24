using UnityEngine;
using FfalconXR;
using System.Linq;
using System;
using System.Collections.Generic;
using System.IO;
#if UNITY_EDITOR

using UnityEditor;
#endif

using Unity.Collections;

namespace RayNeo
{

    //[CreateAssetMenu(fileName = "RayNeoGeneralSettings", menuName = "RayNeo/RayNeo")]
    public class RayNeoExtensionInfo : ScriptableObject
    {
        public string Name;
        public string Introduce;

    }
}
