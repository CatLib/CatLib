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
    /// CatLib for unity application
    /// </summary>
    public class UnityApplication : Application
    {
        /// <summary>
        /// behaviour
        /// </summary>
        private readonly MonoBehaviour behaviour;

        /// <summary>
        /// 构造一个 CatLib for unity application
        /// </summary>
        /// <param name="behaviour">驱动器</param>
        public UnityApplication(MonoBehaviour behaviour)
        {
            this.Singleton<MonoBehaviour>(() => behaviour)
                .Alias<Component>();
            this.behaviour = behaviour;
        }

        /// <summary>
        /// 初始化服务提供者
        /// </summary>
        public override void Init()
        {
            behaviour.StartCoroutine(CoroutineInit());
        }

        /// <summary>
        /// 注册服务提供者
        /// </summary>
        /// <param name="provider">服务提供者</param>
        public override void Register(IServiceProvider provider)
        {
            behaviour.StartCoroutine(CoroutineRegister(provider));
        }
    }
}