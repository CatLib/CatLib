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

namespace CatLib
{
    /// <summary>
    /// 在Update之后调用
    /// </summary>
    public interface ILateUpdate
    {
        /// <summary>
        /// LateUpdate时调用
        /// </summary>
        void LateUpdate();
    }
}