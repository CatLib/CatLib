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
using CatLib.API.Random;
using UnityEngine;

namespace CatLib.Random
{
    /// <summary>
    /// 路由服务
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("CatLib/Random")]
    public sealed class UnityRandomProvider : MonoBehaviour, IServiceProvider, IServiceProviderType
    {
        /// <summary>
        /// 默认的随机算法
        /// </summary>
        public string DefaultRandomType = RandomTypes.MersenneTwister;

        /// <summary>
        /// 基础服务提供者
        /// </summary>
        private RandomProvider baseProvider;

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
        /// Unity Awake
        /// </summary>
        public void Awake()
        {
            baseProvider = new RandomProvider
            {
                DefaultRandomType = DefaultRandomType
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