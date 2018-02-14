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

using CatLib.API.Time;
using UTime = UnityEngine.Time;

namespace CatLib.Time
{
    /// <summary>
    /// Unity时间系统
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class UnityTime : ITime
    {
        /// <summary>
        /// 从游戏开始到现在所用的时间(秒)
        /// </summary>
        public float Time
        {
            get
            {
                return UTime.time;
            }
        }

        /// <summary>
        /// 上一帧到当前帧的时间(秒)
        /// </summary>
        public float DeltaTime
        {
            get
            {
                return UTime.deltaTime;
            }
        }

        /// <summary>
        /// 从游戏开始到现在的时间（秒）使用固定时间来更新
        /// </summary>
        public float FixedTime
        {
            get
            {
                return UTime.fixedTime;
            }
        }

        /// <summary>
        /// 从当前scene开始到目前为止的时间（秒）
        /// </summary>
        public float TimeSinceLevelLoad
        {
            get
            {
                return UTime.timeSinceLevelLoad;
            }
        }

        /// <summary>
        /// 固定的更新时间（秒）
        /// </summary>
        public float FixedDeltaTime
        {
            get
            {
                return UTime.fixedDeltaTime;
            }
            set
            {
                UTime.fixedDeltaTime = value;
            }
        }

        /// <summary>
        /// 能获取的最大更新时间
        /// </summary>
        public float MaximumDeltaTime
        {
            get
            {
                return UTime.maximumDeltaTime;
            }
        }

        /// <summary>
        /// 平稳的更新时间，根据前N帧的加权平均值
        /// </summary>
        public float SmoothDeltaTime
        {
            get
            {
                return UTime.smoothDeltaTime;
            }
        }

        /// <summary>
        /// 时间缩放系数
        /// </summary>
        public float TimeScale
        {
            get
            {
                return UTime.timeScale;
            }
            set
            {
                UTime.timeScale = value;
            }
        }

        /// <summary>
        /// 总帧数
        /// </summary>
        public int FrameCount
        {
            get
            {
                return UTime.frameCount;
            }
        }

        /// <summary>
        /// 自游戏开始后的总时间（暂停也会增加）
        /// </summary>
        public float RealtimeSinceStartup
        {
            get
            {
                return UTime.realtimeSinceStartup;
            }
        }

        /// <summary>
        /// 每秒的帧率
        /// </summary>
        public int CaptureFramerate
        {
            get
            {
                return UTime.captureFramerate;
            }
            set
            {
                UTime.captureFramerate = value;
            }
        }

        /// <summary>
        /// 不考虑时间缩放的更新时间
        /// </summary>
        public float UnscaledDeltaTime
        {
            get
            {
                return UTime.unscaledDeltaTime;
            }
        }

        /// <summary>
        /// 不考虑时间缩放的从游戏开始到现在的时间
        /// </summary>
        public float UnscaledTime
        {
            get
            {
                return UTime.unscaledTime;
            }
        }
    }
}