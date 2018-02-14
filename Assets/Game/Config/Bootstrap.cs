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

using CatLib;

namespace Game
{
    /// <summary>
    /// 项目注册的引导程序
    /// </summary>
    public class Bootstrap
    {
        /// <summary>
        /// 项目注册的引导程序
        /// </summary>
        public static IBootstrap[] Bootstraps
        {
            get
            {
                return new IBootstrap[]
                {
                    new EntranceBootstrap { /*dispatch = "game://start"*/ },
                    new Main(), 

                    // todo: 请在此处增加您项目的引导程序
                };
            }
        }
    }
}
