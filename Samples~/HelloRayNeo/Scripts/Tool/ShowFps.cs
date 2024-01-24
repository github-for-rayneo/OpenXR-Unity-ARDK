using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ShowFps : MonoBehaviour
{
    /// 每次刷新计算的时间
    public  float  UpdateInterval = 0.5f;
    /// 上一次计算间隔结束时间
    private double LastInterval;
    private int    Frames;
    private float  CurrentFps;

    private Text FpsTxt;
    private void Start()
    {
        FpsTxt = GetComponent<Text>();
        if (FpsTxt == null)
        {
            FpsTxt = gameObject.AddComponent<Text>();
        }
        LastInterval = Time.realtimeSinceStartup;
        Frames = 0;
    }

    private void Update()
    {
        ++Frames;
        float TimeNow = Time.realtimeSinceStartup;
        if (TimeNow >= LastInterval + UpdateInterval)
        {
            CurrentFps = (float)(Frames / (TimeNow - LastInterval));
            Frames = 0;
            LastInterval = TimeNow;
        }

        float Max = Application.targetFrameRate / 2;
        if (CurrentFps >= Max)
        {
            FpsTxt.color = Color.green;
        }
        else if (CurrentFps < Max && CurrentFps >= 5)
        {
            FpsTxt.color = Color.yellow;
        }
        else
        {
            FpsTxt.color = Color.red;
        }
        string fps = "Fps:" + CurrentFps.ToString("f2");
        FpsTxt.text = fps;
    }
}