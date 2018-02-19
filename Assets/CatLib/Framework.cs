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
    public class Framework : MonoBehaviour
    {
        /// <summary>
        /// CatLib Unity Framework
        /// </summary>
        protected Application application;

        /// <summary>
        /// Unity Start
        /// </summary>
        protected virtual void Start()
        {
            application = new UnityApplication(this);
            application.Init();
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
