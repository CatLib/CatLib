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
using System.Collections;

namespace CatLib.API.MonoDriver
{
    /// <summary>
    /// Mono驱动器
    /// </summary>
    public interface IMonoDriver
    {
        /// <summary>
        /// 在主线程中调用
        /// </summary>
        /// <param name="action">协程，执行会处于主线程</param>
        void MainThread(IEnumerator action);

        /// <summary>
        /// 在主线程中调用
        /// </summary>
        /// <param name="action">回调，回调的内容会处于主线程</param>
        void MainThread(Action action);

        /// <summary>
        /// 启动协程
        /// </summary>
        /// <param name="routine">协程</param>
        UnityEngine.Coroutine StartCoroutine(IEnumerator routine);

        /// <summary>
        /// 停止协程
        /// </summary>
        /// <param name="routine">协程</param>
        void StopCoroutine(IEnumerator routine);

        /// <summary>
        /// 从驱动器中卸载对象
        /// 如果对象使用了增强接口，那么卸载对应增强接口
        /// 从驱动器中卸载对象会引发IDestroy增强接口
        /// </summary>
        /// <param name="obj">对象</param>
        /// <exception cref="ArgumentNullException">当卸载对象为<c>null</c>时引发</exception>
        void Detach(object obj);

        /// <summary>
        /// 如果对象实现了增强接口那么将对象装载进对应驱动器
        /// </summary>
        /// <param name="obj">对象</param>
        /// <exception cref="ArgumentNullException">当装载对象为<c>null</c>时引发</exception>
        void Attach(object obj);
    }
}
