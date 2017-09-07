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

namespace CatLib.API.MonoDriver
{
    /// <summary>
    /// Mono驱动器事件
    /// </summary>
    public sealed class MonoDriverEvents
    {
        /// <summary>
        /// 当释放之前
        /// </summary>
        public static readonly string OnBeforeDestroy = "CatLib.API.MonoDriver.OnBeforeDestroy";

        /// <summary>
        /// 当释放完成后
        /// </summary>
        public static readonly string OnDestroyed = "CatLib.API.MonoDriver.OnDestroyed";
    }
}