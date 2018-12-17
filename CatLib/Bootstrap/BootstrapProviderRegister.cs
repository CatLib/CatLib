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

using System.Collections.Generic;
using UnityEngine;

namespace CatLib
{
    /// <summary>
    /// 服务提供者引导程序
    /// </summary>
    [ExcludeFromCodeCoverage]
    [Priority(20)]
    public sealed class BootstrapProviderRegister : IBootstrap
    {
        /// <summary>
        /// 允许从组件列表加载
        /// </summary>
        public bool LoadFromComponents { get; set; }

        /// <summary>
        /// 服务提供者列表
        /// </summary>
        private readonly IServiceProvider[] providers;

        /// <summary>
        /// 构建一个服务提供者引导程序
        /// </summary>
        /// <param name="serviceProviders">服务提供者列表</param>
        public BootstrapProviderRegister(IServiceProvider[] serviceProviders = null)
        {
            providers = Arr.Merge(Providers.ServiceProviders, serviceProviders);
            LoadFromComponents = true;
        }

        /// <summary>
        /// 引导程序接口
        /// </summary>
        public void Bootstrap()
        {
            LoadUnityComponentProvider();
            LoadCodeProvider();
        }

        /// <summary>
        /// 加载以代码形式提供的服务提供者
        /// </summary>
        private void LoadCodeProvider()
        {
            RegisterProviders(providers);
        }

        /// <summary>
        /// 加载Unity组件的服务提供者
        /// </summary>
        private void LoadUnityComponentProvider()
        {
            if (!LoadFromComponents)
            {
                return;
            }

            var root = App.Make<Component>();
            if (root == null)
            {
                return;
            }

            RegisterProviders(root.GetComponentsInChildren<IServiceProvider>());
        }

        /// <summary>
        /// 注册服务提供者
        /// </summary>
        /// <param name="providers">服务提供者</param>
        private static void RegisterProviders(IEnumerable<IServiceProvider> providers)
        {
            foreach (var provider in providers)
            {
                if (provider == null)
                {
                    continue;
                }

                if (!App.IsRegisted(provider))
                {
                    App.Register(provider);
                }
            }
        }
    }
}
