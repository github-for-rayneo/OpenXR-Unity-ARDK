/* 
*   NatCorder
*   Copyright (c) 2020 Yusuf Olokoba.
*/

namespace NatSuite.Recorders.Inputs
{

    using UnityEngine;
    using UnityEngine.Rendering;
    using System;
    using System.Collections;
    using Clocks;

    /// <summary>
    /// Recorder input for recording video frames from one or more game cameras.
    /// </summary>
    public sealed class CameraInput : IDisposable
    { // DEPLOY

        #region --Client API--
        /// <summary>
        /// Control number of successive camera frames to skip while recording.
        /// This is very useful for GIF recording, which typically has a lower framerate appearance.
        /// </summary>
        public int frameSkip;

        /// <summary>
        /// Create a video recording input from a game camera.
        /// </summary>
        /// <param name="recorder">Media recorder to receive committed frames.</param>
        /// <param name="clock">Clock for generating timestamps.</param>
        /// <param name="cameras">Game cameras to record.</param>
        public CameraInput(IMediaRecorder recorder, IClock clock, params Camera[] cameras)
        {
            // Sort cameras by depth
            Array.Sort(cameras, (a, b) => (int)(10 * (a.depth - b.depth)));
            var (width, height) = recorder.frameSize;
            // Save state
            this.recorder = recorder;
            this.clock = clock;
            this.cameras = cameras;
            this.frameDescriptor = new RenderTextureDescriptor(width, height, RenderTextureFormat.ARGB32, 24);
            this.readbackBuffer = SystemInfo.supportsAsyncGPUReadback ? null : new Texture2D(width, height, TextureFormat.RGBA32, false, false);
            this.attachment = cameras[0].gameObject.AddComponent<CameraInputAttachment>();
            this.pixelBuffer = new byte[width * height * 4];
            // Start recording
            frameDescriptor.sRGB = true;

            attachment.StartCoroutine(OnFrame());
        }

        /// <summary>
        /// Stop recorder input and release resources.
        /// </summary>
        public void Dispose()
        {
            CameraInputAttachment.Destroy(attachment);
            Texture2D.Destroy(readbackBuffer);
            pixelBuffer = null;
        }
        #endregion


        #region --Operations--

        private readonly IMediaRecorder recorder;
        private readonly IClock clock;
        private readonly Camera[] cameras;
        private readonly RenderTextureDescriptor frameDescriptor;
        private readonly Texture2D readbackBuffer;
        private readonly CameraInputAttachment attachment;
        private byte[] pixelBuffer;
        private int frameCount;

        private IEnumerator OnFrame()
        {
            var endOfFrame = new WaitForEndOfFrame();
            while (true)
            {
                // Check frame index
                yield return endOfFrame;
                if (frameCount++ % (frameSkip + 1) != 0)
                    continue;
                var frameBuffer = cameras[0].targetTexture;
                var timestamp = clock.timestamp;
                if (SystemInfo.supportsAsyncGPUReadback)
                    AsyncGPUReadback.Request(frameBuffer, 0, request =>
                    {
                        if (pixelBuffer != null)
                        {
                            request.GetData<byte>().CopyTo(pixelBuffer);
                            recorder.CommitFrame(pixelBuffer, timestamp);
                        }
                    });
                else
                {
                    var prevActive = RenderTexture.active;
                    RenderTexture.active = frameBuffer;
                    readbackBuffer.ReadPixels(new Rect(0, 0, frameBuffer.width, frameBuffer.height), 0, 0, false);
                    readbackBuffer.GetRawTextureData<byte>().CopyTo(pixelBuffer);
                    recorder.CommitFrame(pixelBuffer, timestamp);
                    RenderTexture.active = prevActive;
                }
                //RenderTexture.ReleaseTemporary(frameBuffer);
            }
        }

        private sealed class CameraInputAttachment : MonoBehaviour { }
        #endregion
    }
}