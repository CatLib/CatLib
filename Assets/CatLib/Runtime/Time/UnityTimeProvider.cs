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

namespace CatLib.Time
{
    /// <summary>
    /// 路由服务
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("CatLib/Time")]
    public sealed class UnityTimeProvider : MonoBehaviour, IServiceProvider, IServiceProviderType
    {
        /// <summary>
        /// 默认的时间名字
        /// </summary>
        public string DefaultTime = "default";

        /// <summary>
        /// 基础服务提供者
        /// </summary>
        private TimeProvider baseProvider;

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
            baseProvider = new TimeProvider
            {
                DefaultTime = DefaultTime
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