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

using UnityEngine;

namespace CatLib
{
    /// <summary>
    /// Unity Application
    /// </summary>
    internal sealed class UnityApplication : Application
    {
        /// <summary>
        /// 基础组件
        /// </summary>
        private readonly MonoBehaviour component;

        /// <summary>
        /// Unity Application
        /// </summary>
        /// <param name="component">根组件</param>
        public UnityApplication(MonoBehaviour component)
        {
            this.component = component;
            Instance(Type2Service(typeof(MonoBehaviour)), component);
            Instance(Type2Service(typeof(Component)), component);
            Bootstrap(GetBootstraps());
        }

        /// <summary>
        /// 获取引导程序
        /// </summary>
        /// <returns>引导脚本</returns>
        private IBootstrap[] GetBootstraps()
        {
            return Arr.Merge(component.GetComponents<IBootstrap>(), global::CatLib.Bootstrap.Bootstraps);
        }
    }
}
