using FfalconXR;
using RayNeo.API;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace RayNeo
{

    public class SimpleTouch : MonoSingleton<SimpleTouch>
    {


        RayNeoInput m_Action;

        public bool m_CustomConfig = false;

        public TouchConfig m_TouchConfig;

        [Serializable]
        public class OnTapEvent : UnityEvent { }
        [Serializable]
        public class OnPosEvent : UnityEvent<Vector2> { }


        [Serializable]
        public class OnTouchMoveEvent : UnityEvent<SwipeActionType, Vector2> { }


        [SerializeField]
        private OnTapEvent m_OnSimleTap = new OnTapEvent();
        public OnTapEvent OnSimpleTap { get => m_OnSimleTap; }


        [SerializeField]
        private OnTapEvent m_OnDoubleTap = new OnTapEvent();
        public OnTapEvent OnDoubleTap { get => m_OnDoubleTap; }


        [SerializeField]
        private OnTapEvent m_OnTripleTap = new OnTapEvent();
        public OnTapEvent OnTripleTap { get => m_OnTripleTap; }


        [SerializeField]
        private OnPosEvent m_OnTouchStart = new OnPosEvent();
        public OnPosEvent OnTouchStart { get => m_OnTouchStart; }

        [SerializeField]
        private OnTouchMoveEvent m_OnTouchMove = new OnTouchMoveEvent();
        public OnTouchMoveEvent OnTouchMove { get => m_OnTouchMove; }

        [SerializeField]
        private OnPosEvent m_OnTouchUp = new OnPosEvent();
        public OnPosEvent OnTouchUp { get => m_OnTouchUp; }

        [SerializeField]
        private OnPosEvent m_OnSwipeLeftEnd = new OnPosEvent();
        public OnPosEvent OnSwipeLeftEnd { get => m_OnSwipeLeftEnd; }

        [SerializeField]
        private OnPosEvent m_OnSwipeRightEnd = new OnPosEvent();
        public OnPosEvent OnSwipeRightEnd { get => m_OnSwipeRightEnd; }

        private long m_LastTapPerformedTime = 0;
        private int m_PerformedTapCounts = 0;
        private bool m_BeginClickCheck = false;

        private bool m_FingerDownState = false;
        private Vector2 m_FingerDownPosition = Vector2.zero;
        private Vector2 m_FingerMovePosition = Vector2.zero;
        private bool m_InSwipe = false;
        //如果处于滑动期间,第一次设置手势时的坐标点.
        private Vector2 m_OnSwipeStartPos = Vector2.zero;

        private const int m_SwipeSwitchThreahold = 20;


        private SwipeActionType m_swipeType = SwipeActionType.NONE;

        private void OnEnable()
        {
            if (m_CustomConfig)
            {

            }
            else
            {
                m_TouchConfig = RayNeoXRGeneralSettings.Instance.SimpleTouchConfig;
            }
            m_Action = new RayNeoInput();
            m_Action.Enable();
            //只监听单击.
            m_Action.SimpleTouch.Tap.performed += TapPerformed;
            m_Action.SimpleTouch.Tap.started += TapStarted;

            //只监听位置
            m_Action.SimpleTouch.Position.performed += PositionPerformed;

            //只监听按下与抬起
            m_Action.SimpleTouch.Press.performed += PressPerformed;
        }
        private void PressPerformed(InputAction.CallbackContext context)
        {
            int count = (int)context.ReadValue<float>();
            m_FingerDownState = count > 0;
            var pos = m_Action.SimpleTouch.Position.ReadValue<Vector2>();
            m_FingerMovePosition = pos;
            if (m_FingerDownState)
            {
                OnTouchStart?.Invoke(pos);
            }
            else
            {

                if (m_InSwipe && Vector2.Distance(m_OnSwipeStartPos, pos) > m_TouchConfig.SwipeEndThreshold)
                {
                    //是滑动.并且滑动数值大于设定阈值. 才执行end
                    //m_FingerDownPosition
                    if (m_swipeType == SwipeActionType.LEFT)
                    {
                        OnSwipeLeftEnd?.Invoke(pos);
                    }
                    else if (m_swipeType == SwipeActionType.RIGHT)
                    {
                        OnSwipeRightEnd?.Invoke(pos);

                    }
                }

                m_swipeType = SwipeActionType.NONE;
                OnTouchUp?.Invoke(pos);
                m_InSwipe = false;
            }
        }
        private void PositionPerformed(InputAction.CallbackContext context)
        {
            if (!m_FingerDownState)
            {
                return;
            }
            var pos = context.ReadValue<Vector2>();
            if (m_InSwipe)
            {
                SwipeDirectionCheck(pos);
            }
            else if (Vector2.Distance(pos, m_FingerDownPosition) > m_TouchConfig.MovingThreshold)
            {
                //超阈值了.
                Change2SwipeState();
                SwipeDirectionCheck(pos);
            }
            OnTouchMove?.Invoke(m_swipeType, pos);
            m_FingerMovePosition = pos;
        }

        private void Change2SwipeState()
        {
            m_InSwipe = true;
            if (m_PerformedTapCounts == 1 && m_TouchConfig.ReleaseSimpleTapImmediately)
            {
                return;//不执行操作.
            }
            if (!m_TouchConfig.ReleaseLastTapOnChangeToMove)
            {
                return;//不执行操作.
            }

            FireTap();
        }

        private Vector2 m_LastSwipeDirectionPos;
        private void SwipeDirectionCheck(Vector2 pos)
        {

            if (pos.x > m_FingerMovePosition.x)
            {
                if (m_swipeType == SwipeActionType.LEFT)
                {
                    if (Mathf.Abs(m_LastSwipeDirectionPos.x - pos.x) < m_SwipeSwitchThreahold)
                    {
                        return;//lock
                    }
                }

                if (m_swipeType != SwipeActionType.RIGHT)
                {
                    m_OnSwipeStartPos = pos;
                }
                m_swipeType = SwipeActionType.RIGHT;
                m_LastSwipeDirectionPos = pos;


            }
            else
            {
                if (m_swipeType == SwipeActionType.RIGHT)
                {
                    if (Mathf.Abs(m_LastSwipeDirectionPos.x - pos.x) < m_SwipeSwitchThreahold)
                    {
                        return;//lock
                    }
                }
                if (m_swipeType != SwipeActionType.LEFT)
                {
                    m_OnSwipeStartPos = pos;
                }
                m_swipeType = SwipeActionType.LEFT;
                m_LastSwipeDirectionPos = pos;
            }
        }
        private void TapStarted(InputAction.CallbackContext context)
        {
            m_BeginClickCheck = false;
            m_LastSwipeDirectionPos = m_OnSwipeStartPos = m_FingerDownPosition = m_Action.SimpleTouch.Position.ReadValue<Vector2>();

        }
        private void TapPerformed(InputAction.CallbackContext context)
        {
            if (m_InSwipe)
            {
                return;//不点击了.
            }
            long curTime = CommonUtils.CurrentTimeMilliseconds;
            m_LastTapPerformedTime = curTime;
            m_PerformedTapCounts++;

            m_BeginClickCheck = true;
            if (m_TouchConfig.ReleaseSimpleTapImmediately)
            {
                OnSimpleTap?.Invoke();
            }
        }


        private void OnDisable()
        {
            m_Action.Disable();
        }

        private void Update()
        {
            if (!m_BeginClickCheck)
            {
                return;
            }

            long curTime = CommonUtils.CurrentTimeMilliseconds;
            if (curTime - m_LastTapPerformedTime >= (m_TouchConfig.MulitTapSpacing * 1000))
            {
                FireTap();
            }
        }

        private void FireTap()
        {
            if (m_PerformedTapCounts == 0)
            {

            }
            else if (m_PerformedTapCounts == 1)
            {
                if (!m_TouchConfig.ReleaseSimpleTapImmediately)
                {
                    OnSimpleTap?.Invoke();
                }
            }
            else if (m_PerformedTapCounts == 2)
            {
                OnDoubleTap?.Invoke();
            }
            else if (m_PerformedTapCounts == 3)
            {
                OnTripleTap?.Invoke();
            }
            else
            {
                OnSimpleTap?.Invoke();

            }
            m_BeginClickCheck = false;
            m_PerformedTapCounts = 0;//执行点击. 并清零.
        }
    }

    public enum SwipeActionType
    {
        NONE,
        LEFT,
        RIGHT,
    }

}