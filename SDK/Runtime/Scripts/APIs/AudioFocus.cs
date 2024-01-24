//using FFalcon.XR.Runtime;
using System;
using UnityEngine;
namespace RayNeo.API
{
    public enum AudioFocusStatus
    {
        AUDIOFOCUS_LOSS_TRANSIENT_CAN_DUCK = -3,
        AUDIOFOCUS_LOSS_TRANSIENT = -2,
        AUDIOFOCUS_LOSS = -1,
        AUDIOFOCUS_NONE = 0,
        AUDIOFOCUS_GAIN_TRANSIENT_EXCLUSIVE = 4,
        AUDIOFOCUS_GAIN_TRANSIENT_MAY_DUCK = 3,
        AUDIOFOCUS_GAIN_TRANSIENT = 2,
        AUDIOFOCUS_GAIN = 1
    }
    public class AudioFocus
    {

        private static Action<AudioFocusStatus> m_audioFocusStatusCallback;
        private static SetAudioFocusListener m_audioFocusListener;

        private static AndroidJavaObject m_AudioFocus;
        private static AndroidJavaObject AudioManager
        {
            get
            {
                if (m_AudioFocus == null)
                {
                    m_AudioFocus = new AndroidJavaObject(PlatformAndroid.SupportPackagePath + ".AudioFocusManager");

                }

                return m_AudioFocus;
            }
        }
        //抢占或释放焦点
        public static bool SetAudioFocusStatus(bool IsRequest)
        {

            //传入true，表征抢占焦点.传入false，表征释放焦点
#if UNITY_ANDROID && !UNITY_EDITOR
            CheckAudioListenerExist();
               bool IsSuccess = AudioManager.Call<bool>("setAudioFocus", PlatformAndroid.ApplicationContext, m_audioFocusListener, IsRequest);
            return IsSuccess;
#elif UNITY_EDITOR
            return false;
#endif
        }

        private static void CheckAudioListenerExist()
        {
            if (m_audioFocusListener == null)
            {
                m_audioFocusListener = new SetAudioFocusListener();
            }
        }

        public static void RegistAudioFocusChangeCallBack(Action<AudioFocusStatus> call)
        {
            m_audioFocusStatusCallback += call;

        }
        public static void UnRegistAudioFocusChangeCallBack(Action<AudioFocusStatus> call)
        {
            m_audioFocusStatusCallback -= call;
        }

        public sealed class SetAudioFocusListener : AndroidJavaProxy
        {

            public SetAudioFocusListener() : base("android.media.AudioManager$OnAudioFocusChangeListener")
            {
            }

            public void onAudioFocusChange(int AudioRequest)
            {
                MainThreadQueue.Instance.ExecuteQueue.Enqueue(() =>
                {
                    m_audioFocusStatusCallback?.Invoke((AudioFocusStatus)AudioRequest);
                });
            }
        }

    }
}
