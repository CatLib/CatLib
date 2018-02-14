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

namespace CatLib
{
    /// <summary>
    /// 框架默认的引导程序
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class Bootstrap
    {
        /// <summary>
        /// 引导程序
        /// </summary>
        public static IBootstrap[] Bootstraps
        {
            get
            {
                return Arr.Merge(new IBootstrap[]
                {
                    new TypeFinderBootstrap(),
                    new ProviderBootstrap(),
                }, Game.Bootstrap.Bootstraps);
            }
        }
    }
}
