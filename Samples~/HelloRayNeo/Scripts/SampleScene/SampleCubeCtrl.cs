using RayNeo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SampleCubeCtrl : MonoBehaviour, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{


    private bool m_update = false;
    private bool m_rotDirection = false;

    private void Update()
    {
        if (!m_update)
        {
            transform.Rotate(Vector3.forward * (m_rotDirection ? 1 : -1)*0.1f, Space.World);

            return;
        }

        transform.Rotate(Vector3.forward * (m_rotDirection ? 1 : -1), Space.World);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        m_update = true;

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        m_update = false;

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //RingManager.OpenRing();
        ModifyRotation();
    }
    public void ModifyRotation()
    {
        m_rotDirection = !m_rotDirection;

    }
}