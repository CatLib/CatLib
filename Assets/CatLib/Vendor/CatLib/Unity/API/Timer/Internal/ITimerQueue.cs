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
using CatLib.API.Time;

namespace CatLib.API.Timer
{
    /// <summary>
    /// 计时器任务队列
    /// </summary>
    public interface ITimerQueue
    {
        /// <summary>
        /// 是否是暂停的
        /// </summary>
        bool IsPause { get; }

        /// <summary>
        /// 当队列的所有计时器完成时
        /// </summary>
        /// <param name="onCompleted">完成时</param>
        /// <returns>当前组实例</returns>
        ITimerQueue OnCompleted(Action onCompleted);

        /// <summary>
        /// 设定使用的时间系统
        /// </summary>
        /// <param name="time">时间</param>
        /// <returns>当前组实例</returns>
        ITimerQueue SetTime(ITime time);
    }
}
