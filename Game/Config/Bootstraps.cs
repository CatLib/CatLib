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

namespace Game
{
    /// <summary>
    /// 项目注册的引导程序
    /// </summary>
    public static class Bootstraps
    {
        /// <summary>
        /// 项目注册的引导程序
        /// </summary>
        public static IBootstrap[] Bootstrap
        {
            get
            {
                return new IBootstrap[]
                {
                    new BootstrapTypeFinder(Assemblys.Assembly),
                    new BootstrapProviderRegister(Providers.ServiceProviders),
                };
            }
        }
    }
}
