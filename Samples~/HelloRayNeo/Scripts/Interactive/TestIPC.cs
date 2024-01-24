using RayNeo;
using RayNeo.API;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TestIPC : MonoBehaviour
{

    public RayTrackedPoseDriver m_rayDriver;

    private long m_clickTime = 0;
    private long m_doubleClickTime = 500;// 500∫¡√Î

    public Text m_gpsInfo;
    private PhoneGPSResultType m_state = PhoneGPSResultType.UNKNOW;
    private string m_gpsMsg;

    public RingTouchCube m_RingCube;
    private Quaternion m_CubeDownRotate;
    private Vector3 m_CubeDownRingPos;

    public RayNeoInput m_Input;

    void Start()
    {
        RingManager.OpenRing(RingTouchType.CustomTouchEvent, false);
        IPC.OpenPhoneGPS();
        IPC.GpsStateChagneCallBack += GPSStateChange;
        IPC.GPSPushCallBack += GPSMsgPush;
        m_Input = new RayNeoInput();
        m_Input.Enable();
        //m_RingHeavyClick.Enable();
        //m_RingPadMove.Enable();
    }


    private void GPSStateChange(PhoneGPSResultType code, string msg)
    {
        m_state = code;
    }

    public void GPSMsgPush(long time, double latitude, double longitude, double altitude, double speed,
                           double horizontalAccuracyMeters, double AccuracyMeters)
    {
        m_gpsMsg = time + "," + latitude + "," + longitude + "," + altitude + "," + speed;
    }
    void Update()
    {
        if (m_state == PhoneGPSResultType.UNKNOW)
        {
            m_gpsInfo.text = "";
        }
        else
        {
            m_gpsInfo.text = "gps state:" + m_state + "\n" + m_gpsMsg;
        }

        if (m_RingCube.PointerDown)
        {
            if (m_Input.Ring.TouchPadClick.WasPressedThisFrame())
            {
                m_CubeDownRingPos = m_Input.Ring.Position.ReadValue<Vector3>();
                m_CubeDownRotate = m_RingCube.transform.rotation;
            }
            else
            {
                var curRingTouchPosDelta = m_Input.Ring.Position.ReadValue<Vector3>() - m_CubeDownRingPos;
                var cubeRot = m_CubeDownRotate.eulerAngles;
                cubeRot += new Vector3(curRingTouchPosDelta.y, curRingTouchPosDelta.x, 0);
                m_RingCube.transform.localRotation = Quaternion.Euler(cubeRot);
            }


            if (m_Input.Ring.HeavyClick.IsPressed())
            {
                m_RingCube.transform.localScale = Vector3.Lerp(m_RingCube.transform.localScale, new Vector3(2, 2, 2), Time.deltaTime * 10);
            }
            else
            {
                m_RingCube.transform.localScale = Vector3.Lerp(m_RingCube.transform.localScale, Vector3.one, Time.deltaTime * 10);
            }
        }

    }

    private void OnDestroy()
    {
        IPC.GpsStateChagneCallBack -= GPSStateChange;
        IPC.GPSPushCallBack -= GPSMsgPush;

        //m_dev.Disable();
        RingManager.CloseRing();
        m_rayDriver.SetLineVisibleState(false);

    }
}
