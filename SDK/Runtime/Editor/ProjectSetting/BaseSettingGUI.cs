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

    public class BaseSettingGUI : RayNeoProjectSettingGUIBase
    {
        GUIStyle m_itemTitleStyle = new GUIStyle();

        public BaseSettingGUI()
        {
            m_itemTitleStyle.fontStyle = FontStyle.Bold;
            m_itemTitleStyle.normal.textColor = Color.white;
        }

        public override void GUI(RayNeo.API.DeviceType dType, GUIStyle titleStyle)
        {
            EditorGUILayout.LabelField("Basic", titleStyle, GUILayout.ExpandWidth(false));
            //var cfg = RayNeoXRGeneralSettings.Instance.GetSimpleTouchConfig(dType);
            EditorGUILayout.BeginHorizontal();
            DrawDKBase();
            DrawDevices();
            EditorGUILayout.EndHorizontal();
        }

        private void DrawDKBase()
        {
            var bcfg = RayNeoXRGeneralSettings.Instance.baseConfig;
            EditorGUILayout.BeginVertical("frameBox", GUILayout.Width(300));
            bcfg.LogLevel = (LogLevel)EditorGUILayout.EnumPopup("Log Level", bcfg.LogLevel);

            bcfg.SleepTimeOut = EditorGUILayout.IntSlider(new GUIContent("Sleep Time Out", "-1 for SleepTimeout.NeverSleep"), bcfg.SleepTimeOut, -1, 6000);
            bcfg.ErrorWindow = EditorGUILayout.ToggleLeft("Error Window", bcfg.ErrorWindow);
            EditorGUILayout.EndVertical();
        }

        private Vector2 m_scrollPos;
        private void DrawDevices()
        {
            int height = 114;
            m_scrollPos = EditorGUILayout.BeginScrollView(m_scrollPos, true, false, GUILayout.Height(height));
            EditorGUILayout.BeginHorizontal();
            for (int i = 0; i < RayNeoXRGeneralSettings.Instance.Devices.Count; i++)
            {
                DrawDevice(RayNeoXRGeneralSettings.Instance.Devices[i], height);
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndScrollView();
        }
        private void DrawDevice(DeviceInfo di, int heigh)
        {
            EditorGUILayout.BeginVertical("frameBox", GUILayout.Width(220), GUILayout.Height(heigh - 24));
            EditorGUILayout.LabelField(di.DeviceName, m_itemTitleStyle, GUILayout.ExpandWidth(false));
            EditorGUILayout.LabelField("Resolution:\t" + di.Width + "X" + di.Height);
            EditorGUILayout.LabelField("FOV:\t\t" + di.FOV);
            //GUILayout.Space(20);
            if (GUILayout.Button("Set to the current debug environment"))
            {
                Camera.main.fieldOfView = di.FOV;
                GameViewResolution.SwitchOrientation(di.Width, di.Height, "RayNeo_" + di.Width + "X" + di.Height);
            }
            EditorGUILayout.EndVertical();
        }

    }



}