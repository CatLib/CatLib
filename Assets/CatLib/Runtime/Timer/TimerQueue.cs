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
using CatLib.API.Timer;
using System;
using System.Collections.Generic;

namespace CatLib.Timer
{
    /// <summary>
    /// 计时器队列
    /// </summary>
    internal sealed class TimerQueue : ITimerQueue
    {
        /// <summary>
        /// 是否是暂停的
        /// </summary>
        public bool IsPause { get; internal set; }

        /// <summary>
        /// 时间实现
        /// </summary>
        private ITime time;

        /// <summary>
        /// 计时器列表
        /// </summary>
        private readonly List<Timer> timers;

        /// <summary>
        /// 当队列中的所有任务完成时
        /// </summary>
        private Action onCompleted;

        /// <summary>
        /// 游标,确定了当前执行的timer位置
        /// </summary>
        private int cursor;

        /// <summary>
        /// 构建时间组时的逻辑帧
        /// </summary>
        private readonly int frame;

        /// <summary>
        /// 当前计时器队列是否完成的
        /// </summary>
        private bool IsCompleted
        {
            get
            {
                return cursor >= timers.Count;
            }
        }

        /// <summary>
        /// 构建一个计时器队列
        /// </summary>
        /// <param name="time">时间实现</param>
        public TimerQueue(ITime time)
        {
            this.time = time;
            timers = new List<Timer>();
            cursor = 0;
            frame = time.FrameCount;
            IsPause = false;
        }

        /// <summary>
        /// 当组的所有计时器完成时
        /// </summary>
        /// <param name="onCompleted">完成时</param>
        /// <returns>当前组实例</returns>
        public ITimerQueue OnCompleted(Action onCompleted)
        {
            GuardCompleted("OnCompleted");
            this.onCompleted = onCompleted;
            return this;
        }

        /// <summary>
        /// 设定使用的时间系统
        /// </summary>
        /// <param name="time">时间</param>
        /// <returns>当前组实例</returns>
        public ITimerQueue SetTime(ITime time)
        {
            this.time = time;
            return this;
        }

        /// <summary>
        /// 触发计时器
        /// </summary>
        /// <returns>计时器队列是否已经完成</returns>
        internal bool Tick()
        {
            if (IsPause || frame >= time.FrameCount)
            {
                return IsCompleted;
            }

            var deltaTime = time.DeltaTime;
            Timer timer;
            while ((timer = GetTimer()) != null)
            {
                if (!timer.Tick(ref deltaTime))
                {
                    break;
                }
                ++cursor;
            }

            if (!IsCompleted)
            {
                return false;
            }

            if (onCompleted != null)
            {
                onCompleted.Invoke();
            }

            return true;
        }

        /// <summary>
        /// 将计时器加入队列
        /// </summary>
        /// <param name="timer">计时器</param>
        internal void Add(Timer timer)
        {
            timers.Add(timer);
        }

        /// <summary>
        /// 获取计时器
        /// </summary>
        /// <returns>计时器</returns>
        private Timer GetTimer()
        {
            return !IsCompleted ? timers[cursor] : null;
        }

        /// <summary>
        /// 检测完成状态
        /// </summary>
        /// <param name="func">函数名</param>
        private void GuardCompleted(string func)
        {
            if (IsCompleted)
            {
                throw new RuntimeException("Timer Queue is completed , Can not call " + func + "();");
            }
        }
    }
}
