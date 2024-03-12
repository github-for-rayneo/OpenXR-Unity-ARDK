using RayNeo;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Rayneo
{

    public static class RayNeoProjectSetting
    {
        //public RayNeoType type;
        //public enum RayNeoType
        //{
        //    X2 = 0,
        //    AIR_PLUS = 1,
        //    AIR2 = 2
        //}
        [SettingsProvider]
        public static SettingsProvider GetSettingsProvider()
        {
            //InputSystemUIInputModule
            return new RayNeoSettingsProvider();
        }
    }

    class RayNeoSettingsProvider : SettingsProvider
    {
        public const string GeneralSettings = "Assets/XR/RayNeoGeneralSettings.asset";

        //public SimpleTouchSettingGUI m_gui;
        private Dictionary<int, RayNeoProjectSettingGUIBase> m_GuiItems;

        //private int m_currentToolBarIndex = 0;
        private RayNeo.API.DeviceType m_deviceType = RayNeo.API.DeviceType.X2_Normal;

        GUIStyle m_ItemTitleStyle = new GUIStyle();

        //private RayNeoXRGeneralSettings m_settings;
        private Texture m_Logo;

        public static void CreateGeneralSettings()
        {
            if (RayNeoXRGeneralSettings.Instance == null)
            {
                RayNeoXRGeneralSettings[] settings = Resources.FindObjectsOfTypeAll<RayNeoXRGeneralSettings>();
                if (settings != null && settings.Length > 0)
                {
                    RayNeoXRGeneralSettings.Instance = settings[0];
                }

            }
            if (RayNeoXRGeneralSettings.Instance == null && File.Exists(GeneralSettings))
            {
                RayNeoXRGeneralSettings.Instance = AssetDatabase.LoadAssetAtPath<RayNeoXRGeneralSettings>(GeneralSettings);
            }
            if (RayNeoXRGeneralSettings.Instance == null)
            {
                CreateNewSettingsAsset(GeneralSettings);
            }
        }
        public RayNeoSettingsProvider(IEnumerable<string> keywords = null) : base("Project/RayNeo", SettingsScope.Project, keywords)
        {

            m_ItemTitleStyle.fontStyle = FontStyle.Bold;
            m_ItemTitleStyle.fontSize = 16;
            m_ItemTitleStyle.normal.textColor = Color.white;
            m_GuiItems = new Dictionary<int, RayNeoProjectSettingGUIBase>();
            m_GuiItems.Add(0, new BaseSettingGUI());
            m_GuiItems.Add(1, new SimpleTouchSettingGUI());
            m_GuiItems.Add(2, new SimulatorGUI());
            m_GuiItems.Add(3, new ExtensionGUI());


            m_Logo = AssetDatabase.LoadAssetAtPath<Texture>("Assets/Runtime/Editor/Textures/LOGO.png");
            ConfigPreloadInfo();
            guiHandler = ONGUI;
            deactivateHandler = OnDeactive;
            titleBarGuiHandler = () =>
            {
                GUILayout.Label(m_Logo, GUILayout.Width(512 * 0.3f), GUILayout.Height(112 * 0.3f));

            };
        }

        private void OnDeactive()
        {
            SaveCfg();
        }

        private void SaveCfg()
        {
            EditorUtility.SetDirty(RayNeoXRGeneralSettings.Instance);
            AssetDatabase.SaveAssets();
            EditorUtility.ClearDirty(RayNeoXRGeneralSettings.Instance);
        }
        private void ONGUI(string serachContext)
        {


            //GUILayout.BeginVertical("frameBox");
            //EditorGUILayout.BeginFoldoutHeaderGroup(foldout, "打包SDK配置");
            //GUILayout.BeginArea(new Rect(0, 30, 100, 200));
            //EditorGUILayout.LabelField("asdfasdfsadf");
            //GUILayout.EndArea();
            //GUILayout.EndVertical();
            //EditorGUILayout.TextArea
            if (m_deviceType == RayNeo.API.DeviceType.NONE)
            {
                return;
            }

            foreach (var item in m_GuiItems)
            {
                EditorGUILayout.BeginVertical("frameBox");
                item.Value.GUI(m_deviceType, m_ItemTitleStyle);
                EditorGUILayout.EndVertical();
            }

            //m_currentToolBarIndex = GUILayout.Toolbar(m_currentToolBarIndex, m_toolBars);
            //m_guiItems[m_currentToolBarIndex].GUI(m_deviceType);
        }

        //总不能连设置都不点开吧....
        private static void CreateNewSettingsAsset(string relativePath)
        {
            var settings = ScriptableObject.CreateInstance<RayNeoXRGeneralSettings>();
            settings.CreateDefaultData();
            AssetDatabase.CreateAsset(settings, relativePath);
            EditorGUIUtility.PingObject(settings);
            //PlayerSettings.SetPreloadedAssets(assets.ToArray());
            RayNeoXRGeneralSettings.Instance = settings;


            //}
            //else
            //{
            //    CleanOldSettings();
            //}
        }
        private void ConfigPreloadInfo()
        {
            UnityEngine.Object[] preloadedAssets = PlayerSettings.GetPreloadedAssets();
            for (int i = 0; i < preloadedAssets.Length; i++)
            {
                if (preloadedAssets[i] != null && preloadedAssets[i].GetType() == typeof(RayNeoXRGeneralSettings))
                {
                    return;
                }
            }
            var assets = preloadedAssets.ToList();
            assets.Add(RayNeoXRGeneralSettings.Instance);
            PlayerSettings.SetPreloadedAssets(assets.ToArray());
        }

        //public partial class InputSettings : ScriptableObject

        //    // Create settings file.
        //    var settings = ScriptableObject.CreateInstance<InputSettings>();
        //    AssetDatabase.CreateAsset(settings, relativePath);
        //    EditorGUIUtility.PingObject(settings);
        //    // Install the settings. This will lead to an InputSystem.onSettingsChange event which in turn
        //    // will cause us to re-initialize.
        //    InputSystem.settings = settings;
        //}


    }



    //[CustomPropertyDrawer(typeof(DisableAttribute))]
    //public class DisableDrawer : PropertyDrawer
    //{
    //    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    //    {
    //        EditorGUI.BeginDisabledGroup(true);
    //        EditorGUI.PropertyField(position, property, label);
    //        EditorGUI.EndDisabledGroup();
    //    }
    //}

}