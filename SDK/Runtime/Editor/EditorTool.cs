using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;


/// <summary>
/// ����Editor���ù�������
/// </summary>
public class EditorTool
{
    static string[] AssetGUIDs;
    static string[] AssetPaths;
    static string[] AllAssetPaths;

    [MenuItem("Assets/��Դ���ò���", false, 25)]
    static void FindreAssetFerencesMenu()
    {
        Debug.LogError("��ʼ������Դ����");

        if (Selection.assetGUIDs.Length == 0)
        {
            Debug.LogError("����Project�ļ�����ѡ����Ҫ���ҵ���Դ�ļ����Ҽ���ѡ����Դ���ò��ҡ�");
            return;
        }

        AssetGUIDs = Selection.assetGUIDs;
        AssetPaths = new string[AssetGUIDs.Length];

        for (int i = 0; i < AssetGUIDs.Length; i++)
        {
            AssetPaths[i] = AssetDatabase.GUIDToAssetPath(AssetGUIDs[0]);
            Debug.LogError("[MercuryX2]:AssetPath|" + AssetPaths[i]);
        }

        AllAssetPaths = AssetDatabase.GetAllAssetPaths();
        FindreAssetReferences();
    }

    static void FindreAssetReferences()
    {
        List<string> logInfo = new List<string>();
        string path;
        string log;
        for (int i = 0; i < AllAssetPaths.Length; i++)
        {
            path = AllAssetPaths[i];

            if (path.EndsWith(".prefab") || path.EndsWith(".unity"))
            {
                string content = File.ReadAllText(path);
                if (content == null)
                {
                    continue;
                }
                for (int j = 0; j < AssetGUIDs.Length; j++)
                {
                    if (content.IndexOf(AssetGUIDs[j]) > 0)
                    {
                        log = string.Format("{0}����{1}", path, AssetPaths[j]);
                        logInfo.Add(log);
                    }
                }
            }
        }

        for (int i = 0; i < logInfo.Count; i++)
        {
            Debug.LogError(logInfo[i]);
        }
        Debug.LogError("[MercuryX2]:ѡ�������������:" + logInfo.Count);
    }

    public static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
    {
        // Get the subdirectories for the specified directory.
        DirectoryInfo dir = new DirectoryInfo(sourceDirName);

        if (!dir.Exists)
        {
            throw new DirectoryNotFoundException(
                "Source directory does not exist or could not be found:"
                + sourceDirName);
        }

        DirectoryInfo[] dirs = dir.GetDirectories();

        // If the destination directory doesn't exist, create it.       
        Directory.CreateDirectory(destDirName);

        // Get the files in the directory and copy them to the new location.
        FileInfo[] files = dir.GetFiles();
        foreach (FileInfo file in files)
        {
            string tempPath = Path.Combine(destDirName, file.Name);
            file.CopyTo(tempPath, true);
        }

        // If copying subdirectories, copy them and their contents to new location.
        if (copySubDirs)
        {
            foreach (DirectoryInfo subdir in dirs)
            {
                string tempPath = Path.Combine(destDirName, subdir.Name);
                DirectoryCopy(subdir.FullName, tempPath, copySubDirs);
            }
        }
    }
}