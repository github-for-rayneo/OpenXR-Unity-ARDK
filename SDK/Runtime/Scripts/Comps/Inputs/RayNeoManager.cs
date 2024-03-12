using RayNeo.API;
using System;
using System.Collections.Generic;
using UnityEngine;
using static RayNeo.API.PlatformAndroid;

namespace RayNeo
{
    public class RayNeoManager : MonoBehaviour
    {

        public List<WindowsTipsInfo> m_LoadLibErrorTips;
        public GameObject m_ErrorPrefab;
        public void Awake()
        {
            //LibLoadResult result = PlatformAndroid.GetLibLoadResult();
            //if (result != LibLoadResult.Suc)
            //{

            //    for (int i = 0; i < m_LoadLibErrorTips.Count; i++)
            //    {
            //        if (m_LoadLibErrorTips[i].type == result)
            //        {
            //            TipsWindowCtrl go = Instantiate(m_ErrorPrefab).GetComponent<TipsWindowCtrl>();
            //            go.Init(m_LoadLibErrorTips[i].text);
            //            return;
            //        }
            //    }
            //}
        }
    }

    [Serializable]
    public class WindowsTipsInfo
    {
        public LibLoadResult type;
        public string text;
        public Color color;

    }
}
