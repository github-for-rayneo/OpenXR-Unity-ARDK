using UnityEngine;
/// <summary>
/// ����canvas��Ĭ�ϲ�����Image�ϵ�Ҳ��һ������ʹui��Ⱦ��ģ�͵�ǰ�档
/// </summary>
public class SetCanvasOverlay : MonoBehaviour
{

    public Shader Overlay;
    private void Awake()
    {
        UnityEngine.UI.Graphic.defaultGraphicMaterial.shader = Overlay;
        // UnityEngine.UI.Graphic.defaultGraphicMaterial.shader = Shader.Find("UI/Overlay");//����shaderû�����ã�����shader���ᱻ������ڣ�����Find������
    }
}