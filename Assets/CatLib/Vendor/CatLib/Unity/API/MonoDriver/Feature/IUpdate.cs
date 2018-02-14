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
    /// Update 接口
    /// </summary>
    public interface IUpdate
    {
        /// <summary>
        /// 当Update时调用
        /// </summary>
        void Update();
    }
}