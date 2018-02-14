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

using CatLib.API.Time;

namespace CatLib.Time
{
    /// <summary>
    /// 时间服务
    /// </summary>
    internal sealed class TimeProvider : IServiceProvider
    {
        /// <summary>
        /// 默认时间的名字
        /// </summary>
        public string DefaultTime { get; set; }

        /// <summary>
        /// 时间服务
        /// </summary>
        public TimeProvider()
        {
            DefaultTime = "default";
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
        }

        /// <summary>
        /// 注册时间服务
        /// </summary>
        public void Register()
        {
            RegisterTimeManager();
        }

        /// <summary>
        /// 注册时间服务管理器
        /// </summary>
        private void RegisterTimeManager()
        {
            App.Singleton<TimeManager>().Alias<ITimeManager>().Alias<ITime>().OnResolving((bind, obj) =>
            {
                var timeManager = (TimeManager)obj;

                timeManager.Extend(() => new UnityTime());
                timeManager.SetDefault(DefaultTime);

                return timeManager;
            });
        }
    }
}