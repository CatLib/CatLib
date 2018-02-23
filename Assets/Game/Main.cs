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

using CatLib;
using UnityEngine;
using ILogger = CatLib.API.Debugger.ILogger;

namespace Game
{
    /// <summary>
    /// 项目入口
    /// </summary>
    [DisallowMultipleComponent]
    public class Main : Framework
    {
        /// <summary>
        /// 项目入口
        /// </summary>
        protected override void OnStartCompleted()
        {
            // called this function after, use App.Make function to get service
            // ex: App.Make<ILogger>().Debug("hello world");
            // all can make service see : http://catlib.io/v1/guide/can-make.html

            Debug.Log("Hello CatLib");
        }

        /// <summary>
        /// 当引导完成后
        /// </summary>
        protected override void OnBootstraped()
        {
            base.OnBootstraped();
            Debug.Log("OnBootstraped");
        }

        /// <summary>
        /// 当服务提供者初始化之前
        /// </summary>
        /// <param name="provider">准备初始化的服务提供者</param>
        protected override void OnIniting(IServiceProvider provider)
        {
            base.OnIniting(provider);
            Debug.Log("Initing Provider [<color=#00ff00>" + provider + "</color>]");
        }

        /// <summary>
        /// 当框架终止之前
        /// </summary>
        protected override void OnTerminate()
        {
            base.OnTerminate();
            Debug.Log("OnTerminate");
        }

        /// <summary>
        /// 当框架终止之后
        /// </summary>
        protected override void OnTerminated()
        {
            base.OnTerminated();
            Debug.Log("OnTerminated");
        }
    }
}
