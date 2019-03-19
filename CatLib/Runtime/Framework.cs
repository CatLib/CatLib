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

using UnityEngine;

namespace CatLib
{
    /// <summary>
    /// 框架入口
    /// </summary>
    [DisallowMultipleComponent]
    [HelpURL("https://catlib.io/lasted")]
    public abstract class Framework : MonoBehaviour
    {
        /// <summary>
        /// 调试等级
        /// </summary>
        public DebugLevels DebugLevel = DebugLevels.Production;

        /// <summary>
        /// CatLib Unity Application
        /// </summary>
        private Application application;

        /// <summary>
        /// CatLib Unity Application
        /// </summary>
        public IApplication Application
        {
            get { return application; }
        }

        /// <summary>
        /// 当框架启动完成时
        /// </summary>
        protected abstract void OnStartCompleted();

        /// <summary>
        /// Unity Awake
        /// </summary>
        protected virtual void Awake()
        {
            DontDestroyOnLoad(gameObject);
            application = CreateApplication(DebugLevel);
            BeforeBootstrap(application);
            application.Bootstrap(GetBootstraps());
        }

        /// <summary>
        /// Unity Start
        /// </summary>
        protected virtual void Start()
        {
            application.Init();
        }

        /// <summary>
        /// 创建新的Application实例
        /// </summary>
        /// <param name="debugLevel">调试等级</param>
        /// <returns>Application实例</returns>
        protected virtual Application CreateApplication(DebugLevels debugLevel)
        {
            return new UnityApplication(this)
            {
                DebugLevel = DebugLevel
            };
        }

        /// <summary>
        /// 在引导开始之前
        /// </summary>
        /// <param name="application">应用程序</param>
        protected virtual void BeforeBootstrap(IApplication application)
        {
            application.On(ApplicationEvents.OnStartCompleted, OnStartCompleted);
        }

        /// <summary>
        /// 获取引导程序
        /// </summary>
        /// <returns>引导脚本</returns>
        protected virtual IBootstrap[] GetBootstraps()
        {
            return Arr.Merge(GetComponents<IBootstrap>(), Bootstraps.GetBoostraps(this));
        }

        /// <summary>
        /// 当被释放时
        /// </summary>
        protected virtual void OnDestroy()
        {
            if (application != null)
            {
                application.Terminate();
            }
        }
    }
}
