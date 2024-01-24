/* 
*   NatCorder
*   Copyright (c) 2020 Yusuf Olokoba.
*/

namespace NatSuite.Recorders.Clocks {

    /// <summary>
    /// Clock for generating recording timestamps.
    /// Clocks are important for synchronizing audio and video tracks when recording with audio.
    /// Clocks are thread-safe, so they can be used on multiple threads simultaneously.
    /// </summary>
    public interface IClock {
        
        /// <summary>
        /// Current timestamp in nanoseconds.
        /// </summary>
        long timestamp { get; }
    }
}