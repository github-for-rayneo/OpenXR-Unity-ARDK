using FfalconXR.InputModule;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace RayNeo.Editor
{
    public class DebugMono : MonoBehaviour
    {
        public static DebugMono dm;
        private bool onCtrl = false;//是否按下了ctrl  以用鼠标键盘控制

        //private Vector3 oldGlassV3;//进入控制的偏移量.
        //private Vector3 oldMobileV3;//进入控制的偏移量.
        private Vector3 mouseStartTag;//鼠标记录值

        float xRotOff = 0;//相机的偏移积累量
        float yRotOff = 0;//相机的偏移积累量
        float zRotOff = 0;//相机的偏移积累量

        float xPosOff = 0;
        float yPosOff = 0;
        float zPosOff = 0;

        //private float mouseToRayRate = 54;//鼠标位移到射线旋转的比值
        private float cameraMoveSpeed = 0.3f;//相机移动的速率



        private Vector2 m_rayTouchPos = Vector3.zero;
        //private RingTouchMotionEventType m_rayTouchEvent = RingTouchMotionEventType.NONE;
        //private Vector3 m_rayRotation = Vector3.zero;
        private float m_rayRotSpeed = 0.3f;//


        private void Awake()
        {
            SimulatorStateChange(DebugMidway.IsInSimulator);
            DebugMidway.OnSimulatorChange += SimulatorStateChange;
        }

        private void OnDestroy()
        {
            DebugMidway.OnSimulatorChange -= SimulatorStateChange;

        }
        private void SimulatorStateChange(bool isInSimulator)
        {
            onCtrl = isInSimulator;
        }
        public static void AddMono()
        {

            if (dm != null)
            {
                return;
            }
            else
            {
                GameObject go = (GameObject)GameObject.FindObjectOfType(typeof(DebugMono));
                if (go != null)
                {
                    dm = go.GetComponent<DebugMono>();
                    if (dm != null)
                    {
                        return;
                    }
                }
            }
            new GameObject("DebugMono").AddComponent<DebugMono>();
        }
        private void Start()
        {
            SceneManager.sceneLoaded += OnSceneChange;
            if (dm)
            {
                Destroy(this);
                return;
            }
            dm = this;
            DontDestroyOnLoad(this);
        }
        private void OnSceneChange(Scene s, LoadSceneMode m)
        {

        }

        private void Update()
        {
            if (!onCtrl)
            {
                return;
            }

            HandleCameraRotate();
        }
        private void HandleCameraRotate()
        {
            var curKey = Keyboard.current;

            if (curKey.wKey.isPressed)
            {
                xRotOff += -cameraMoveSpeed;
            }
            else if (curKey.sKey.isPressed)
            {
                xRotOff += cameraMoveSpeed;
            }
            if (curKey.aKey.isPressed)
            {
                yRotOff += -cameraMoveSpeed;
            }
            else if (curKey.dKey.isPressed)
            {
                yRotOff += cameraMoveSpeed;
            }
            if (curKey.qKey.isPressed)
            {
                zRotOff += -cameraMoveSpeed;
            }
            else if (curKey.eKey.isPressed)
            {
                zRotOff += cameraMoveSpeed;
            }
            HeadTrackedPoseDriver.SetQuaternion(Quaternion.Euler(xRotOff, yRotOff, zRotOff));
            if (curKey.jKey.isPressed)
            {
                xPosOff += -cameraMoveSpeed;
            }
            else if (curKey.lKey.isPressed)
            {
                xPosOff += cameraMoveSpeed;
            }
            if (curKey.kKey.isPressed)
            {
                zPosOff += -cameraMoveSpeed;
            }
            else if (curKey.iKey.isPressed)
            {
                zPosOff += cameraMoveSpeed;
            }
            if (curKey.uKey.isPressed)
            {
                yPosOff += -cameraMoveSpeed;
            }
            else if (curKey.oKey.isPressed)
            {
                yPosOff += cameraMoveSpeed;
            }

            HeadTrackedPoseDriver.SetPosition(new Vector3(xPosOff, yPosOff, zPosOff) * 3);

            //if (curKey.leftArrowKey.isPressed)
            //{
            //    m_rayRotation.y -= m_rayRotSpeed;
            //}
            //else if (curKey.rightArrowKey.isPressed)
            //{
            //    m_rayRotation.y += m_rayRotSpeed;
            //}
            //if (curKey.upArrowKey.isPressed)
            //{
            //    m_rayRotation.x -= m_rayRotSpeed;
            //}
            //else if (curKey.downArrowKey.isPressed)
            //{
            //    m_rayRotation.x += m_rayRotSpeed;
            //}
            //RingManager.OnRotation(Quaternion.Euler(m_rayRotation));


            //  else if (curKey.numpad7Key.wasReleasedThisFrame)
            //  {
            //      m_rayTouchEvent = RingTouchMotionEventType.ACTION_UP;
            //  }
            //  else if (curKey.numpad8Key.wasPressedThisFrame || curKey.numpad4Key.wasPressedThisFrame ||
            //curKey.numpad5Key.wasPressedThisFrame || curKey.numpad6Key.wasPressedThisFrame)
            //  {
            //      if (m_rayTouchEvent != RingTouchMotionEventType.ACTION_MOVE)
            //      {
            //          m_rayTouchEvent = RingTouchMotionEventType.ACTION_DOWN;
            //      }
            //  }
            //  else if (curKey.numpad8Key.wasReleasedThisFrame && curKey.numpad4Key.wasReleasedThisFrame &&
            //   curKey.numpad5Key.wasReleasedThisFrame && curKey.numpad6Key.wasReleasedThisFrame)
            //  {
            //      m_rayTouchEvent = RingTouchMotionEventType.ACTION_UP;

            //  }
            if (curKey.numpad8Key.isPressed)
            {
                m_rayTouchPos.y += m_rayRotSpeed;
            }
            else if (curKey.numpad5Key.isPressed)
            {
                m_rayTouchPos.y -= m_rayRotSpeed;
            }
            else if (curKey.numpad4Key.isPressed)
            {
                m_rayTouchPos.x -= m_rayRotSpeed;
            }
            else if (curKey.numpad6Key.isPressed)
            {
                m_rayTouchPos.x += m_rayRotSpeed;
            }


            //if (m_rayTouchEvent != RingTouchMotionEventType.NONE)
            //{
            //    RingManager.OnTouch((int)m_rayTouchEvent, m_rayTouchPos.x, m_rayTouchPos.y);
            //}
            //if (m_rayTouchEvent == RingTouchMotionEventType.ACTION_UP)
            //{
            //    m_rayTouchEvent = RingTouchMotionEventType.NONE;
            //}
            LaserDebug();
            //       private Vector2 m_rayTouchPos;
            //private RingTouchMotionEventType m_rayTouchEvent = RingTouchMotionEventType.NONE;
            //private Vector3 m_rayRotation;


        }
        private EventSystem m_CurrentEventSystem;
        protected List<RaycastResult> m_RaycastResultCache = new List<RaycastResult>();
        protected readonly List<Graphic> m_RaycastResults = new List<Graphic>();

        private void LaserDebug()
        {
            if (m_CurrentEventSystem != EventSystem.current)
            {
                m_CurrentEventSystem = EventSystem.current;
            }

            Vector3 point = Vector3.zero;
            RayTrackedPoseDriver driver = GameObject.FindObjectOfType<RayTrackedPoseDriver>();
            if (driver == null)
            {
                return;
            }
            Quaternion rot;
            if (DigoutRaycastPoint(ref point))
            {
                rot = Quaternion.LookRotation((point - driver.transform.position).normalized);
            }
            else
            {
                Vector2 screenPoint = Mouse.current.position.ReadValue();
                Ray ray = Camera.main.ScreenPointToRay(screenPoint);
                float distance = Vector3.Dot(driver.transform.forward, ray.direction);
                Vector3 worldP = Camera.main.ScreenToWorldPoint(new Vector3(screenPoint.x, screenPoint.y, distance * driver.m_BeamLine.transform.localScale.z));
                rot = Quaternion.LookRotation((worldP - driver.transform.position).normalized);
            }

            if (driver.m_RayControllerType == RayControllerType.Ring)
            {
                RingManager.OnRotation(rot);
                //m_rayTouchPos 暂时未定义该测试接口.
                if (Mouse.current.leftButton.wasPressedThisFrame)
                {
                    RingManager.OnTouch((int)RingTouchMotionEventType.ACTION_DOWN, m_rayTouchPos.x, m_rayTouchPos.y);
                }
                else if (Mouse.current.leftButton.isPressed)
                {
                    RingManager.OnTouch((int)RingTouchMotionEventType.ACTION_MOVE, m_rayTouchPos.x, m_rayTouchPos.y);
                }
                else if (Mouse.current.leftButton.wasReleasedThisFrame)
                {
                    RingManager.OnTouch((int)RingTouchMotionEventType.ACTION_UP, m_rayTouchPos.x, m_rayTouchPos.y);
                }
                if (Mouse.current.rightButton.wasPressedThisFrame)
                {
                    RingManager.OnTouchPadHeavyPressChange(true);
                }
                else if (Mouse.current.rightButton.wasReleasedThisFrame)
                {
                    RingManager.OnTouchPadHeavyPressChange(false);
                }

                //if (curKey.numpad7Key.wasPressedThisFrame)
                //{
                //m_rayTouchEvent = RingTouchMotionEventType.ACTION_CANCEL;
                //RingManager.OnTouch((int)m_rayTouchEvent, m_rayTouchPos.x, m_rayTouchPos.y);
                //m_rayTouchEvent = RingTouchMotionEventType.ACTION_DOWN;
                //RingManager.OnTouch((int)m_rayTouchEvent, m_rayTouchPos.x, m_rayTouchPos.y);
                //m_rayTouchEvent = RingTouchMotionEventType.ACTION_MOVE;

                //}

            }
            else
            {
                RingManager.OnRotation(rot);
                driver.transform.rotation = rot;
            }

        }

        private bool DigoutRaycastPoint(ref Vector3 point)
        {
            Vector2 screenPoint = Mouse.current.position.ReadValue();
            //Quaternion.LookRotation
            m_RaycastResultCache.Clear();
            Canvas[] cs = GameObject.FindObjectsOfType<Canvas>();
            for (int i = 0; i < cs.Length; i++)
            {
                if (cs[i].GetComponent<XRGraphicRaycaster>() != null)
                {
                    RaycastByScreenPoint(cs[i], Camera.main, screenPoint, m_RaycastResultCache);
                }
            }


            float distance = 0;
            bool digout = false;
            //if(m_RaycastResultCache)
            if (m_RaycastResultCache.Count > 0)
            {
                digout = true;
                RaycastResult firstResult = m_RaycastResultCache[0];
                for (int i = 1; i < m_RaycastResultCache.Count; i++)
                {

                    if (firstResult.distance > m_RaycastResultCache[i].distance)
                    {
                        firstResult = m_RaycastResultCache[i];
                    }
                }
                point = firstResult.worldPosition;
                distance = firstResult.distance;
            }


            Ray ray = Camera.main.ScreenPointToRay(screenPoint);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, 0))
            {
                if (digout)
                {
                    if (hit.distance < distance)
                    {
                        point = hit.point;
                    }
                }
                else
                {
                    point = hit.point;
                }
                digout = true;
            }
            return digout;
        }
        public void RaycastByScreenPoint(Canvas canvas, Camera cacheCamera, Vector2 eventPosition, List<RaycastResult> resultAppendList)
        {
            Ray ray = cacheCamera.ScreenPointToRay(eventPosition);

            m_RaycastResults.Clear();
            var canvasGraphics = GraphicRegistry.GetGraphicsForCanvas(canvas);
            if (canvasGraphics == null || canvasGraphics.Count == 0)
                return;
            float distance = 0;
            Graphic graphic = Raycast(canvas, cacheCamera, eventPosition, canvasGraphics, ray, out distance);

            if (graphic != null)
            {
                var castResult = new RaycastResult
                {
                    gameObject = graphic.gameObject,
                    //module = this,
                    distance = distance,
                    screenPosition = eventPosition,
                    index = resultAppendList.Count,
                    depth = graphic.depth,
                    sortingLayer = canvas.sortingLayerID,
                    sortingOrder = canvas.sortingOrder,
                    worldPosition = ray.origin + ray.direction * distance,
                    worldNormal = -graphic.transform.forward
                };
                resultAppendList.Add(castResult);
            }
        }

        protected Graphic Raycast(Canvas canvas, Camera eventCamera, Vector2 pointerPosition, IList<Graphic> foundGraphics, Ray ray, out float distance)
        {
            // Necessary for the event system
            int totalCount = foundGraphics.Count;

            Graphic output = null;
            distance = 0;
            int maxDepth = -1;

            //根据开发制作预制体普遍习惯(节点从上往下开发)
            //节点越下，graphic注册越迟 所以采用倒序遍历，可以通过depth过滤很多graphic的判定
            for (int i = totalCount - 1; i >= 0; i--)
            {
                Graphic graphic = foundGraphics[i];

                int depth = graphic.depth;

                if (depth <= maxDepth)
                    continue;

                if (depth == -1 || !graphic.raycastTarget || graphic.canvasRenderer.cull)
                    continue;

                if (!RectTransformUtility.RectangleContainsScreenPoint(graphic.rectTransform, pointerPosition, eventCamera))
                    continue;

                if (eventCamera != null && eventCamera.WorldToScreenPoint(graphic.rectTransform.position).z > eventCamera.farClipPlane)
                    continue;

                if (graphic.Raycast(pointerPosition, eventCamera))
                {
                    //判断距离
                    if (eventCamera == null || canvas.renderMode == RenderMode.ScreenSpaceOverlay)
                    {
                        distance = 0;
                    }
                    else
                    {
                        Transform trans = graphic.transform;
                        distance = (Vector3.Dot(trans.forward, trans.position - ray.origin) / Vector3.Dot(trans.forward, ray.direction));

                        // Check to see if the go is behind the camera.
                        if (distance < 0)
                            continue;
                    }

                    output = graphic;
                    maxDepth = depth;
                }
            }

            return output;
        }

        private void OnGUI()
        {
            if (onCtrl)
            {
                //这是水星的提示符
                //GUILayout.Label("鼠标控制射线左右上下\nwasd控制镜头左右上下,qe控制旋转\nijkl控制平面位置，uo控制上下位置");
                GUILayout.Label("鼠标控制射线左右上下\nwasd控制镜头左右上下,qe控制旋转\nijkl控制平面位置，uo控制上下位置");

                if (RingManager.RingOpened)
                {
                    GUILayout.Label("左键模拟戒指开始触摸操作.右键模拟戒指触摸板按下操作.");
                }
            }
        }

    }

    [InitializeOnLoad]
    public class DebugWindow
    {
        private static string cachedCurrentScene;
        private static string currentScene
        {
            get
            {
                return cachedCurrentScene;
            }
            set
            {
                cachedCurrentScene = value;
            }
        }
        static DebugWindow()
        {

            EditorApplication.update += Update;
        }

        public static void Update()
        {
            if (EditorApplication.isPlaying)
            {
                if (DebugMono.dm == null)
                {
                    DebugMono.AddMono();
                }
            }
        }
    }


}