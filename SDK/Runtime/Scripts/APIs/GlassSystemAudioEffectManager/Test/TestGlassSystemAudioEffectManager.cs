using RayNeo.API;
using UnityEngine;
using UnityEngine.UI;

public class TestGlassSystemAudioEffectManager : MonoBehaviour
{
    public Button Btn;

    // Start is called before the first frame update
    void Start()
    {
        Btn.onClick.AddListener(
            () => {
                GlassSystemAudioEffectManager.Instance.PlaySoundEffect(GlassSystemAudioEffectKey.rayneo_click);
            });

        GlassSystemAudioEffectManager.Instance.Init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
