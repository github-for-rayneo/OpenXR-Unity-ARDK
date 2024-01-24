using RayNeo;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using com.rayneo.xr.extensions;
using RayNeo.API;
using FfalconXR;

public class TestPlaneDetection : MonoBehaviour
{

    XRPlaneInfo[] m_infoArrays = new XRPlaneInfo[3];

    private List<GameObject> m_planeObjs = new List<GameObject>();

    public Text m_tips;

    public Material m_m;

    private void Awake()
    {
#if UNITY_EDITOR
        m_tips.text = "请在眼镜端执行.";
#else
        Algorithm.EnableSlamHeadTracker();
        Algorithm.EnablePlaneDetection();
#endif
        for (int i = 0; i < m_infoArrays.Length; i++)
        {
            var go = new GameObject("Plane" + i);
            m_planeObjs.Add(go);
        }
    }



    private void Update()
    {

#if UNITY_EDITOR
        //以下是编辑器中的测试代码
        int res = 1;
        m_infoArrays[0].local_polygon = new float[] { 0.2268041f, -0.04797018f, 0.1254312f, 0.1474568f, 0.06891724f, 0.1936747f, -0.0574673f, 0.1814282f, -0.08233707f, 0.1769734f, -0.1225531f, 0.1689448f, -0.1617062f, 0.1453579f, -0.2268041f, -0.01964405f, -0.1995946f, -0.1936747f, 0.1393107f, -0.09651327f };
        m_infoArrays[0].local_polygon_size = 8;
        m_infoArrays[0].pose.position.x = -0.18f;
        m_infoArrays[0].pose.position.y = -0.30f;
        m_infoArrays[0].pose.position.z = 0.36f;
        var q = new Quaternion(-0.02839f, -0.70654f, 0.70654f, -0.02839f);
        m_infoArrays[0].pose.rotation.x = q.x;
        m_infoArrays[0].pose.rotation.y = q.y;
        m_infoArrays[0].pose.rotation.z = q.z;
        m_infoArrays[0].pose.rotation.w = q.w;

#else
        int res = Algorithm.GetPlaneInfo(m_infoArrays);
 
#endif
        Log.Debug("TestPlaneDetection获取平面信息---z序列:" + res + ":" + m_infoArrays.Length);
        for (int i = 0; i < m_infoArrays.Length; i++)
        {
            if (i < res)
            {
                m_planeObjs[i].SetActive(true);
                XRPlaneInfo info = m_infoArrays[i];
                Log.Debug("TestPlaneDetection 开始创建模型:" + info.local_polygon.Length + ":" + info.local_polygon_size);

                GameObject obj = Algorithm.CreatePlaneMesh(info, m_planeObjs[i], true, m_m);
                obj.transform.localPosition = Algorithm.ConvertPlanePosition(info);
                obj.transform.localRotation = Algorithm.ConvertPlaneRotation(info);
                var mesh = obj.GetComponent<MeshFilter>().mesh;

                Log.Debug("TestPlaneDetection 模型创建完毕:" + obj.transform.localPosition + ":" + obj.transform.localRotation + ":" + obj.transform.localRotation.eulerAngles);

            }
            else
            {
                m_planeObjs[i].SetActive(false);
            }
        }

    }

    private void OnDestroy()
    {
#if UNITY_EDITOR

#else
        Algorithm.DisablePlaneDetection();
        Algorithm.DisableSlamHeadTracker();

#endif

    }


}
