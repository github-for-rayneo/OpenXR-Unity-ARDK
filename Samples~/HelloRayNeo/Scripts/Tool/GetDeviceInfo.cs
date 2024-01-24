using FfalconXR;
using UnityEngine;

/// <summary>
/// ��ȡ�豸��Ϣ
/// </summary>
public class GetDeviceInfo : MonoBehaviour
{

    private void Start()
    {
        GetDeviceInformation();
    }

    private void GetDeviceInformation()
    {

        Log.Debug("[RayNeoX2]:��ȡ�豸����ϸ��Ϣ");
        Log.Debug("[RayNeoX2]:�豸ģ��:" + SystemInfo.deviceModel);
        Log.Debug("[RayNeoX2]:�豸����:" + SystemInfo.deviceName);
        Log.Debug("[RayNeoX2]:�豸����:" + SystemInfo.deviceType.ToString());
        Log.Debug("[RayNeoX2]:�豸Ψһ��ʶ��:" + SystemInfo.deviceUniqueIdentifier);
        Log.Debug("[RayNeoX2]:�Ƿ�֧��������:" + SystemInfo.copyTextureSupport.ToString());
        Log.Debug("[RayNeoX2]:�Կ�ID:" + SystemInfo.graphicsDeviceID.ToString());
        Log.Debug("[RayNeoX2]:�Կ�����:" + SystemInfo.graphicsDeviceName);
        Log.Debug("[RayNeoX2]:�Կ�����:" + SystemInfo.graphicsDeviceType.ToString());
        Log.Debug("[RayNeoX2]:�Կ���Ӧ��:" + SystemInfo.graphicsDeviceVendor);
        Log.Debug("[RayNeoX2]:�Կ���Ӧ��ID:" + SystemInfo.graphicsDeviceVendorID.ToString());
        Log.Debug("[RayNeoX2]:�Կ��汾��:" + SystemInfo.graphicsDeviceVersion);
        Log.Debug("[RayNeoX2]:�Դ��С����λ��MB��:" + SystemInfo.graphicsMemorySize);
        Log.Debug("[RayNeoX2]:�Կ��Ƿ�֧�ֶ��߳���Ⱦ:" + SystemInfo.graphicsMultiThreaded.ToString());
        Log.Debug("[RayNeoX2]:֧�ֵ���ȾĿ������:" + SystemInfo.supportedRenderTargetCount.ToString());
        Log.Debug("[RayNeoX2]:ϵͳ�ڴ��С(��λ��MB):" + SystemInfo.systemMemorySize.ToString());
        Log.Debug("[RayNeoX2]:����ϵͳ:" + SystemInfo.operatingSystem);
    }
}
