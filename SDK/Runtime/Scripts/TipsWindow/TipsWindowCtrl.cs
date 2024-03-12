using UnityEngine;
using UnityEngine.UI;

public class TipsWindowCtrl : MonoBehaviour
{
    public Canvas m_Canvas;
    public Text m_TipsText;

    public void Init(string error)
    {
        m_Canvas.worldCamera = Camera.main;
        m_TipsText.text = error;
    }

}

