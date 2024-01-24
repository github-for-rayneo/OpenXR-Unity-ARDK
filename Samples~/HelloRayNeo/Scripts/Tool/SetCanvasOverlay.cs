using UnityEngine;
/// <summary>
/// 设置canvas的默认材质球（Image上的也是一样），使ui渲染在模型的前面。
/// </summary>
public class SetCanvasOverlay : MonoBehaviour
{

    public Shader Overlay;
    private void Awake()
    {
        UnityEngine.UI.Graphic.defaultGraphicMaterial.shader = Overlay;
        // UnityEngine.UI.Graphic.defaultGraphicMaterial.shader = Shader.Find("UI/Overlay");//可能shader没有引用，导致shader不会被打入包内，导致Find不到。
    }
}