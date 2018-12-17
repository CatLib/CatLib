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
    /// 框架默认的服务提供者
    /// <para>这里的提供者在框架启动时必定会被加载</para>
    /// </summary>
    internal class Providers
    {
        /// <summary>
        /// 服务提供者
        /// </summary>
        public static IServiceProvider[] ServiceProviders
        {
            get
            {
                return new IServiceProvider[]
                {

                };
            }
        }
    }
}
