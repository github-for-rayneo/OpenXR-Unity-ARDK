//using RayNeo.Tool;
//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public class KillSelf : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //TouchEventCtrl.Instance.OnDoubleTap += Kill;
        //ProjectSettings

    }

    private void Kill()
    {
        Application.Quit();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        //TouchEventCtrl.Instance.OnDoubleTap -= Kill;

    }
}
