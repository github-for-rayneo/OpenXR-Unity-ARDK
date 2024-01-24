using RayNeo;
using UnityEngine;
using UnityEngine.InputSystem;

public class ResetHeadTrack : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {

        SimpleTouch.Instance.OnDoubleTap.AddListener(OnReset);
        //m_action.Touch.SimpleTap.started += OnSimpleTapEvent;
        //m_action.Touch.SimpleTap.performed += OnSimpleTapEvent;
        //m_action.Touch.SimpleTap.canceled += OnSimpleTapEvent;
        //TouchEventCtrl.Instance.OnDoubleTap += OnReset;
    }

    private void OnDestroy()
    {
        if (SimpleTouch.SingletonExist)
        {
            SimpleTouch.Instance.OnDoubleTap.RemoveListener(OnReset);
        }


    }
    private void OnReset()
    {
        HeadTrackedPoseDriver.ResetQuaternion();
    }

    void Update()
    {

    }
}
