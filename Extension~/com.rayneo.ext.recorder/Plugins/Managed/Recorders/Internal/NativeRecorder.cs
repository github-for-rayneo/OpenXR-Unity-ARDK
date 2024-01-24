/* 
*   NatCorder
*   Copyright (c) 2020 Yusuf Olokoba.
*/

namespace NatSuite.Recorders.Internal {

    using AOT;
    using System;
    using System.Runtime.InteropServices;
    using System.Threading.Tasks;

    public sealed class NativeRecorder : IMediaRecorder {
        
        #region --IMediaRecorder--

        public (int width, int height) frameSize {
            get {
                recorder.FrameSize(out var width, out var height);
                return (width, height);
            }
        }

        public NativeRecorder (Func<Bridge.RecordingHandler, IntPtr, IntPtr> recorderCreator) {
            this.recordingTask = new TaskCompletionSource<string>();
            var handle = GCHandle.Alloc(recordingTask, GCHandleType.Normal);
            this.recorder = recorderCreator(OnRecording, (IntPtr)handle);
        }

        public void CommitFrame<T> (T[] pixelBuffer, long timestamp) where T : struct {
            var handle = GCHandle.Alloc(pixelBuffer, GCHandleType.Pinned);
            CommitFrame(handle.AddrOfPinnedObject(), timestamp);
            handle.Free();
        }

        public void CommitFrame (IntPtr nativeBuffer, long timestamp) => recorder.CommitFrame(nativeBuffer, timestamp);

        public void CommitSamples (float[] sampleBuffer, long timestamp) => recorder.CommitSamples(sampleBuffer, sampleBuffer.Length, timestamp);

        public Task<string> FinishWriting () {
            recorder.FinishWriting();
            return recordingTask.Task;
        }
        #endregion


        #region --Operations--

        private readonly IntPtr recorder;
        private readonly TaskCompletionSource<string> recordingTask;

        [MonoPInvokeCallback(typeof(Bridge.RecordingHandler))]
        private static void OnRecording (IntPtr context, IntPtr path) {
            // Get task
            var handle = (GCHandle)context;
            var recordingTask = handle.Target as TaskCompletionSource<string>;
            handle.Free();
            // Invoke completion task
            if (path != IntPtr.Zero)
                recordingTask.SetResult(Marshal.PtrToStringAnsi(path));
            else
                recordingTask.SetException(new Exception(@"Recorder failed to finish writing"));
        }
        #endregion
    }
}