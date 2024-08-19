

namespace RayNeo.API
{
    /// <summary>
    /// �۾�ϵͳ��Ч ʵ����
    /// </summary>
    public class GlassSystemAudioEffectManager : BaseGlassSystemAudioEffectManager<GlassSystemAudioEffectManager>
        , IGlassSystemAudioEffectManager
    {

        /// <summary>
        /// ��ʼ��
        /// ��� GlassSystemAudioEffectKey ö�� ������
        /// ���г�ʼ����ȡ��Ӧ��ϵͳ��Ч id
        /// ���ԣ��ǵ���ǰ��ʼ��
        /// </summary>
        public override void Init()
        {
            base.Init();
        }

        /// <summary>
        /// ����ָ����ϵͳ��Ч
        /// </summary>
        /// <param name="soundKey"></param>
        public override void PlaySoundEffect(GlassSystemAudioEffectKey soundKey)
        {
            base.PlaySoundEffect(soundKey);
        }

        /// <summary>
        /// ֹͣ����ָ����ϵͳ��Ч
        /// </summary>
        /// <param name="soundKey"></param>
        public override void StopSoundEffect(GlassSystemAudioEffectKey soundKey)
        {
            base.StopSoundEffect(soundKey);
        }
    }

}
