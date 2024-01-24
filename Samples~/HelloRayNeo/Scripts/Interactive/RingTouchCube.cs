using FfalconXR.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RingTouchCube : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    public Rigidbody m_RigiBody;

    private float m_DownDistance = 0;
    private Vector3 m_DownOffset = Vector3.zero;
    private bool m_PointerDown = false;

    public bool PointerDown { get => m_PointerDown; }
    public void OnPointerClick(PointerEventData eventData)
    {
    }

    private void Update()
    {
        if (!m_PointerDown)
        {
            return;
        }
        transform.position = m_DownDistance * RayManager.CurrentRay.Ray.direction + m_DownOffset;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        m_DownDistance = RayManager.CurrentRay.Distance;
        Vector3 downDirection = RayManager.CurrentRay.Ray.direction;
        m_DownOffset = transform.position - downDirection * m_DownDistance;
        m_PointerDown = true;
        m_RigiBody.isKinematic = true;
        Debug.LogError("OnPointerDown");


    }

    public void OnPointerUp(PointerEventData eventData)
    {
        m_RigiBody.isKinematic = false;
        m_PointerDown = false;
        Debug.LogError("OnPointerUp");
    }
}
