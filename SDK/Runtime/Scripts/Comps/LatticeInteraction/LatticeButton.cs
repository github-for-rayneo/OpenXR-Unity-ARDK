using System;
using UnityEngine;
using static UnityEngine.UI.Button;

namespace RayNeo
{

    /// <summary>
    /// ���������ť
    /// </summary>
    public class LatticeButton : MonoBehaviour
    {
        public GameObject m_focusObj;
        //����Խ�� ����Խ��
        public int level = 1;
        //����Խ��Լ����
        public int order = 1;


        public Action<LatticeButton> OnFocus;
        public Action<LatticeButton> OnUnFocus;

        public object Params;

        public ButtonClickedEvent onClick { get; set; }

        public LatticeButton()
        {
            onClick = new ButtonClickedEvent();

        }

        void Awake()
        {
            LatticeBrain.RegButton(this);
            if (m_focusObj != null)
            {
                m_focusObj.SetActive(false);
            }
        }

        void OnDestroy()
        {
            LatticeBrain.RemoveButton(this);

        }

        public void MonoFocus()
        {
            OnFocus?.Invoke(this);
            if (m_focusObj != null)
            {
                m_focusObj.SetActive(true);
            }
        }

        public void MonoUnFocus()
        {
            OnUnFocus?.Invoke(this);
            if (m_focusObj != null)
            {
                m_focusObj.SetActive(false);
            }

        }
        //public bool InFocus
        //{
        //    get
        //    {
        //        return m_focusObj
        //    }
        //}

    }
}
