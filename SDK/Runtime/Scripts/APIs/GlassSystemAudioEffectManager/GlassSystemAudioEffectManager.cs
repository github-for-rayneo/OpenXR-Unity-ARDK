

namespace RayNeo.API
{
    /// <summary>
    /// 眼镜系统音效 实现类
    /// </summary>
    public class GlassSystemAudioEffectManager : BaseGlassSystemAudioEffectManager<GlassSystemAudioEffectManager>
        , IGlassSystemAudioEffectManager
    {

        /// <summary>
        /// 初始化
        /// 会把 GlassSystemAudioEffectKey 枚举 的数据
        /// 进行初始化获取对应的系统音效 id
        /// 所以，记得提前初始化
        /// </summary>
        public override void Init()
        {
            base.Init();
        }

        /// <summary>
        /// 播发指定的系统音效
        /// </summary>
        /// <param name="soundKey"></param>
        public override void PlaySoundEffect(GlassSystemAudioEffectKey soundKey)
        {
            base.PlaySoundEffect(soundKey);
        }

        /// <summary>
        /// 停止播放指定的系统音效
        /// </summary>
        /// <param name="soundKey"></param>
        public override void StopSoundEffect(GlassSystemAudioEffectKey soundKey)
        {
            base.StopSoundEffect(soundKey);
        }
    }

}
