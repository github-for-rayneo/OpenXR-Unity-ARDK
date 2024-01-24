using UnityEngine;
using UnityEngine.UI;
using RayNeo;

public class TestRuntimeFacial : MonoBehaviour
{
    #region ���ݶ���
    //�沿���
    public Text Distance;
    #endregion

    #region ��������
    private void Update()
    {
        Vector3 pos = FaceDetectorManager.Ins.GetFacePosition(out bool suc);
        Debug.LogError("pos:"+ pos+" suc:"+suc);
        if (!suc)
        {
            //��ȡ������Ϣʧ��.
            return;
        }
        transform.position = Vector3.Lerp(transform.position, Camera.main.transform.TransformPoint(new Vector3(pos.x, pos.y, pos.z + 0.2f)), Time.deltaTime * 15);

        Distance.text = transform.position.z.ToString("f02") + "��";


        transform.LookAt(Camera.main.transform);

    }
    private void OnDestroy()
    {
        FaceDetectorManager.Ins.StopFaceDectector();
    }
    #endregion
}
