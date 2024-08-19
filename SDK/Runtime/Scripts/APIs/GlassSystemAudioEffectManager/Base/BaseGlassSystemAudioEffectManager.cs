
using FfalconXR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RayNeo.API {

    #region enum GlassSystemAudioEffectKey

    /// <summary>
    /// �۾�ϵͳ��Ч key
    /// �ο���ַ��https://leiniao-ibg.feishu.cn/docx/CJfsdoI7FoGDVpx52tVcMaLqnRe?from=from_copylink
    /// { "rayneo_slide_screen", "rayneo_click", "rayeno_double_click"}
    /// ע�⣺���԰���ַ����Ҫ����Ƶ���ƶ�Ӧ��ӵ� GlassSystemAudioEffectKey ö�����棬����Ҫ��ɾ������
    /// </summary>
    public enum GlassSystemAudioEffectKey
    {
        rayneo_slide_screen,
        rayneo_click,
        rayeno_double_click
    }

    #endregion

    /// <summary>
    /// �۾�ϵͳ��Ч ����
    /// </summary>
    public class BaseGlassSystemAudioEffectManager<T> :Singleton<T>,IGlassSystemAudioEffectManager 
        where T : BaseGlassSystemAudioEffectManager<T> 
    {
        #region Data

        /// <summary>
        /// TAG
        /// </summary>
        protected virtual string TAG { get; } = "[BaseGlassSystemAudioEffectManager] ";

        /// <summary>
        /// ����ϵͳ���б�
        /// </summary>
        protected List<string> mGlassSysAudEffectList= new List<string>();

        /// <summary>
        /// Android �� GlassSystemAudioEffectManager
        /// </summary>
        protected AndroidJavaObject m_AndroidJavaSysAudEffMgr;
        protected AndroidJavaObject AndroidJavaSysAudEffMgr
        {
            get
            {
                if (m_AndroidJavaSysAudEffMgr == null)
                {
                    m_AndroidJavaSysAudEffMgr = new AndroidJavaObject(PlatformAndroid.SupportPackagePath + ".GlassSystemAudioEffectManager.GlassSystemAudioEffectManager");
                }

                return m_AndroidJavaSysAudEffMgr;
            }
        }

        #endregion

        #region Lifecycle function

        /// <summary>
        /// OnSingletonInit
        /// </summary>
        protected override void OnSingletonInit()
        {
            base.OnSingletonInit();
        }

        #endregion

        #region Interface function

        // <summary>
        /// ��ʼ��
        /// ��� GlassSystemAudioEffectKey ö�� ������
        /// ���г�ʼ����ȡ��Ӧ��ϵͳ��Ч id
        /// ���ԣ��ǵ���ǰ��ʼ��
        /// </summary>
        public virtual void Init() {
            Debug.Log(TAG + "Init����");
            AndroidJavaSysAudEffMgr.Call("init", GetGlassSysAudEffectListJsonStr());
        }

        /// <summary>
        /// ����ָ����ϵͳ��Ч
        /// </summary>
        /// <param name="soundKey"></param>
        public virtual void PlaySoundEffect(GlassSystemAudioEffectKey soundKey) {
            Debug.Log(TAG + "PlaySoundEffect����soundKey = " + soundKey);
            AndroidJavaSysAudEffMgr.Call("playSoundEffect", soundKey.ToString());
        }

        /// <summary>
        /// ֹͣ����ָ����ϵͳ��Ч
        /// </summary>
        /// <param name="soundKey"></param>
        public virtual void StopSoundEffect(GlassSystemAudioEffectKey soundKey) {
            Debug.Log(TAG + "StopSoundEffect����soundKey = "+ soundKey);
            AndroidJavaSysAudEffMgr.Call("stopSoundEffect", soundKey.ToString());
        }

        #endregion

        #region Protected function

        /// <summary>
        /// ��ȡϵͳ�ִ�
        /// ��ö��תΪ�ִ�
        /// </summary>
        /// <returns></returns>
        protected virtual string GetGlassSysAudEffectListJsonStr() {
            Debug.Log(TAG + "GetGlassSysAudEffectList����");
            string jsonString;
            List<string> glassSysAudEffectList = new List<string>();
            foreach (var item in Enum.GetNames(typeof(GlassSystemAudioEffectKey)))
            {
                Debug.Log(TAG + "GetGlassSysAudEffectList() add " + item);
                mGlassSysAudEffectList.Add(item);
            }
            jsonString = JsonConvert.SerializeObject(mGlassSysAudEffectList);
            if (jsonString == null) Debug.LogError(TAG + "GetGlassSysAudEffectList() GlassSystemAudioEffectKey is null ");

            return jsonString;
        }

        #endregion
    }
}
