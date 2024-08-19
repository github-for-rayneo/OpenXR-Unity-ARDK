using System;

namespace RayNeo.API { 

    /**
     * �۾�ϵͳ��Ч �ӿ�
     */
    public interface IGlassSystemAudioEffectManager
    {

        /// <summary>
        /// ��ʼ��
        /// </summary>
        void Init();

        /// <summary>
        /// ����ָ����ϵͳ��Ч
        /// </summary>
        /// <param name="soundKey"></param>
        void PlaySoundEffect(GlassSystemAudioEffectKey soundKey);

        /// <summary>
        /// ֹͣ����ָ����ϵͳ��Ч
        /// </summary>
        /// <param name="soundKey"></param>
        void StopSoundEffect(GlassSystemAudioEffectKey soundKey);
    }

}
