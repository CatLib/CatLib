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
    /// 项目注册的服务提供者
    /// </summary>
    public static class Providers
    {
        /// <summary>
        /// 项目注册的服务提供者
        /// </summary>
        public static IServiceProvider[] ServiceProviders
        {
            get
            {
                return new IServiceProvider[]
                {
                    // todo: 在此处增加您项目的服务提供者
                };
            }
        }
    }
}