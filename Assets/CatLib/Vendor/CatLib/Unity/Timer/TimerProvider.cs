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

using CatLib.API.Timer;

namespace CatLib.Timer
{
    /// <summary>
    /// 计时器服务
    /// </summary>
    public sealed class TimerProvider : IServiceProvider
    {
        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
        }

        /// <summary>
        /// 注册计时器服务
        /// </summary>
        public void Register()
        {
            App.Singleton<TimerManager>().Alias<ITimerManager>();
        }
    }
}