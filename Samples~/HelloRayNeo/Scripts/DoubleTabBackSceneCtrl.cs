using RayNeo;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class DoubleTabBackSceneCtrl : MonoBehaviour
{

    public void OnDoubleTapCallBack()
    {
        SceneManager.LoadScene("Entry");
    }

    void Start()
    {
        SimpleTouch.Instance.OnDoubleTap.AddListener(OnDoubleTapCallBack);
    }

    private void OnDestroy()
    {
        if (SimpleTouch.SingletonExist)
        {
            SimpleTouch.Instance.OnDoubleTap.RemoveListener(OnDoubleTapCallBack);
        }

    }
}
