using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadomPlacment : MonoBehaviour
{
    public GameObject[] prefabs;
    public int count = 10;
    public float radius = 3f;

    void Start()
    {
        for (int i = 0; i < count; i++)
        {
            //Random.insideUniteCircle ---Find a random place in a certain radius circle
            Vector2 unitCircle = Random.insideUnitCircle * radius;
            Vector3 pos= new Vector3(unitCircle.x,0,unitCircle.y);

            Quaternion rotation = Quaternion.Euler(0, Random.Range(0, 360f), 0f);
            GameObject obj= prefabs[Random.Range(0, prefabs.Length)];
            Instantiate(obj,pos,rotation,transform);
        }
    }
}
