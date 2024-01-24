using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateCube : MonoBehaviour
{
    #region 数据定义
    [SerializeField]
    private GameObject[] CubePrefab;
    [SerializeField]
    private Transform  CubeContainer;
  
    [SerializeField]
    private Transform  CircleCenter;
    [SerializeField]
    private Transform  PointerParent;

    [SerializeField]
    private Material[] CircleMat;

    #endregion

    #region 生命周期
    private void Start()
    {
        //CreateCubeMatrix(4);
        GenerateAnnular(CubePrefab, 20);
    }

    #endregion

    #region 物体创建
    private void CreateCubeMatrix(int Wall)
    {
        for (int i = 0; i < Wall; i++)
        {
            for (int j = 0; j < Wall; j++)
            {
                for (int k = 0; k < Wall; k++)
                {
                    GameObject go = Instantiate(CubePrefab[0], CubeContainer);
                    go.SetActive(true);
                    go.transform.position = new Vector3(i, j, k);
                }
            }
        }
    }

    private void GenerateAnnular(GameObject[] CubePre, float Interval)
    {
        int Num = (int)(360.0f / Interval);
        for (int i = 0; i < CubePre.Length; i++)
        {
            for (int j = 0; j < Num; j++)
            {
                GameObject go = Instantiate(CubePre[i], PointerParent);
                go.transform.RotateAround(CircleCenter.position, Vector3.up, Interval * j);
                go.SetActive(true);

                go.GetComponent<Renderer>().material = CircleMat[i];
                go.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                go.isStatic = false;
            }
        }
    }

    #endregion

}
