using FfalconXR;
using NatSuite.Recorders;
using NatSuite.Recorders.Clocks;
using NatSuite.Recorders.Inputs;
using UnityEngine;
namespace RayNeo.Record
{

    public class RecordManager
    {

        private static RecordManager m_rm = new RecordManager();

        public static RecordManager Ins
        {
            get { return m_rm; }
        }

        public RealtimeClock m_clock;
        public MP4Recorder m_recorder;
        public CameraInput m_cameraInput;
        public void StartRecord(Camera c, int w = 640, int h = 480, int fps = 30)
        {
            if (m_cameraInput != null)
            {
                return;//�����ٴο���.
            }
            m_clock = new RealtimeClock();
            m_recorder = new MP4Recorder(w, h, fps);//����¼��
            m_cameraInput = new CameraInput(m_recorder, m_clock, c != null ? c : Camera.main);//������������

        }

        public async void StopRecord()
        {
            if (m_cameraInput == null)
            {
                return;
            }
            m_cameraInput.Dispose();//��������ֹͣ
            string m_resultPath = await m_recorder.FinishWriting();//¼�����д�벢����
            Log.Error($"Saved recording to: {m_resultPath}");
            m_cameraInput = null;
        }
    }
}
