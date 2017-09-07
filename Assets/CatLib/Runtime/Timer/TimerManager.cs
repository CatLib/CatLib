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
    /// 计时器管理器
    /// </summary>
    internal sealed class TimerManager : Manager<ITimerQueue> , ITimerManager, IUpdate
    {
        /// <summary>
        /// 时间管理器
        /// </summary>
        private readonly ITimeManager timeManager;

        /// <summary>
        /// 运行列表
        /// </summary>
        private readonly SortSet<TimerQueue, int> executeList;

        /// <summary>
        /// 计时器队列堆栈
        /// </summary>
        private readonly Stack<TimerQueue> timerQueue;

        /// <summary>
        /// 构建一个计时器管理器
        /// </summary>
        /// <param name="timeManager">时间管理器</param>
        public TimerManager([Inject(Required = true)]ITimeManager timeManager)
        {
            this.timeManager = timeManager;
            executeList = new SortSet<TimerQueue, int>();
            timerQueue = new Stack<TimerQueue>();
        }

        /// <summary>
        /// 创建一个计时器
        /// </summary>
        /// <param name="task">计时器执行的任务</param>
        /// <returns>计时器</returns>
        public ITimer Make(Action task = null)
        {
            var withGroupStack = timerQueue.Count > 0;
            var queue = withGroupStack
                ? timerQueue.Peek()
                : new TimerQueue(timeManager.Default);
            var timer = new Timer(queue, task);
            queue.Add(timer);
            if (!withGroupStack)
            {
                executeList.Add(queue, int.MaxValue);
            }
            return timer;
        }

        /// <summary>
        /// 创建一个计时器队列
        /// </summary>
        /// <param name="area">在这个区域中Make的计时器会按照Make顺序加入同一个队列</param>
        /// <param name="priority">优先级(值越小越优先)</param>
        /// <returns>计时器队列</returns>
        public ITimerQueue MakeQueue(Action area, int priority = int.MaxValue)
        {
            Guard.NotNull(area, "area");
            var queue = new TimerQueue(timeManager.Default);
            timerQueue.Push(queue);
            try
            {
                area.Invoke();
            }
            finally
            {
                timerQueue.Pop();
            }

            executeList.Add(queue, priority);
            return queue;
        }

        /// <summary>
        /// 停止计时器队列的运行
        /// </summary>
        /// <param name="queue">计时器队列</param>
        public void Cancel(ITimerQueue queue)
        {
            var timerQueue = queue as TimerQueue;
            Guard.NotNull(timerQueue, "timerQueue");
            executeList.Remove(timerQueue);
        }

        /// <summary>
        /// 暂停计时器队列
        /// </summary>
        /// <param name="queue">计时器队列</param>
        public void Pause(ITimerQueue queue)
        {
            var timerQueue = queue as TimerQueue;
            Guard.NotNull(timerQueue, "timerQueue");
            timerQueue.IsPause = true;
        }

        /// <summary>
        /// 重新开始播放计时器队列
        /// </summary>
        /// <param name="queue">计时器队列</param>
        public void Play(ITimerQueue queue)
        {
            var timerQueue = queue as TimerQueue;
            Guard.NotNull(timerQueue, "timerQueue");
            timerQueue.IsPause = false;
        }

        /// <summary>
        /// 每帧更新
        /// </summary>
        public void Update()
        {
            foreach (var queue in executeList)
            {
                if (queue.Tick())
                {
                    executeList.Remove(queue);
                }
            }
        }
    }
}
