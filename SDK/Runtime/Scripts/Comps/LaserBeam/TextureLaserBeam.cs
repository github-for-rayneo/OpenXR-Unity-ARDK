using FfalconXR.InputModule;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class TextureLaserBeam : LaserBeam
{

    public Texture m_normalImg;
    public Texture m_hoverImg;
    public Material m_textureMat;
    /// <summary>
    /// 缩放倍率.
    /// </summary>
    public float m_ImgScale = 2f;

    private
    protected new void Start()
    {
        base.Start();
        if (m_LaserBeamDot == null)
        {
            return;
        }
        GameObject textureLaser = new GameObject("TextureLaser");
        textureLaser.transform.SetParent(m_LaserBeamDot.parent);
        textureLaser.transform.localPosition = Vector3.forward;//不然绘制不出来.
        textureLaser.transform.localScale = Vector3.one;
        textureLaser.transform.localRotation = Quaternion.Euler(0, 0, 0);
        MeshRenderer mr = textureLaser.AddComponent<MeshRenderer>();
        mr.sortingOrder = m_SortingOrder;
        MeshFilter mf = textureLaser.AddComponent<MeshFilter>();
        m_textureMat.mainTexture = m_normalImg;
        mr.material = m_textureMat;
        //CreateReticleVertices(Init(m_SortingOrder));
        CreateQuad(mf);

    }

    private void CreateQuad(MeshFilter mf)
    {
        Mesh mesh = new Mesh();
        mf.mesh = mesh;
        Vector3[] vertices = new Vector3[4];
        vertices[0] = new Vector3(-1, -1, 0);
        vertices[1] = new Vector3(-1, 1, 0);
        vertices[2] = new Vector3(1, 1, 0);
        vertices[3] = new Vector3(1, -1, 0);
        int[] indices = new int[6];
        indices[0] = 0;
        indices[1] = 1;
        indices[2] = 2;
        indices[3] = 0;
        indices[4] = 2;
        indices[5] = 3;
        mesh.vertices = vertices;
        mesh.triangles = indices;
        mesh.RecalculateBounds();


    }

    public override void OnPointerEnter(RaycastResult raycastResult, GameObject interactiveObj)
    {
        m_textureMat.mainTexture = m_hoverImg;
        base.OnPointerEnter(raycastResult, interactiveObj);
    }

    public override void OnPointerExit(GameObject previousObject)
    {
        base.OnPointerExit(previousObject);
        m_textureMat.mainTexture = m_normalImg;
    }
    private new void Update()
    {
        base.Update();
        m_textureMat.SetFloat("_DistanceInMeters", m_Dis);
        m_textureMat.SetFloat("_ScaleValue", m_OuterDiameter * m_Dis * m_ImgScale);

    }
}
