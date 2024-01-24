using System;
using UnityEngine;
using UnityEditor;
using System.Threading;

namespace RayNeo.Editor
{
    /// <summary>
    /// ���µ���XRSDKCore,���������������
    /// </summary>
    [InitializeOnLoad]
    public class XRSDKCoreDllImport
    {
        private static string m_Flag = "";
        private static bool m_NeedImport = false;
        private static bool m_CanImport = false;

        static XRSDKCoreDllImport()
        {
            m_Flag = System.DateTime.Now.ToString("yyyy.MM.dd");
            EditorApplication.update += Update;
            Thread thread = new Thread(() =>
            {
                Thread.Sleep(3000);
                m_CanImport = true;
            });
            thread.Start();
            m_NeedImport = EditorPrefs.GetBool(m_Flag) == false;
        }

        static void Update()
        {
            if (m_CanImport)
            {
                m_CanImport = false;
                if (m_NeedImport)
                {
                    Debug.Log("Reimport UnityXRSDKCore.dll");
                    AssetDatabase.ImportAsset("FFALCON/XRSDK/Plugins/UnityXRSDKCore.dll");
                }
                EditorPrefs.SetBool(m_Flag, true);
                EditorApplication.update -= Update;
            }
        }
    }
}

