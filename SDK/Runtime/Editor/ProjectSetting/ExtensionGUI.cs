using FfalconXR;
using RayNeo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UIElements;

namespace Rayneo
{

    class ExtItemInfo
    {
        public bool IsUPM = true;
        public string Path;
        public string name;
        public string displayName;
        public string description;
        public string version;
    }

    public class ExtensionGUI : RayNeoProjectSettingGUIBase
    {

        public string ExtendsionPath = Path.GetFullPath(UnityEditor.AssetDatabase.GUIDToAssetPath(UnityEditor.AssetDatabase.FindAssets(typeof(ExtensionGUI).Name)[0]) + "/../../../../../Extension~");
        private List<ExtItemInfo> exts;
        private Dictionary<string, UnityEditor.PackageManager.PackageInfo> m_PackageInfos = new Dictionary<string, UnityEditor.PackageManager.PackageInfo>();
        private EditorWindow m_ProjectSettingsWindow;
        private VisualElement m_panel;

        private Vector2 m_ItemExtSize = new Vector2(250, 120);
        //private m_SettingPi;
        public ExtensionGUI()
        {
            if (!Directory.Exists(ExtendsionPath))
            {
                return;
            }
            var gvWndType = typeof(Editor).Assembly.GetType("UnityEditor.ProjectSettingsWindow");
            m_ProjectSettingsWindow = EditorWindow.GetWindow(gvWndType);
            FieldInfo settingContainerFi = m_ProjectSettingsWindow.GetType().BaseType.GetField("m_SettingsPanel", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            m_panel = (VisualElement)settingContainerFi.GetValue(m_ProjectSettingsWindow);
            string[] dirs = Directory.GetDirectories(ExtendsionPath);
            exts = new List<ExtItemInfo>();
            for (int i = 0; i < dirs.Length; i++)
            {
                string packageJson = dirs[i] + "/package.json";
                if (File.Exists(packageJson))
                {
                    ExtItemInfo extiInfo = JsonUtility.FromJson<ExtItemInfo>(File.ReadAllText(packageJson));
                    extiInfo.Path = dirs[i];
                    exts.Add(extiInfo);
                }
            }
            LoadPackageList();
            //var windows = UnityEngine.Resources.FindObjectsOfTypeAll<UnityEditor.EditorWindow>();
            //for (int i = 0; i < windows.Length; i++)
            //{
            //    var w = windows[i];

            //    if (w.GetType().Name.Equals("ProjectSettingsWindow"))
            //    {
            //        m_ProjectSettingsWindow = w;
            //        break;
            //    }
            //    //if (windows[i].titleContent.text.Contains("OpenXR"))
            //    //{
            //    //windows[i].Repaint();
            //    //}
            //}
        }

        ListRequest m_PackageListRequest;
        private void LoadPackageList()
        {
            m_PackageListRequest = Client.List();
        }
        public override void GUI(RayNeo.API.DeviceType dType, GUIStyle titleStyle)
        {
            EditorGUILayout.LabelField("Extension Packages", titleStyle, GUILayout.ExpandWidth(false));

            if (exts == null || exts.Count == 0)
            {
                return;
            }

            if (m_PackageListRequest != null)
            {
                if (m_PackageListRequest.IsCompleted)
                {
                    if (m_PackageListRequest.Status == StatusCode.Success)
                    {
                        m_PackageInfos.Clear();
                        foreach (var item in m_PackageListRequest.Result)
                        {
                            m_PackageInfos.Add(item.name, item);
                        }
                    }
                    m_PackageListRequest = null;
                }
            }
            float contentWidth = m_panel.contentContainer.layout.width - 40;

            int xCount = (int)(contentWidth / m_ItemExtSize.x);
            if (xCount == 0)
            {
                xCount = 1;
            }

            int extIndex = 0;


            EditorGUILayout.BeginVertical();

            while (extIndex < exts.Count)
            {
                EditorGUILayout.BeginHorizontal();
                for (int i = 0; i < xCount && extIndex < exts.Count; i++)
                {
                    DrawItemExt(exts[extIndex], Mathf.Max(contentWidth / xCount, m_ItemExtSize.x));
                    extIndex++;
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();
        }

        private void DrawItemExt(ExtItemInfo extInfo, float width)
        {
            EditorGUILayout.BeginVertical("frameBox", GUILayout.Width(width), GUILayout.Height(m_ItemExtSize.y));
            EditorGUILayout.LabelField(extInfo.displayName);
            if (extInfo.IsUPM)
            {
                EditorGUILayout.LabelField("package:" + extInfo.name);
                EditorGUILayout.LabelField("version:" + extInfo.version);
                EditorGUILayout.LabelField("description:\n\t" + extInfo.description, GUILayout.Height(30));
            }
            else
            {
                EditorGUILayout.Space(70);
            }
            if (GUILayout.Button(extInfo.IsUPM && m_PackageInfos.ContainsKey(extInfo.name) ? "Reimport" : "Import"))
            {
                //将文件放到packages
                EditorTool.DirectoryCopy(extInfo.Path, "Packages/" + extInfo.name, true);
            }
            EditorGUILayout.EndVertical();

        }
    }
}