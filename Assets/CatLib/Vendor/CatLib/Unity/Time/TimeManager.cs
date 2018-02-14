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

namespace CatLib.Time
{
    /// <summary>
    /// 时间服务管理器
    /// 时间服务允许获取一个或者多个不同类型的时间
    /// </summary>
    internal sealed class TimeManager : SingleManager<ITime> , ITimeManager
    {
        /// <summary>
        /// 默认名字
        /// </summary>
        private string defaultName;

        /// <summary>
        /// 设定默认名字
        /// </summary>
        /// <param name="name">默认名字</param>
        public void SetDefault(string name)
        {
            defaultName = name;
        }

        /// <summary>
        /// 从游戏开始到现在所用的时间(秒)
        /// </summary>
        public float Time
        {
            get
            {
                return Default.Time;
            }
        }

        /// <summary>
        /// 上一帧到当前帧的时间(秒)
        /// </summary>
        public float DeltaTime
        {
            get
            {
                return Default.DeltaTime;
            }
        }

        /// <summary>
        /// 从游戏开始到现在的时间（秒）使用固定时间来更新
        /// </summary>
        public float FixedTime
        {
            get
            {
                return Default.FixedTime;
            }
        }

        /// <summary>
        /// 从当前scene开始到目前为止的时间(秒)
        /// </summary>
        public float TimeSinceLevelLoad
        {
            get
            {
                return Default.TimeSinceLevelLoad;
            }
        }

        /// <summary>
        /// 固定的上一帧到当前帧的时间(秒)
        /// </summary>
        public float FixedDeltaTime
        {
            get
            {
                return Default.FixedDeltaTime;
            }
            set
            {
                Default.FixedDeltaTime = value;
            }
        }

        /// <summary>
        /// 能获取最大的上一帧到当前帧的时间(秒)
        /// </summary>
        public float MaximumDeltaTime
        {
            get
            {
                return Default.MaximumDeltaTime;
            }
        }

        /// <summary>
        /// 平稳的上一帧到当前帧的时间(秒)，根据前N帧的加权平均值
        /// </summary>
        public float SmoothDeltaTime
        {
            get
            {
                return Default.SmoothDeltaTime;
            }
        }

        /// <summary>
        /// 时间缩放系数
        /// </summary>
        public float TimeScale
        {
            get
            {
                return Default.TimeScale;
            }
            set
            {
                Default.TimeScale = value;
            }
        }

        /// <summary>
        /// 总帧数
        /// </summary>
        public int FrameCount
        {
            get
            {
                return Default.FrameCount;
            }
        }

        /// <summary>
        /// 自游戏开始后的总时间（暂停也会增加）
        /// </summary>
        public float RealtimeSinceStartup
        {
            get
            {
                return Default.RealtimeSinceStartup;
            }
        }

        /// <summary>
        /// 每秒的帧率
        /// </summary>
        public int CaptureFramerate
        {
            get
            {
                return Default.CaptureFramerate;
            }
            set
            {
                Default.CaptureFramerate = value;
            }
        }

        /// <summary>
        /// 不考虑时间缩放上一帧到当前帧的时间(秒)
        /// </summary>
        public float UnscaledDeltaTime
        {
            get
            {
                return Default.UnscaledDeltaTime;
            }
        }

        /// <summary>
        /// 不考虑时间缩放的从游戏开始到现在的时间
        /// </summary>
        public float UnscaledTime
        {
            get
            {
                return Default.UnscaledTime;
            }
        }

        /// <summary>
        /// 获取默认的文件系统名字
        /// </summary>
        /// <returns>默认的文件系统名字</returns>
        protected override string GetDefaultName()
        {
            return string.IsNullOrEmpty(defaultName) ? "default" : defaultName;
        }
    }
}
