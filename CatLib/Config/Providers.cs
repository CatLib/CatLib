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
    [ExcludeFromCodeCoverage]
    internal class Providers
    {
        /// <summary>
        /// 服务提供者
        /// </summary>
        public static IServiceProvider[] ServiceProviders
        {
            get
            {
                // 请不要手动修改这里的内容
                // Vendor中的第三方服务提供者或者框架必须的服务提供者会被放在这里
                return Arr.Merge(new IServiceProvider[]
                {
                    
                }, Game.Providers.ServiceProviders);
            }
        }
    }
}
