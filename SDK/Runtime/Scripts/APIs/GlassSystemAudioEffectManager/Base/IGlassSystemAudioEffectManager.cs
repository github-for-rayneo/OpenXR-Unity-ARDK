using System;

namespace RayNeo.API { 

    /**
     * 眼镜系统音效 接口
     */
    public interface IGlassSystemAudioEffectManager
    {

        /// <summary>
        /// 初始化
        /// </summary>
        void Init();

        /// <summary>
        /// 播发指定的系统音效
        /// </summary>
        /// <param name="soundKey"></param>
        void PlaySoundEffect(GlassSystemAudioEffectKey soundKey);

        /// <summary>
        /// 停止播放指定的系统音效
        /// </summary>
        /// <param name="soundKey"></param>
        void StopSoundEffect(GlassSystemAudioEffectKey soundKey);
    }

}
