using RayNeo;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Scene1Ctrl : MonoBehaviour
{

    public LatticeButton m_defaultSelectBtn;

    public LatticeButton m_level2DefaultSelectBtn;
    public GameObject m_level2Obj;

    public LatticeButton m_deleteLevel1Btn;
    public LatticeButton m_deleteLevel2Btn;
    public LatticeButton m_backBtn;
    public Image OnTripleTapButtonImage;


    public void OnDoubleTapCallBack()
    {
        CloseLevel2();
    }

    void Start()
    {
        LatticeBrain.FocusLevel(m_defaultSelectBtn.level);
        LatticeBrain.SelectButton(m_defaultSelectBtn);
        LatticeBrain.Brain.OnDoubleTap += OnDoubleTapCallBack;
        m_defaultSelectBtn.onClick.AddListener(OnClick);
        m_level2DefaultSelectBtn.onClick.AddListener(CloseLevel2);
        m_deleteLevel1Btn.onClick.AddListener(DeleteLevel1);
        m_deleteLevel2Btn.onClick.AddListener(DeleteLevel2);
        m_backBtn.onClick.AddListener(BackToMainScene);

        SimpleTouch.Instance.OnTripleTap.AddListener(OnTripleTapButtonImageRandomColor);
    }

    private void OnTripleTapButtonImageRandomColor()
    {
        OnTripleTapButtonImage.color = new Color(Random.Range(0,1.0f), Random.Range(0, 1.0f), Random.Range(0, 1.0f));
    }

    private void BackToMainScene()
    {
        SceneManager.LoadScene("Entry");

    }

    private void DeleteLevel1()
    {
        LatticeBrain.RemoveButton(m_deleteLevel1Btn);
    }

    private void DeleteLevel2()
    {
        LatticeBrain.RemoveButton(m_deleteLevel2Btn);

    }

    private void CloseLevel2()
    {
        m_level2Obj.SetActive(false);
        LatticeBrain.MovingChangeItem = false;
        LatticeBrain.FocusLevel(m_defaultSelectBtn.level);
    }
    private void OnClick()
    {
        m_level2Obj.SetActive(true);
        LatticeBrain.MovingChangeItem = true;
        LatticeBrain.SelectButton(m_level2DefaultSelectBtn, true);
    }
    // Update is called once per frame
    void Update()
    {

    }

    private void OnDestroy()
    {
        SimpleTouch.Instance.OnTripleTap.RemoveListener(OnTripleTapButtonImageRandomColor);
    }
}
