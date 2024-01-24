using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
namespace RayNeo
{

    public class EventTriggerListener : UIBehaviour, IMoveHandler, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler, IDragHandler, IScrollHandler, IEventSystemHandler
    {
        #region 事件定义
        public Action<GameObject, Vector2> OnPointMove;
        public Action<GameObject, Vector2> OnPointDrag;
        public Action<GameObject, Vector2> OnPointScroll;


        public Action<GameObject> OnPointDown;
        public Action<GameObject> OnPointtUp;

        public Action<GameObject> OnPointClick;
        public Action<GameObject> OnPointEnter;
        public Action<GameObject> OnPointExit;
        public Action<GameObject> OnPointSelect;
        public Action<GameObject> OnPointDeselect;

        #endregion

        #region 事件触发
        public static EventTriggerListener Get(GameObject go)
        {
            EventTriggerListener Listener = go.GetComponent<EventTriggerListener>();
            if (Listener == null)
            {
                Listener = go.AddComponent<EventTriggerListener>();
            }
            return Listener;
        }

        public void OnMove(AxisEventData EventData)
        {
            if (OnPointMove != null)
            {
                OnPointMove(gameObject, EventData.moveVector);
            }
        }

        public void OnDrag(PointerEventData EventData)
        {
            if (OnPointDrag != null)
            {
                OnPointDrag(gameObject, EventData.delta);
            }
        }

        public void OnScroll(PointerEventData EventData)
        {
            if (OnPointScroll != null)
            {
                OnPointScroll(gameObject, EventData.scrollDelta);
            }
        }

        public void OnPointerDown(PointerEventData EventData)
        {
            if (OnPointDown != null)
            {
                OnPointDown(gameObject);
            }
        }

        public void OnPointerUp(PointerEventData EventData)
        {
            if (OnPointtUp != null)
            {
                OnPointtUp(gameObject);
            }
        }

        public void OnPointerClick(PointerEventData EventData)
        {
            if (OnPointClick != null)
            {
                OnPointClick(gameObject);
            }
        }

        public void OnPointerEnter(PointerEventData EventData)
        {
            if (OnPointEnter != null)
            {
                OnPointEnter(gameObject);
            }
        }

        public void OnPointerExit(PointerEventData EventData)
        {
            if (OnPointExit != null)
            {
                OnPointExit(gameObject);
            }
        }

        public void OnSelect(BaseEventData EventData)
        {
            if (OnPointSelect != null)
            {
                OnPointSelect(gameObject);
            }
        }

        public void OnDeselect(BaseEventData EventData)
        {
            if (OnPointDeselect != null)
            {
                OnPointDeselect(gameObject);
            }
        }

        #endregion
    }
}
