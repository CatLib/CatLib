/*
 * This file is part of the CatLib package.
 *
 * (c) CatLib <support@catlib.io>
 *
 * For the full copyright and license information, please view the LICENSE
 * file that was distributed with this source code.
 *
 * Document: http://catlib.io/
 */

using CatLib;
using UnityEngine;

namespace Demo.Editor
{
    /// <summary>
    /// 项目注册的引导程序
    /// </summary>
    public static class Bootstraps
    {
        /// <summary>
        /// 获取引导程序
        /// </summary>
        /// <param name="component">Unity组件</param>
        /// <returns>引导程序</returns>
        public static IBootstrap[] GetBoostraps(Component component)
        {
            return new IBootstrap[]
            {
                new BootstrapTypeFinder(Assemblys.Assembly),
                new BootstrapProviderRegister(component, Providers.ServiceProviders),
            };
        }
    }
}
