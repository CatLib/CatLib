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

using CatLib.Facade;

namespace CatLib
{
    /// <summary>
    /// 默认提供者引导
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class StartBootstrap : IBootstrap
    {
        /// <summary>
        /// 引导程序接口
        /// </summary>
        public void Bootstrap()
        {
            App.On(ApplicationEvents.OnStartCompleted, (payload) =>
            {
                if (Router.Instance != null)
                {
                    Router.Instance.Dispatch("bootstrap://start");
                }
            });
        }
    }
}
