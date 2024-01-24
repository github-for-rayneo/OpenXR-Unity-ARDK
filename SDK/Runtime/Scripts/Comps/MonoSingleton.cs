using System;
using UnityEngine;

namespace RayNeo
{
    /// <summary>
    /// Mono单例
    /// </summary>
    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        private static T m_Instance;

        /// <summary>
        /// 单例对象 
        /// </summary>
        public static T Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    T[] ts = GameObject.FindObjectsOfType<T>();

                    if (ts != null && ts.Length > 0)
                    {
                        if (ts.Length == 1)
                        {
                            m_Instance = ts[0];
                        }
                        else
                        {
                            throw new Exception(string.Format("## Uni Exception ## Cls:{0} Info:Singleton not allows more than one instance", typeof(T)));
                        }
                    }
                    else
                    {
                        m_Instance = new GameObject(string.Format("{0}(Singleton)", typeof(T).ToString())).AddComponent<T>();
                        m_Instance.OnSingletonInit();
                    }
                }

                return m_Instance;
            }
        }
        public static bool SingletonExist
        {
            get => m_Instance != null;
        }

        protected MonoSingleton() { }

        protected virtual void Awake()
        {
            m_Instance = this as T;
            //DontDestroyOnLoad(this.gameObject);
        }

        protected virtual void OnDestroy()
        {
            m_Instance = null;
        }

        protected virtual void OnSingletonInit()
        {
        }
    }
}