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
    /// Unity Application
    /// </summary>
    internal sealed class UnityApplication : Application
    {
        /// <summary>
        /// Unity Application
        /// </summary>
        /// <param name="component">根组件</param>
        public UnityApplication(Component component)
        {
            Instance(Type2Service(typeof(Component)), component);
        }
    }
}
