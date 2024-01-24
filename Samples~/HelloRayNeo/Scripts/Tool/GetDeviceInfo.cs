using FfalconXR;
using UnityEngine;

/// <summary>
/// 获取设备信息
/// </summary>
public class GetDeviceInfo : MonoBehaviour
{

    private void Start()
    {
        GetDeviceInformation();
    }

    private void GetDeviceInformation()
    {

        Log.Debug("[RayNeoX2]:读取设备的详细信息");
        Log.Debug("[RayNeoX2]:设备模型:" + SystemInfo.deviceModel);
        Log.Debug("[RayNeoX2]:设备名称:" + SystemInfo.deviceName);
        Log.Debug("[RayNeoX2]:设备类型:" + SystemInfo.deviceType.ToString());
        Log.Debug("[RayNeoX2]:设备唯一标识符:" + SystemInfo.deviceUniqueIdentifier);
        Log.Debug("[RayNeoX2]:是否支持纹理复制:" + SystemInfo.copyTextureSupport.ToString());
        Log.Debug("[RayNeoX2]:显卡ID:" + SystemInfo.graphicsDeviceID.ToString());
        Log.Debug("[RayNeoX2]:显卡名称:" + SystemInfo.graphicsDeviceName);
        Log.Debug("[RayNeoX2]:显卡类型:" + SystemInfo.graphicsDeviceType.ToString());
        Log.Debug("[RayNeoX2]:显卡供应商:" + SystemInfo.graphicsDeviceVendor);
        Log.Debug("[RayNeoX2]:显卡供应商ID:" + SystemInfo.graphicsDeviceVendorID.ToString());
        Log.Debug("[RayNeoX2]:显卡版本号:" + SystemInfo.graphicsDeviceVersion);
        Log.Debug("[RayNeoX2]:显存大小（单位：MB）:" + SystemInfo.graphicsMemorySize);
        Log.Debug("[RayNeoX2]:显卡是否支持多线程渲染:" + SystemInfo.graphicsMultiThreaded.ToString());
        Log.Debug("[RayNeoX2]:支持的渲染目标数量:" + SystemInfo.supportedRenderTargetCount.ToString());
        Log.Debug("[RayNeoX2]:系统内存大小(单位：MB):" + SystemInfo.systemMemorySize.ToString());
        Log.Debug("[RayNeoX2]:操作系统:" + SystemInfo.operatingSystem);
    }
}
