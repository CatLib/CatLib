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

using UnityEngine;

namespace CatLib
{
    /// <summary>
    /// 框架入口
    /// </summary>
    [DisallowMultipleComponent]
    public abstract class Framework : MonoBehaviour, IBootstrap
    {
        /// <summary>
        /// CatLib Unity Framework
        /// </summary>
        private Application application;

        /// <summary>
        /// CatLib Unity Framework
        /// </summary>
        protected IApplication Application
        {
            get { return application; }
        }

        /// <summary>
        /// 入口引导
        /// </summary>
        [Priority]
        public virtual void Bootstrap()
        {
            App.On(ApplicationEvents.OnStartCompleted, OnStartCompleted);
            App.On(ApplicationEvents.OnBootstraped, OnBootstraped);
            App.On<IServiceProvider>(ApplicationEvents.OnIniting, OnIniting);
            App.On(ApplicationEvents.OnTerminate, OnTerminate);
            App.On(ApplicationEvents.OnTerminated, OnTerminated);
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
            application = new UnityApplication(this);
            application.Bootstrap(GetBootstraps());
            application.Init();
        }

        /// <summary>
        /// 当所有引导完成时
        /// </summary>
        protected virtual void OnBootstraped()
        {
        }

        /// <summary>
        /// 当终止框架之前
        /// </summary>
        protected virtual void OnTerminate()
        {

        }

        /// <summary>
        /// 当终止框架完成后
        /// </summary>
        protected virtual void OnTerminated()
        {

        }

        /// <summary>
        /// 当服务提供者初始化之前
        /// </summary>
        /// <param name="provider">准备初始化的服务提供者</param>
        protected virtual void OnIniting(IServiceProvider provider)
        {

        }

        /// <summary>
        /// 获取引导程序
        /// </summary>
        /// <returns>引导脚本</returns>
        protected virtual IBootstrap[] GetBootstraps()
        {
            return Arr.Merge(GetComponents<IBootstrap>(), Bootstraps.Bootstrap);
        }

        /// <summary>
        /// 当被释放时
        /// </summary>
        protected virtual void OnDestroy()
        {
            application.Terminate();
        }
    }
}
