/*
 * This file is part of the CatLib package.
 *
 * (c) CatLib <support@catlib.io>
 *
 * For the full copyright and license information, please view the LICENSE
 * file that was distributed with this source code.
 *
 * Document: http://catlib.io/
 */

using CatLib;
using CatLib.Editor;

namespace Demo.Editor
{
    /// <summary>
    /// 编辑器项目入口
    /// </summary>
    public class EditorMain : EditorFramework
    {
        /// <summary>
        /// 获取引导程序
        /// </summary>
        /// <returns>引导脚本</returns>
        protected override IBootstrap[] GetBootstraps()
        {
            return Arr.Merge(base.GetBootstraps(), Bootstraps.GetBoostraps(null));
        }
    }
}