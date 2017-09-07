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
    /// 当被释放时
    /// </summary>
    public interface IOnDestroy
    {
        /// <summary>
        /// 当被释放时调用
        /// </summary>
        void OnDestroy();
    }
}