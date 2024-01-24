using RayNeo;
using RayNeo.API;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestSensorAlgorithm : MonoBehaviour
{

    #region ���ݶ���

    [SerializeField]
    private Transform CompassRoot;
    [SerializeField]
    private Text AzimuthTxt;

    #endregion

    #region ��������

    private void Update()
    {
        GetAzimuth();
    }

    #endregion

    #region ҵ���߼�


    /// <summary>
    /// ƫ���Ǳ仯
    /// </summary>
    private void GetAzimuth()
    {
        CompassRoot.localRotation = Quaternion.Euler(new Vector3(0, 0, Algorithm.GetAzimuth()));
    }

    #endregion
}
