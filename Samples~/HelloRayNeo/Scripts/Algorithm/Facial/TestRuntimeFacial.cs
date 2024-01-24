using UnityEngine;
using UnityEngine.UI;
using RayNeo;

public class TestRuntimeFacial : MonoBehaviour
{
    #region 数据定义
    //面部外框
    public Text Distance;
    #endregion

    #region 生命周期
    private void Update()
    {
        Vector3 pos = FaceDetectorManager.Ins.GetFacePosition(out bool suc);
        Debug.LogError("pos:"+ pos+" suc:"+suc);
        if (!suc)
        {
            //获取脸部信息失败.
            return;
        }
        transform.position = Vector3.Lerp(transform.position, Camera.main.transform.TransformPoint(new Vector3(pos.x, pos.y, pos.z + 0.2f)), Time.deltaTime * 15);

        Distance.text = transform.position.z.ToString("f02") + "米";


        transform.LookAt(Camera.main.transform);

    }
    private void OnDestroy()
    {
        FaceDetectorManager.Ins.StopFaceDectector();
    }
    #endregion
}
