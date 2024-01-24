/* 
*   NatCorder
*   Copyright (c) 2020 Yusuf Olokoba.
*/

namespace NatSuite.Recorders {

    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// A recorder capable of recording video frames, and optionally audio frames, to a media output.
    /// All recorder methods are thread safe, and as such can be called from any thread.
    /// </summary>
    public interface IMediaRecorder {

        /// <summary>
        /// Recording frame size.
        /// </summary>
        (int width, int height) frameSize { get; }

        /// <summary>
        /// Commit a video pixel buffer for encoding.
        /// The pixel buffer MUST have an RGBA8888 pixel layout.
        /// </summary>
        /// <param name="pixelBuffer">Pixel buffer to commit.</param>
        /// <param name="timestamp">Sample buffer timestamp in nanoseconds.</param>
        void CommitFrame<T> (T[] pixelBuffer, long timestamp) where T : struct;

        /// <summary>
        /// Commit a video pixel buffer for encoding.
        /// The pixel buffer MUST have an RGBA8888 pixel layout.
        /// </summary>
        /// <param name="nativeBuffer">Pixel buffer in native memory to commit.</param>
        /// <param name="timestamp">Sample buffer timestamp in nanoseconds.</param>
        void CommitFrame (IntPtr nativeBuffer, long timestamp);
        
        /// <summary>
        /// Commit an audio sample buffer for encoding.
        /// </summary>
        /// <param name="sampleBuffer">Linear PCM audio sample buffer, interleaved by channel.</param>
        /// <param name="timestamp">Sample buffer timestamp in nanoseconds.</param>
        void CommitSamples (float[] sampleBuffer, long timestamp);

        /// <summary>
        /// Finish writing and return the path to the recorded media file.
        /// </summary>
        Task<string> FinishWriting ();
    }
}