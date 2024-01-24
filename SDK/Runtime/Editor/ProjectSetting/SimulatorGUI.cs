using FfalconXR;
using RayNeo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Rayneo
{

    public class SimulatorGUI : RayNeoProjectSettingGUIBase
    {
        public override void GUI(RayNeo.API.DeviceType dType, GUIStyle titleStyle)
        {
            EditorGUILayout.LabelField("Simulator", titleStyle, GUILayout.ExpandWidth(false));

            DebugMidway.IsInSimulator = EditorGUILayout.ToggleLeft(new GUIContent("Simulator Enabled"), DebugMidway.IsInSimulator);
            if (!DebugMidway.IsInSimulator)
            {
                return;
            }
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Simulator device", GUILayout.Width(100));
            DebugMidway.SetDeviceType((RayNeo.API.DeviceType)EditorGUILayout.EnumPopup(DebugMidway.GetDeviceType(), GUILayout.Width(200)));
            EditorGUILayout.EndHorizontal();
        }
    }
}