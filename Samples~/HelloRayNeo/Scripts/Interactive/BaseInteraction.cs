using UnityEngine;
using UnityEngine.UI;

public class BaseInteraction : MonoBehaviour
{

    public Image m_BtnBg;//sample 1

    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void SwitchBtnBg()
    {
        m_BtnBg.color = new Color(Random.value, Random.value, Random.value, 1);
    }
}
