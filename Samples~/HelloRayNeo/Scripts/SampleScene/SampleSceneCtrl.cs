using FfalconXR;
using RayNeo;
using RayNeo.API;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SampleSceneCtrl : MonoBehaviour
{

    public GameObject m_PlaneDetection;
    private void Awake()
    {

        m_PlaneDetection.SetActive(RayNeoInfo.DeviceIsIntegratedType());
    }
    public void OnBtnClick(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void CloseApp()
    {
        Application.Quit();
    }

    public void OpenBatteryInfo()
    {
        PlatformAndroid.OpenSystemMonitoring();
    }
    public void CloseBatteryInfo()
    {
        PlatformAndroid.CloseSystemMonitoring();
    }
}
