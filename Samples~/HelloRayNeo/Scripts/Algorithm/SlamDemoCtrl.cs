using RayNeo.API;
using UnityEngine;

public class SlamDemoCtrl : MonoBehaviour
{
    public GameObject m_Cube;
    public int m_LineCount = 10;
    public float m_CubeSpace = 0.3f;
    // Start is called before the first frame update
    void Start()
    {
        Algorithm.EnableSlamHeadTracker();
        CreateCubes();
    }


    private void OnDestroy()
    {
        Algorithm.DisableSlamHeadTracker();
    }
    private void CreateCubes()
    {

        int centerPoint = m_LineCount / 2;

        for (int i = 0; i < m_LineCount; i++)
        {
            for (int j = 0; j < m_LineCount; j++)
            {
                var c = Instantiate(m_Cube);
                c.transform.position = new Vector3(m_CubeSpace * (i - centerPoint), m_Cube.transform.position.y, (m_CubeSpace * (j - centerPoint)));

            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
