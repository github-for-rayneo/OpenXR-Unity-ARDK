using UnityEngine;
using UnityEngine.EventSystems;

namespace RayNeo
{
    public class UIHoverScaler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {

        public Vector3 m_onHoverScaleValue = new Vector3(1.1f, 1.1f, 1.1f);
        public Vector3 m_onExitScaleValue = Vector3.one;

        public AnimationCurve m_scaleAnim = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));

        public float m_animSpeed = 2;

        public bool m_is2D = true;
        private bool m_onFocus = false;
        private float m_curAnimTime;
        private void Awake()
        {

        }

        private void Update()
        {
            if (m_onFocus)
            {
                if (m_curAnimTime < 1)
                {
                    m_curAnimTime += Time.deltaTime * m_animSpeed;
                    if (m_curAnimTime > 1)
                    {
                        m_curAnimTime = 1;
                    }


                    float scaleValue = m_scaleAnim.Evaluate(m_curAnimTime);


                    gameObject.transform.localScale = new Vector3(
                         m_onExitScaleValue.x + (m_onHoverScaleValue.x - m_onExitScaleValue.x) * scaleValue
                        , m_onExitScaleValue.y + (m_onHoverScaleValue.y - m_onExitScaleValue.y) * scaleValue
                        , m_is2D ? 1 : (m_onExitScaleValue.z + (m_onHoverScaleValue.z - m_onExitScaleValue.z) * scaleValue));
                }
            }
            else
            {
                if (m_curAnimTime > 0)
                {
                    m_curAnimTime -= Time.deltaTime * m_animSpeed;
                    if (m_curAnimTime < 0)
                    {
                        m_curAnimTime = 0;
                    }
                    float scaleValue = m_scaleAnim.Evaluate(m_curAnimTime);
                    gameObject.transform.localScale = new Vector3(
                     m_onExitScaleValue.x + (m_onHoverScaleValue.x - m_onExitScaleValue.x) * scaleValue
                    , m_onExitScaleValue.y + (m_onHoverScaleValue.y - m_onExitScaleValue.y) * scaleValue
                    , m_is2D ? 1 : (m_onExitScaleValue.z + (m_onHoverScaleValue.z - m_onExitScaleValue.z) * scaleValue));
                }
            }
        }


        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!gameObject.activeSelf) return;
            m_onFocus = true;
            //gameObject.transform.localScale = m_onHoverScaleValue;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!gameObject.activeSelf) return;
            m_onFocus = false;
            //gameObject.transform.localScale = m_onExitScaleValue;

        }
    }
}
