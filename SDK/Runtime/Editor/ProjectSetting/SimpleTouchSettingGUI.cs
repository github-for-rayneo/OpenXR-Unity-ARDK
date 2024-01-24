using RayNeo;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Rayneo
{

    public class SimpleTouchSettingGUI : RayNeoProjectSettingGUIBase
    {
        //private ReorderableList m_OrderedList;

        private RayNeo.API.DeviceType m_deviceType = RayNeo.API.DeviceType.X2_Normal;

        //private RayNeoType m_curType;

        public SimpleTouchSettingGUI()
        {
        }
        public override void GUI(RayNeo.API.DeviceType dType, GUIStyle titleStyle)
        {

            EditorGUILayout.LabelField("Simple Touch Configs", titleStyle, GUILayout.ExpandWidth(false));
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Choose device", GUILayout.Width(100));
            m_deviceType = (RayNeo.API.DeviceType)EditorGUILayout.EnumPopup(m_deviceType, GUILayout.Width(200));
            EditorGUILayout.EndHorizontal();

            var cfg = RayNeoXRGeneralSettings.Instance.GetSimpleTouchConfig(m_deviceType);

            //EditorGUI.BeginDisabledGroup(true);
            //EditorGUILayout.enum
            //EditorGUI.EndDisabledGroup();
            //双击之间的间隔.该数值会影响单击响应速度
            cfg.MulitTapSpacing = EditorGUILayout.Slider(new GUIContent("Mulit Tap Spacing", "If it is 0, it means that events such as double clicking will not be executed"), cfg.MulitTapSpacing, 0, 1);

            cfg.MovingThreshold = (uint)EditorGUILayout.IntSlider("Moving Threshold", (int)cfg.MovingThreshold, 0, 200);


            cfg.SwipeEndThreshold = (uint)EditorGUILayout.IntSlider("Swipe End Threshold", (int)cfg.SwipeEndThreshold, (int)cfg.MovingThreshold + 1, 300);
            //EditorGUILayout.BeginHorizontal();
            //EditorGUILayout.LabelField(new GUIContent("Release SimpleTap Immediately", "Will immediately callback the event upon clicking, even if there are other gestures in the future"), GUILayout.Width(200));
            cfg.ReleaseSimpleTapImmediately = EditorGUILayout.ToggleLeft(new GUIContent("Release SimpleTap Immediately", "Will immediately callback the event upon clicking, even if there are other gestures in the future"), cfg.ReleaseSimpleTapImmediately);

            cfg.ReleaseLastTapOnChangeToMove = EditorGUILayout.ToggleLeft(new GUIContent("Release Last TapEvent On Change To Move", "For example, if you lift your hand after clicking, and your fingers touch and slide again, the last click event will be immediately released"), cfg.ReleaseLastTapOnChangeToMove);

            //EditorGUILayout.EndHorizontal();
            //cfg.MulitTapSpacing = EditorGUILayout.FloatField("MulitTapSpacing", cfg.MulitTapSpacing);

            //EditorGUILayout.PropertyField
            //m_currentToolBarIndex = GUILayout.Toolbar(m_currentToolBarIndex, types);
            //m_curType = (RayNeoType)EditorGUILayout.EnumPopup(new GUIContent("Device Type"), RayNeoType.X2, GUILayout.Width(200));
            //EditorGUILayout.ObjectField(setting.TouchConfigs);
            //setting.TouchConfig
        }
    }


}