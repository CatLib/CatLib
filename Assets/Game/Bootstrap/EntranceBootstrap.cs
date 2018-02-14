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
using CatLib.API.Routing;

namespace Game
{
    /// <summary>
    /// 入口调度服务
    /// </summary>
    public class EntranceBootstrap : IBootstrap
    {
        /// <summary>
        /// 调度入口
        /// </summary>
        public string dispatch;

        /// <summary>
        /// 引导程序接口
        /// </summary>
        [Priority(int.MaxValue - 1)]
        public void Bootstrap()
        {
            App.On(ApplicationEvents.OnStartCompleted, () =>
            {
                if (!string.IsNullOrEmpty(dispatch) && App.HasInstance<IRouter>())
                {
                    App.Make<IRouter>().Dispatch(dispatch);
                }
            });
        }
    }
}
