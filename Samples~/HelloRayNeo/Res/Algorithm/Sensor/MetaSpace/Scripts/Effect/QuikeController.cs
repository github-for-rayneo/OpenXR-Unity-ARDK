using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuikeController : MonoBehaviour
{
    public GameObject screen;
    void OnGUI()
    {
       if (GUI.Button(new Rect(25,25,100,30),"Switch"))
       {
        //screen.setactive()
       } 
    }
}
