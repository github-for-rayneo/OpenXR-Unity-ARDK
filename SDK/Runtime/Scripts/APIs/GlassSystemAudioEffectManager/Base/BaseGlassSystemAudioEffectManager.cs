
using FfalconXR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RayNeo.API {

    #region enum GlassSystemAudioEffectKey

    /// <summary>
    /// 眼镜系统音效 key
    /// 参考地址：https://leiniao-ibg.feishu.cn/docx/CJfsdoI7FoGDVpx52tVcMaLqnRe?from=from_copylink
    /// { "rayneo_slide_screen", "rayneo_click", "rayeno_double_click"}
    /// 注意：可以把网址上需要的音频名称对应添加到 GlassSystemAudioEffectKey 枚举里面，不需要的删掉即可
    /// </summary>
    public enum GlassSystemAudioEffectKey
    {
        rayneo_slide_screen,
        rayneo_click,
        rayeno_double_click
    }

    #endregion

    /// <summary>
    /// 眼镜系统音效 基类
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
        /// 保存系统音列表
        /// </summary>
        protected List<string> mGlassSysAudEffectList= new List<string>();

        /// <summary>
        /// Android 端 GlassSystemAudioEffectManager
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
        /// 初始化
        /// 会把 GlassSystemAudioEffectKey 枚举 的数据
        /// 进行初始化获取对应的系统音效 id
        /// 所以，记得提前初始化
        /// </summary>
        public virtual void Init() {
            Debug.Log(TAG + "Init（）");
            AndroidJavaSysAudEffMgr.Call("init", GetGlassSysAudEffectListJsonStr());
        }

        /// <summary>
        /// 播发指定的系统音效
        /// </summary>
        /// <param name="soundKey"></param>
        public virtual void PlaySoundEffect(GlassSystemAudioEffectKey soundKey) {
            Debug.Log(TAG + "PlaySoundEffect（）soundKey = " + soundKey);
            AndroidJavaSysAudEffMgr.Call("playSoundEffect", soundKey.ToString());
        }

        /// <summary>
        /// 停止播放指定的系统音效
        /// </summary>
        /// <param name="soundKey"></param>
        public virtual void StopSoundEffect(GlassSystemAudioEffectKey soundKey) {
            Debug.Log(TAG + "StopSoundEffect（）soundKey = "+ soundKey);
            AndroidJavaSysAudEffMgr.Call("stopSoundEffect", soundKey.ToString());
        }

        #endregion

        #region Protected function

        /// <summary>
        /// 获取系统字串
        /// 把枚举转为字串
        /// </summary>
        /// <returns></returns>
        protected virtual string GetGlassSysAudEffectListJsonStr() {
            Debug.Log(TAG + "GetGlassSysAudEffectList（）");
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
