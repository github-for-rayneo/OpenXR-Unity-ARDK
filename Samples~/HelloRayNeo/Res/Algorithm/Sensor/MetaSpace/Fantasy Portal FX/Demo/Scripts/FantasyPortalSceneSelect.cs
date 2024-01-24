using UnityEngine;
using UnityEngine.SceneManagement;

namespace PortalFX
{

public class FantasyPortalSceneSelect : MonoBehaviour
{
	public bool GUIHide = false;
	public bool GUIHide2 = false;
	public bool GUIHide3 = false;
	
    public void LoadSceneDemo1()
    {
        SceneManager.LoadScene("MagicPortal01");
    }
    public void LoadSceneDemo2()
    {
        SceneManager.LoadScene("MagicPortal02");
    }
    public void LoadSceneDemo3()
    {
        SceneManager.LoadScene("MagicPortal03");
    }
    public void LoadSceneDemo4()
    {
        SceneManager.LoadScene("MagicPortal04");
    }
    public void LoadSceneDemo5()
    {
        SceneManager.LoadScene("MagicPortal05");
    }
    public void LoadSceneDemo6()
    {
        SceneManager.LoadScene("MagicPortal06");
    }
    public void LoadSceneDemo7()
    {
        SceneManager.LoadScene("MagicPortal07");
    }
    public void LoadSceneDemo8()
    {
        SceneManager.LoadScene("MagicPortal08");
    }
    public void LoadSceneDemo9()
    {
        SceneManager.LoadScene("MagicPortal09");
    }
    public void LoadSceneDemo10()
    {
        SceneManager.LoadScene("MagicPortal10");
    }
	public void LoadSceneDemo11()
    {
        SceneManager.LoadScene("MagicPortal11");
    }
	public void LoadSceneDemo12()
    {
        SceneManager.LoadScene("MagicPortal12");
    }
	public void LoadSceneDemo13()
    {
        SceneManager.LoadScene("MagicPortal13");
    }
	public void LoadSceneDemo14()
    {
        SceneManager.LoadScene("MagicPortal14");
    }
	public void LoadSceneDemo15()
    {
        SceneManager.LoadScene("MagicPortal15");
    }
	public void LoadSceneDemo16()
    {
        SceneManager.LoadScene("MagicPortal16");
    }
	public void LoadSceneDemo17()
    {
        SceneManager.LoadScene("MagicPortal17");
    }
	public void LoadSceneDemo18()
    {
        SceneManager.LoadScene("MagicPortal18");
    }
	public void LoadSceneDemo19()
    {
        SceneManager.LoadScene("MagicPortal19");
    }
	public void LoadSceneDemo20()
    {
        SceneManager.LoadScene("MagicPortal20");
    }
	public void LoadSceneDemo21()
    {
        SceneManager.LoadScene("MagicPortal21");
    }
	
	void Update ()
	 {
 
     if(Input.GetKeyDown(KeyCode.J))
	 {
         GUIHide = !GUIHide;
     
         if (GUIHide)
		 {
             GameObject.Find("CanvasSceneSelect").GetComponent<Canvas> ().enabled = false;
         }
		 else
		 {
             GameObject.Find("CanvasSceneSelect").GetComponent<Canvas> ().enabled = true;
         }
     }
	      if(Input.GetKeyDown(KeyCode.K))
	 {
         GUIHide2 = !GUIHide2;
     
         if (GUIHide2)
		 {
             GameObject.Find("Canvas").GetComponent<Canvas> ().enabled = false;
         }
		 else
		 {
             GameObject.Find("Canvas").GetComponent<Canvas> ().enabled = true;
         }
     }
		if(Input.GetKeyDown(KeyCode.L))
	 {
         GUIHide3 = !GUIHide3;
     
         if (GUIHide3)
		 {
             GameObject.Find("CanvasTips").GetComponent<Canvas> ().enabled = false;
         }
		 else
		 {
             GameObject.Find("CanvasTips").GetComponent<Canvas> ().enabled = true;
         }
     }
	 }
}
}