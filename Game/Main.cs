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
            // Application entry, Your code starts writing here
            // called this function after, use App.Make function to get service
            // ex: App.Make<IYourService>().Debug("hello world");

            Debug.Log("Hello CatLib");
        }

        /// <summary>
        /// 当服务提供者初始化之前
        /// </summary>
        /// <param name="provider">准备初始化的服务提供者</param>
        protected override void OnProviderInit(IServiceProvider provider)
        {
            base.OnProviderInit(provider);
            Debug.Log("Initing Provider [<color=#00ff00>" + provider + "</color>]");
        }

        /// <summary>
        /// 当框架终止之前
        /// </summary>
        protected override void OnTerminate()
        {
            base.OnTerminate();
            Debug.Log("<color=#00ffff>OnTerminate</color>");
        }

        /// <summary>
        /// 当框架终止之后
        /// </summary>
        protected override void OnTerminated()
        {
            base.OnTerminated();
            Debug.Log("<color=#ff0000>OnTerminated</color>");
        }

        /// <summary>
        /// 获取引导程序
        /// </summary>
        /// <returns>引导脚本</returns>
        protected override IBootstrap[] GetBootstraps()
        {
            return Arr.Merge(base.GetBootstraps(), Bootstraps.Bootstrap);
        }
    }
}
