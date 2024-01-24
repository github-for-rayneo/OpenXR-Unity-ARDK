using FfalconXR.InputModule;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR;

public class GazeLaserBeam : LaserBeam
{

    /// <summary>
    /// 被hover的obj保存一下.
    /// </summary>
    private GameObject m_HoverObj;

    private IPointerClickHandler m_PointerHandler;
    private IDragHandler m_DragHandler;

    public Material m_progressMat;

    /// <summary>
    /// 进度条的renderer
    /// </summary>
    private MeshRenderer m_ProgressRenderer;

    /// <summary>
    /// 进度条内部和圆圈外部的距离
    /// </summary>
    [SerializeField]
    private float m_ProgressInnerDiameterOffset = 1;

    /// <summary>
    /// 进度条外部和圆圈外部的距离
    /// </summary>
    [SerializeField]
    private float m_ProgressOuterDiameterOffset = 4;
    /// <summary>
    /// 进度条速度
    /// </summary>
    [SerializeField]
    private int m_ProgressSpeed = 10;
    private int m_MaxProgressValue = 360;

    /// <summary>
    /// 当前进度条值
    /// </summary>
    private int m_CurProgressValue;
    /// <summary>
    /// 进度条跑完了,是否已经发射了.
    /// </summary>
    private bool m_ProgressClickActionShooted = false;

    private RaycastResult m_raycastResult;

    PointerEventData m_PointerEventData;


    protected new void Start()
    {

        base.Start();
        if (m_LaserBeamDot == null)
        {
            return;
        }
        m_PointerEventData = new PointerEventData(EventSystem.current);
        m_PointerEventData.button = PointerEventData.InputButton.Left;
        GameObject progressGo = new GameObject("ProgressDot");
        progressGo.transform.SetParent(m_LaserBeamDot.parent);
        progressGo.transform.localPosition = Vector3.zero;
        progressGo.transform.localScale = Vector3.one;
        progressGo.transform.localRotation = Quaternion.Euler(0, 0, 0);
        m_ProgressRenderer = progressGo.AddComponent<MeshRenderer>();
        m_ProgressRenderer.material = m_progressMat;
        MeshFilter mf = progressGo.AddComponent<MeshFilter>();
        m_ProgressRenderer.sortingOrder = m_SortingOrder;
        CreateReticleVertices(mf);
    }
    private new void Update()
    {
        base.Update();
        UpdateProgress(m_OuterDiameter * m_Dis, m_Dis);

    }

    public override void OnPointerEnter(RaycastResult raycastResult, GameObject interactiveObj)
    {
        m_HoverObj = interactiveObj;
        //通知点击.
        m_PointerHandler = m_HoverObj.GetComponent<IPointerClickHandler>();
        m_DragHandler = m_HoverObj.GetComponent<IDragHandler>();
        m_raycastResult = raycastResult;

        //m_RunProgress = true;
        m_ProgressClickActionShooted = false;
        m_CurProgressValue = 0;
        m_ProgressRenderer.material.SetFloat("_Progress", m_CurProgressValue);
        base.OnPointerEnter(raycastResult, interactiveObj);
    }

    public override void OnPointerExit(GameObject previousObject)
    {
        m_HoverObj = null;
        m_DragHandler = null;
        m_PointerHandler = null;
        base.OnPointerExit(previousObject);
    }

    public void UpdateProgress(float outerDiameter, float distance)
    {

        if (m_ProgressRenderer == null)
        {
            return;
        }

        //if (!m_openProgress || !m_RunProgress)
        if (m_HoverObj == null)
        {
            m_ProgressRenderer.gameObject.SetActive(false);
            return;
        }

        m_ProgressRenderer.gameObject.SetActive(!m_ProgressClickActionShooted);
        if (m_ProgressRenderer == null)
        {
            return;
        }
        m_ProgressRenderer.material.SetFloat("_InnerDiameter", outerDiameter + m_ProgressInnerDiameterOffset);
        m_ProgressRenderer.material.SetFloat("_OuterDiameter", outerDiameter + m_ProgressOuterDiameterOffset);
        m_ProgressRenderer.material.SetFloat("_DistanceInMeters", distance);
        Vector3 dotScreenPoint = Camera.main.WorldToScreenPoint(m_LaserBeamDot.transform.position + m_LaserBeamDot.forward * distance);
        if (m_CurProgressValue > m_MaxProgressValue)
        {

            m_PointerEventData.position = dotScreenPoint;
            //#if UNITY_EDITOR
            //            new Vector2(Screen.width / 2, Screen.height / 2);
            //#else
            //            new Vector2(XRSettings.eyeTextureWidth / 2, XRSettings.eyeTextureHeight / 2);
            //#endif

            if (m_PointerHandler != null && !m_ProgressClickActionShooted)
            {
                m_PointerHandler.OnPointerClick(m_PointerEventData);
            }
            if (m_DragHandler != null)
            {
                m_PointerEventData.pointerPressRaycast = m_raycastResult;

                m_DragHandler.OnDrag(m_PointerEventData);
            }
            m_ProgressClickActionShooted = true;
        }
        else
        {
            m_ProgressRenderer.material.SetFloat("_Progress", m_CurProgressValue += m_ProgressSpeed);
            m_ProgressRenderer.material.SetVector("_Center", new Vector3(dotScreenPoint.x, Screen.height - dotScreenPoint.y, 0));

        }
    }
}
