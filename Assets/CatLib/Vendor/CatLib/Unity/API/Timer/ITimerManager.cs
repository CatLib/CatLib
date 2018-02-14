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

using System;

namespace CatLib.API.Timer
{
    /// <summary>
    /// 计时器管理器
    /// </summary>
    public interface ITimerManager
    {
        /// <summary>
        /// 创建一个计时器
        /// </summary>
        /// <param name="task">计时器执行的任务</param>
        /// <returns>计时器</returns>
        ITimer Make(Action task = null);

        /// <summary>
        /// 创建一个计时器队列
        /// </summary>
        /// <param name="area">在这个区域中Make的计时器会按照Make顺序加入同一个队列</param>
        /// <param name="priority">优先级(值越小越优先)</param>
        /// <returns>路由器组</returns>
        ITimerQueue MakeQueue(Action area, int priority = int.MaxValue);

        /// <summary>
        /// 停止计时器队列的运行
        /// </summary>
        /// <param name="queue">计时器队列</param>
        void Cancel(ITimerQueue queue);

        /// <summary>
        /// 暂停计时器队列
        /// </summary>
        /// <param name="queue">计时器队列</param>
        void Pause(ITimerQueue queue);

        /// <summary>
        /// 重新开始播放计时器队列
        /// </summary>
        /// <param name="queue">计时器队列</param>
        void Play(ITimerQueue queue);
    }
}
