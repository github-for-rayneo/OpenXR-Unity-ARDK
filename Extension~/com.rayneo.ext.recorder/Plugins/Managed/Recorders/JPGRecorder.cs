/* 
*   NatCorder
*   Copyright (c) 2020 Yusuf Olokoba.
*/

namespace NatSuite.Recorders {

    using UnityEngine;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Threading.Tasks;
    using Internal;

    /// <summary>
    /// JPG image sequence recorder.
    /// This recorder is currently supported on macOS and Windows.
    /// This recorder is NOT thread-safe, and as such it is not fully compliant with the `IMediaRecorder` interfacex.
    /// </summary>
    public sealed class JPGRecorder : IMediaRecorder {

        #region --Client API--
        /// <summary>
        /// Image size.
        /// </summary>
        public (int width, int height) frameSize => (framebuffer.width, framebuffer.height);

        /// <summary>
        /// Create a JPG recorder.
        /// </summary>
        /// <param name="imageWidth">Image width.</param>
        /// <param name="imageHeight">Image height.</param>
        public JPGRecorder (int imageWidth, int imageHeight) {
            // Save state
            this.framebuffer = new Texture2D(imageWidth, imageHeight, TextureFormat.RGBA32, false, false);
            this.writeTasks = new List<Task>();
            // Create directory
            this.recordingPath = Utility.GetPath(string.Empty);
            Directory.CreateDirectory(recordingPath);
        }

        /// <summary>
        /// Commit a video pixel buffer for encoding.
        /// The pixel buffer MUST have an RGBA8888 pixel layout.
        /// </summary>
        /// <param name="pixelBuffer">Pixel buffer containing video frame to commit.</param>
        /// <param name="timestamp">Not used.</param>
        public void CommitFrame<T> (T[] pixelBuffer, long timestamp = default) where T : struct {
            var handle = GCHandle.Alloc(pixelBuffer, GCHandleType.Pinned);
            CommitFrame(handle.AddrOfPinnedObject(), timestamp);
            handle.Free();
        }

        /// <summary>
        /// Commit a video pixel buffer for encoding.
        /// The pixel buffer MUST have an RGBA8888 pixel layout.
        /// </summary>
        /// <param name="nativeBuffer">Pixel buffer in native memory to commit.</param>
        /// <param name="timestamp">Not used.</param>
        public void CommitFrame (IntPtr nativeBuffer, long timestamp = default) {
            // Encode immediately
            framebuffer.LoadRawTextureData(nativeBuffer, framebuffer.width * framebuffer.height * 4);
            var frameData = ImageConversion.EncodeToJPG(framebuffer);
            // Write out on a worker thread
            var frameIndex = ++frameCount;
            writeTasks.Add(Task.Run(() => File.WriteAllBytes(Path.Combine(recordingPath, $"{frameIndex}.jpg"), frameData)));
        }

        /// <summary>
        /// This recorder does not support committing audio samples.
        /// </summary>
        public void CommitSamples (float[] sampleBuffer = default, long timestamp = default) { }

        /// <summary>
        /// Finish writing and return the path to the recorded media file.
        /// </summary>
        public Task<string> FinishWriting () {
            Texture2D.Destroy(framebuffer);
            return Task.WhenAll(writeTasks).ContinueWith(_ => recordingPath);
        }
        #endregion


        #region --Operations--
        private readonly Texture2D framebuffer;
        private readonly string recordingPath;
        private readonly List<Task> writeTasks;
        private int frameCount;
        #endregion
    }
}