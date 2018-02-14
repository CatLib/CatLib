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
    [AddComponentMenu("CatLib/Framework")]
    public sealed class CatLib : MonoBehaviour
    {
        /// <summary>
        /// CatLib Unity Framework
        /// </summary>
        private UnityApplication framework;

        /// <summary>
        /// Unity Start
        /// </summary>
        private void Start()
        {
            framework = new UnityApplication(this);
            framework.Init();
        }

        /// <summary>
        /// 当被释放时
        /// </summary>
        private void OnDestroy()
        {
            framework.Terminate();
        }
    }
}
