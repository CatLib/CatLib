/*
 * This file is part of the CatLib package.
 *
 * (c) Yu Bin <support@catlib.io>
 *
 * For the full copyright and license information, please view the LICENSE
 * file that was distributed with this source code.
 *
 * Document: http://catlib.io/
 */

namespace CatLib.API.Time
{
    /// <summary>
    /// 时间
    /// </summary>
    public interface ITime
    {
        /// <summary>
        /// 从游戏开始到现在所用的时间(秒)
        /// </summary>
        float Time { get; }

        /// <summary>
        /// 上一帧到当前帧的时间(秒)
        /// </summary>
        float DeltaTime { get; }

        /// <summary>
        /// 从游戏开始到现在的时间（秒）使用固定时间来更新
        /// </summary>
        float FixedTime { get; }

        /// <summary>
        /// 从当前scene开始到目前为止的时间(秒)
        /// </summary>
        float TimeSinceLevelLoad { get; }

        /// <summary>
        /// 固定的上一帧到当前帧的时间(秒)
        /// </summary>
        float FixedDeltaTime { get; set; }

        /// <summary>
        /// 能获取最大的上一帧到当前帧的时间(秒)
        /// </summary>
        float MaximumDeltaTime { get; }

        /// <summary>
        /// 平稳的上一帧到当前帧的时间(秒)，根据前N帧的加权平均值
        /// </summary>
        float SmoothDeltaTime { get; }

        /// <summary>
        /// 时间缩放系数
        /// </summary>
        float TimeScale { get; set; }

        /// <summary>
        /// 总帧数
        /// </summary>
        int FrameCount { get; }

        /// <summary>
        /// 自游戏开始后的总时间（暂停也会增加）
        /// </summary>
        float RealtimeSinceStartup { get; }

        /// <summary>
        /// 每秒的帧率
        /// </summary>
        int CaptureFramerate { get; set; }

        /// <summary>
        /// 不考虑时间缩放上一帧到当前帧的时间(秒)
        /// </summary>
        float UnscaledDeltaTime { get; }

        /// <summary>
        /// 不考虑时间缩放的从游戏开始到现在的时间
        /// </summary>
        float UnscaledTime { get; }
    }
}