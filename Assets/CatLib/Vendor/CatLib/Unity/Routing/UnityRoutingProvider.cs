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

using System;
using UnityEngine;

namespace CatLib.Routing
{
    /// <summary>
    /// 路由服务
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("CatLib/Routing")]
    public sealed class UnityRoutingProvider : MonoBehaviour, IServiceProvider, IServiceProviderType
    {
        /// <summary>
        /// 默认的Scheme
        /// </summary>
        public string DefaultScheme = "catlib";

        /// <summary>
        /// 进行路由编译
        /// </summary>
        public bool RouterCompiler = true;

        /// <summary>
        /// 需要编译的程序集
        /// </summary>
        public string[] CompilerAssembly = { };

        /// <summary>
        /// 基础服务提供者
        /// </summary>
        private RoutingProvider baseProvider;

        /// <summary>
        /// 提供者基础类型
        /// </summary>
        public Type BaseType
        {
            get
            {
                return baseProvider.GetType();
            }
        }

        /// <summary>
        /// 构造一个路由服务
        /// </summary>
        public void Awake()
        {
            baseProvider = new RoutingProvider
            {
                DefaultScheme = DefaultScheme,
                CompilerAssembly = CompilerAssembly,
                RouterCompiler = RouterCompiler
            };
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
            baseProvider.Init();
        }

        /// <summary>
        /// 注册路由服务
        /// </summary>
        public void Register()
        {
            baseProvider.Register();
        }
    }
}