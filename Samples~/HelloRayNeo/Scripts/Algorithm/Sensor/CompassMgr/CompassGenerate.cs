using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompassGenerate : MonoBehaviour
{
    [SerializeField]
    private GameObject CompassPointer;
    [SerializeField]
    private Transform  CompassCenter;
    [SerializeField]
    private Transform  PointerParent;

    [SerializeField]
    private Material GrayMat;
    [SerializeField]
    private Material WhiteMat;
    [SerializeField]
    private Material RedMat;

    private void Start()
    {
        GenerateCompass(1);
    }


    private void GenerateCompass(float Interval)
    {
        int Num = (int)(360.0f / Interval);
        for (int i = 0; i < Num; i++)
        {
            GameObject go = Instantiate(CompassPointer, PointerParent);
            go.transform.RotateAround(CompassCenter.position, Vector3.up, Interval * i);
            go.SetActive(true);
            if (i == 0)
            {
                go.transform.localScale = new Vector3(0.04f, 0.04f, 1f);
                go.GetComponent<Renderer>().material = RedMat;
            }
            else if (i % 5 == 0)
            {
                go.GetComponent<Renderer>().material = WhiteMat;
                if (i % 45 == 0)
                {
                    go.transform.localScale = new Vector3(0.04f, 0.04f, 1f);
                }
                else
                {
                    go.transform.localScale = new Vector3(0.02f, 0.02f, 1f);
                }
            }
            else
            {
                go.GetComponent<Renderer>().material = GrayMat;
                go.transform.localScale = new Vector3(0.01f, 0.01f, 1f);
            }
            go.isStatic = false;
        }
    }

    private void OnDestroy()
    {
        CompassPointer = null;
        CompassCenter  = null;
        PointerParent  = null;

        GrayMat  = null;
        WhiteMat = null;
        RedMat   = null;
    }

}
